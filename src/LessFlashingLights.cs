using System.Collections;
using Modding;
using Modding.Utils;
using Satchel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LessFlashingLights
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LessFlashingLights : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
    {
        public new string GetName() => "Less Flashing Lights";
        public override string GetVersion() => "1.0.0.12";
        
        public static GlobalSettings Gs { get; private set; } = new();
        
        public bool ToggleButtonInsideMenu => true;
        
        private Scene _dontDestroyOnLoadScene;
        private GameObject? _emptyGo;
        private bool _ghostExploding;//I do not remember what was the logic behind this, but I'm too scared to remove it

        public override void Initialize()
        {
            ModHooks.OnEnableEnemyHook += OnEnableEnemy;
            ModHooks.ObjectPoolSpawnHook += OnObjectSpawn;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
            
            On.HeroController.Awake += OnHeroAwake;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            On.InvulnerablePulse.startInvulnerablePulse += OnInvulnerablePulse;
            On.WaveEffectControl.OnEnable += OnWaveEffectStart;
            On.BossStatueDreamToggle.Fade += OnDreamToggleFade;
            On.DreamPlantOrb.Start += OnDreamPlantOrbStart;
            On.BossStatueTrophyPlaque.DoTierCompleteEffect += OnBossTrophyTierCompleteEffect;
            On.BossStatueFlashEffect.FlashRoutine += OnBossStatueFlashRoutine;
            
            On.SpriteFlash.flashArmoured += FlashHandler.OnFlashArmoured;
            On.SpriteFlash.flashBenchRest += FlashHandler.OnFlashBench;
            On.SpriteFlash.flashDreamImpact += FlashHandler.OnFlashDream;
            On.SpriteFlash.flashDungQuick += FlashHandler.OnFlashDungQuick;
            On.SpriteFlash.flashFocusGet += FlashHandler.OnFlashFocusGet;
            On.SpriteFlash.flashFocusHeal += FlashHandler.OnFlashHeal;
            On.SpriteFlash.FlashGrimmflame += FlashHandler.OnFlashGrimmFlame;
            On.SpriteFlash.FlashGrimmHit += FlashHandler.OnFlashGrimmHit;
            On.SpriteFlash.flashHealBlue += FlashHandler.OnFlashHealBlue;
            On.SpriteFlash.flashInfected += FlashHandler.OnFlashInfected;
            On.SpriteFlash.FlashingSuperDash += FlashHandler.OnFlashSuperDash;
            On.SpriteFlash.flashShadeGet += FlashHandler.OnFlashShadeGet;
            On.SpriteFlash.flashSporeQuick += FlashHandler.OnFlashSpore;
            On.SpriteFlash.flashWhitePulse += FlashHandler.OnFlashWhitePulse;
        }

        public void Unload()
        {
            ModHooks.OnEnableEnemyHook -= OnEnableEnemy;
            ModHooks.ObjectPoolSpawnHook -= OnObjectSpawn;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChange;
            
            On.HeroController.Awake -= OnHeroAwake;
            On.PlayMakerFSM.OnEnable -= OnFsmEnable;
            On.InvulnerablePulse.startInvulnerablePulse -= OnInvulnerablePulse;
            On.WaveEffectControl.OnEnable -= OnWaveEffectStart;
            On.BossStatueDreamToggle.Fade -= OnDreamToggleFade;
            On.DreamPlantOrb.Start -= OnDreamPlantOrbStart;
            On.BossStatueTrophyPlaque.DoTierCompleteEffect -= OnBossTrophyTierCompleteEffect;
            On.BossStatueFlashEffect.FlashRoutine -= OnBossStatueFlashRoutine;
            
            On.SpriteFlash.flashArmoured -= FlashHandler.OnFlashArmoured;
            On.SpriteFlash.flashBenchRest -= FlashHandler.OnFlashBench;
            On.SpriteFlash.flashDreamImpact -= FlashHandler.OnFlashDream;
            On.SpriteFlash.flashDungQuick -= FlashHandler.OnFlashDungQuick;
            On.SpriteFlash.flashFocusGet -= FlashHandler.OnFlashFocusGet;
            On.SpriteFlash.flashFocusHeal -= FlashHandler.OnFlashHeal;
            On.SpriteFlash.FlashGrimmflame -= FlashHandler.OnFlashGrimmFlame;
            On.SpriteFlash.FlashGrimmHit -= FlashHandler.OnFlashGrimmHit;
            On.SpriteFlash.flashHealBlue -= FlashHandler.OnFlashHealBlue;
            On.SpriteFlash.flashInfected -= FlashHandler.OnFlashInfected;
            On.SpriteFlash.FlashingSuperDash -= FlashHandler.OnFlashSuperDash;
            On.SpriteFlash.flashShadeGet -= FlashHandler.OnFlashShadeGet;
            On.SpriteFlash.flashSporeQuick -= FlashHandler.OnFlashSpore;
            On.SpriteFlash.flashWhitePulse -= FlashHandler.OnFlashWhitePulse;
        }
        
        //Targets the completion effect when returning to hall of gods
        private void OnBossTrophyTierCompleteEffect(On.BossStatueTrophyPlaque.orig_DoTierCompleteEffect orig, BossStatueTrophyPlaque self, BossStatueTrophyPlaque.DisplayType type)
        {
            if(!Gs.ToneDownGodhomeStatues) orig(self, type);
        }

        //Spawn anim
        private IEnumerator OnBossStatueFlashRoutine(On.BossStatueFlashEffect.orig_FlashRoutine orig, BossStatueFlashEffect self)
        {
            if(Gs.ToneDownGodhomeStatues) self.transform.Translate(new Vector3(2000f, 0f, 0f));//yeet the animation offscreen
            yield return orig(self);
        }
        
        //Dream toggle
        private IEnumerator OnDreamToggleFade(On.BossStatueDreamToggle.orig_Fade orig, BossStatueDreamToggle self, bool usingDreamVersion)
        {
            if (Gs.ToneDownGodhomeStatues) self.dreamBurstSpawnPoint.position = new Vector3(-200f, -200f, 0f);//puts our problems far away
            yield return orig(self, usingDreamVersion);
        }
        
        private void OnDreamPlantOrbStart(On.DreamPlantOrb.orig_Start orig, DreamPlantOrb self)
        {
            if(Gs.ToneDownDreamOrbs)
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                GameObject pickupAnim = self.gameObject.Child("PickupAnim");
                pickupAnim.GetComponent<SpriteRenderer>().enabled = false;
                pickupAnim.GetComponent<BasicSpriteAnimator>().enabled = false;
            }
            orig(self);
        }

        private void OnWaveEffectStart(On.WaveEffectControl.orig_OnEnable orig, WaveEffectControl self)
        {
            if (Gs.RemoveGenericFlashingEffects) self.spriteRenderer.enabled = false;
            orig(self);
        }

        //flickering when taking damage
        private void OnInvulnerablePulse(On.InvulnerablePulse.orig_startInvulnerablePulse orig, InvulnerablePulse self)
        {
            if (Gs.RemoveDamageFlickering) self.pulseDuration = 999;//effectively removes the pulse
            orig(self);
        }

        private GameObject OnObjectSpawn(GameObject arg)
        {
            if (arg.name.Contains("Flash") || arg.name.Contains("flash") || arg.name.Contains("White Wave") || arg.name.Contains("Dream Impact"))
            {
                if (!Gs.RemoveGenericFlashingEffects) return arg;
                arg.TryGetComponent(out SpriteRenderer sRenderer);
                arg.TryGetComponent(out MeshRenderer mRenderer);
                if (mRenderer) mRenderer.enabled = false;
                if (sRenderer) sRenderer.enabled = false;
            }

            if (arg.name.Contains("Gas Explosion"))//jelly/egg explosion
            {
                if (!Gs.ToneDownExplosions) return arg;
                arg.Child("orange flash").GetComponent<SpriteRenderer>().enabled = false;
                arg.TryGetComponent(out PlayMakerFSM fsm);
                //if(fsm) fsm.enabled = false;
            }

            if (arg.name == "Grimm_flare_pillar(Clone)")
            {
                arg.Child("Pillar").Child("haze2").GetComponent<SpriteRenderer>().enabled = false;
            }
            
            //Log(arg.name);
            return arg;
        }

        private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            //Log(self.name);
            if (self.name.Contains("Tele Out Corpse R(Clone)") && Gs.ToneDownMageLordFight)
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
                RemoveMageLordFlashes();
            }
            
            else if (self.name.Contains("Corpse Dream Mage Lord 1(Clone)") && Gs.ToneDownMageLordFight)
            {
                GameObject secondCorpse = GameObject.Find("Corpse Dream Mage Lord 1(Clone)");
                secondCorpse.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                secondCorpse.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                secondCorpse.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("End Flash 2"))
            {
                if(Gs.ToneDownMageLordFight)
                {
                    GameObject endFlashes = GameObject.Find("Corpse Mage Lord 1(Clone)").Child("End Flash");
                    endFlashes.Child("End Flash 1").GetComponent<SpriteRenderer>().enabled = false;
                    endFlashes.Child("End Flash 2").GetComponent<SpriteRenderer>().enabled = false;
                }
                
                if(Gs.RemoveSpellPickupsFlashes)
                {
                    GameObject fakeQuakeParent = GameObject.Find("Quake Fake Parent");
                    if (fakeQuakeParent != null)
                    {
                        GameObject fakeQuake = fakeQuakeParent.Child("Knight Get Quake Fake");
                        fakeQuake.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                        fakeQuake.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                        fakeQuake.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }

            else if (self.name.Contains("Gas Explosion Uumuu") && Gs.ToneDownUumuuFight)
            {
                _dontDestroyOnLoadScene.FindGameObject("Gas Explosion Uumuu(Clone)").Child("orange flash")
                    .GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Tele Flash" && Gs.ToneDownRadianceFightsFlashes)
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "Radiant Orb(Clone)" && Gs.ToneDownRadianceFightsFlashes)
            {
                self.gameObject.Child("Impact").GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "white_solid" && Gs.ToneDownRadianceFightsFlashes)
            {
                //Log("white_solid");
                self.gameObject.RemoveComponent<SpriteRenderer>();
                GameObject bossControl = GameObject.Find("Boss Control");
                bossControl.Child("Light Solid").RemoveComponent<SpriteRenderer>();
            }

            else if (self.name == "Shade Hit Vignette" && Gs.ToneDownRadianceFightsFlashes)
            {
                self.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Fireball(Clone)" || self.name == "Fireball Top(Clone)" ||
                     self.name == "Fireball2 Top(Clone)" || self.name.Contains("Fireball2 Spiral(Clone)"))
            {
                if(Gs.RemoveSpellFlashes) self.gameObject.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Roar Wave Emitter Scream(Clone)" && Gs.RemoveBossScreams)
            {
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Q Slam" && Gs.RemoveSpellFlashes)
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "Death Explode Boss(Clone)" && Gs.ToneDownDeathExplosions)
            {
                self.gameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
                self.gameObject.Child("Splat Explode Orange").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("Orange Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Roar Wave Emitter(Clone)" && Gs.RemoveBossScreams)
            {
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 1").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 2").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("Ghost Death") && Gs.ToneDownWarriorDreamsFlashes)
            {
                if (self.name == "Ghost Death(Clone)")
                {
                    self.gameObject.Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                }

                if (!_ghostExploding)
                {
                    _ghostExploding = true;//I don't remember why I did that but sure
                    self.gameObject.Child("White Wave").SetActive(false);
                }

                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("Ghost Warrior") && Gs.ToneDownWarriorDreamsFlashes)
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                /*_dontDestroyOnLoadScene.FindGameObject("Dream Impact(Clone)").GetComponent<MeshRenderer>().enabled =
                    false;*/
            }

            else if (self.name == "Silhouette" && Gs.ToneDownMageLordFight)//part of soul master
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "White Flash" && Gs.RemoveGenericFlashingEffects)
            {
                self.gameObject.TryGetComponent(out SpriteRenderer sRenderer);
                if (sRenderer)
                {
                    sRenderer.enabled = false;
                }
            }
            
            else if (self.name == "Spawn Flash")
            {
                //Log("spawn flash fsm enabled");
                //Log(self.gameObject.name);
                self.enabled = false;
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (_ghostExploding) _ghostExploding = false;
            
            if (newScene.name == "Crossroads_ShamanTemple" && Gs.RemoveSpellPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveAncestralMoundFlashes());
            }
            
            else if (newScene.name == "Room_Fungus_Shaman" && Gs.RemoveSpellPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveOvergrownMoundFlashes());
            }
            
            else if (newScene.name == "Ruins1_24" && Gs.RemoveSpellPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveRealQuakeFlashes());
            }
            
            else if (newScene.name == "Ruins1_24_boss" && (Gs.RemoveSpellPickupsFlashes || Gs.ToneDownMageLordFight))
            {
                GameManager.instance.StartCoroutine(RemoveFakeQuakeFlashes());
            }
            
            else if (newScene.name == "Dream_Abyss" && Gs.ToneDownBirthPlaceFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveBirthPlaceCutsceneFlashes());
            }
            
            else if (newScene.name == "Abyss_15" && Gs.ToneDownBirthPlaceFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveBirthPlaceTriggerFlashes());
            }
            
            else if (newScene.name.Contains("GG_End_Sequence") && Gs.RemovePantheonCompletionFlashes)
            {
                SpriteRenderer[] orbFlashes =
                    GameObject.Find("Big Orb Flash").GetComponentsInChildren<SpriteRenderer>();
                foreach (var renderer in orbFlashes)
                {
                    renderer.enabled = false;
                }

                GameObject.Find("GG_Challenge_Door_Complete_Canvas").Child("Backboard").Child("Core")
                    .Child("BindingFlash").RemoveComponent<Image>();

                IEnumerable<GameObject> bindingFlashes = newScene.GetAllGameObjects()
                    .Where(o => o.name.Contains("GG_victory_orb_appear_extra"));
                foreach (var bindingFlash in bindingFlashes)
                {
                    bindingFlash.Child("white_glow").GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            else if (newScene.name == "Deepnest_East_Hornet" && Gs.ToneDownHornet2Fight)
            {
                IEnumerable<GameObject> blizzardParticles =
                    newScene.GetAllGameObjects().Where(o => o.name.Contains("blizzard_particles"));
                foreach (var blizzardParticle in blizzardParticles)
                {
                    blizzardParticle.SetActive(false);
                }
            }

            else if (newScene.name == "Dream_Final_boss" && Gs.ToneDownRadianceFightsFlashes)
            {
                GameObject.Find("Boss Control").Child("Radiance Roar").RemoveComponent<SpriteFlash>();
            }
            
            else if (newScene.name == "Cliffs_06" && Gs.ToneDownGrimmLanternActivation)//grimm lantern
            {
                GameManager.instance.StartCoroutine(RemoveGrimmLanternFlashes());
            }
            
            else if (newScene.name == "Fungus3_archive_02" && Gs.RemoveQuirrelArchivesCutsceneFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveQuirrelArchivesCutsceneFlashes());
            }
        }

        private bool OnEnableEnemy(GameObject enemy, bool isalreadydead)
        {
            //Log(enemy.name);
            if (enemy.name.Contains("Absolute Radiance") && Gs.ToneDownRadianceFightsFlashes)
            {
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Slash Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Tele Flash").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Break Effect").Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                GameObject bossControl = GameObject.Find("Boss Control");
                bossControl.Child("radiance_slashes").Child("white_fader").GetComponent<SpriteRenderer>().enabled =
                    false;
                bossControl.Child("Final Explode").Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                bossControl.Child("Final Explode").Child("Dream Impact (1)").GetComponent<MeshRenderer>().enabled =
                    false;
                IEnumerable<GameObject> orbs = _dontDestroyOnLoadScene.GetAllGameObjects()
                    .Where(o => o.name.Contains("Radiant Orb"));
                foreach (var orb in orbs)
                {
                    orb.Child("Impact").GetComponent<MeshRenderer>().enabled = false;
                }
            }

            else if (enemy.name == "Mage Lord" && Gs.ToneDownMageLordFight)
            {
                enemy.Child("Appear Flash").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Fire Effect").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Quake Pillar").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Quake Blast").GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (enemy.name == "Dream Mage Lord" && Gs.ToneDownMageLordFight)
            {
                GameObject appearFlash = enemy.Child("Appear Flash");
                GameObject whiteFlash = enemy.Child("White Flash");
                GameObject fireEffect = enemy.Child("Fire Effect");
                GameObject quakePillar = enemy.Child("Quake Pillar");
                GameObject quakeBlast = enemy.Child("Quake Blast");
                
                appearFlash.GetComponent<MeshRenderer>().enabled = false;
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                fireEffect.GetComponent<MeshRenderer>().enabled = false;
                quakePillar.GetComponent<MeshRenderer>().enabled = false;
                quakeBlast.GetComponent<MeshRenderer>().enabled = false;
                
                appearFlash.GetComponent<PlayMakerFSM>().enabled = false;//for some reason, there's additional fsms compared to SM
                whiteFlash.GetComponent<PlayMakerFSM>().enabled = false;
                fireEffect.GetComponent<PlayMakerFSM>().enabled = false;
                quakePillar.GetComponent<PlayMakerFSM>().enabled = false;
                quakeBlast.GetComponent<PlayMakerFSM>().enabled = false;
            }

            else if (enemy.name.Contains("Mega Jellyfish") && Gs.ToneDownUumuuFight)
            {
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (enemy.name == "Radiance" && Gs.ToneDownRadianceFightsFlashes)
            {
                enemy.Child("Slash Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Final Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Death Eye Glow").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Break Effect").Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Death").Child("Knight Split").GetComponent<MeshRenderer>().enabled = false;
            }

            else if (enemy.name == "Mage Lord Phase2" && Gs.ToneDownMageLordFight)
            {
                enemy.Child("Appear Flash").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Quake Pillar").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Quake Blast").GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (enemy.name == "Dream Mage Lord Phase2" && Gs.ToneDownMageLordFight)
            {
                GameObject appearFlash = enemy.Child("Appear Flash");
                GameObject quakePillar = enemy.Child("Quake Pillar");
                GameObject quakeBlast = enemy.Child("Quake Blast");
                
                appearFlash.GetComponent<MeshRenderer>().enabled = false;
                quakePillar.GetComponent<MeshRenderer>().enabled = false;
                quakeBlast.GetComponent<MeshRenderer>().enabled = false;
                
                appearFlash.GetComponent<PlayMakerFSM>().enabled = false;
                quakePillar.GetComponent<PlayMakerFSM>().enabled = false;
                quakeBlast.GetComponent<PlayMakerFSM>().enabled = false;
            }
            
            else if (enemy.name.Contains("Flamebearer") && Gs.ToneDownGrimmKinFights)//grimmkin 
            {
                GameObject redFlash = enemy.Child("Red Flash 1");
                redFlash.GetComponent<SimpleSpriteFade>().enabled = false;
                redFlash.GetComponent<SpriteRenderer>().enabled = false;
                //Log("removing grimmkin warp flash");
            }
            
            else if (enemy.name.Contains("Mage Knight") && Gs.ToneDownMageLordFight)//soul warrior
            {
                GameObject fireEffect = enemy.Child("Fire Effect");
                fireEffect.GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (enemy.name.Contains("Mage") && !enemy.name.Contains("Blob") && !enemy.name.Contains("Balloon") && Gs.ToneDownMageLordFight) //Soul twisters but not follies nor mistakes and hopefully nothing else that I forgot about
            {
                //Log(enemy.name);
                GameObject fireEffect = enemy.Child("Fire Effect");
                fireEffect.GetComponent<MeshRenderer>().enabled = false;
                
                GameObject flashSprite = enemy.Child("Flash Sprite");
                flashSprite.GetComponent<MeshRenderer>().enabled = false;
                
                GameObject appearFlash = enemy.Child("Appear Flash");
                appearFlash.GetComponent<MeshRenderer>().enabled = false;
                
                GameObject whiteFlash = enemy.Child("White Flash");
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
            }
            
            else if (enemy.name.Contains("Ceiling Dropper"))
            {
                GameObject explosion = enemy.Child("Gas Explosion M2(Clone)");
                if(explosion) explosion.Child("orange flash").SetActive(false);
                //Log("Ceiling dropper flash removed");
            }
            
            else if (enemy.name.Contains("Grimm Boss"))
            {
                GameObject redFlash1 = enemy.Child("Red Flash 1");
                GameObject redFlash2 = enemy.Child("Red Flash 2");
                
                redFlash1.GetComponent<SpriteRenderer>().enabled = false;
                redFlash2.GetComponent<SpriteRenderer>().enabled = false;
                
                GameObject explodeEffects = enemy.Child("Explode Effects");
                GameObject whiteFlash = explodeEffects.Child("White Flash");
                GameObject burst1 = explodeEffects.Child("Burst (1)");
                
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                burst1.GetComponent<MeshRenderer>().enabled = false;
                
                GameObject grimmControl = GameObject.Find("Grimm Control");
                if (grimmControl)
                {
                    GameObject finalIntroFlash = grimmControl.Child("Final Intro Flash");
                    if(finalIntroFlash) finalIntroFlash.GetComponent<MeshRenderer>().enabled = false;
                    
                    GameObject greatEyeOpen2 = GameObject.Find("/Grimm Control/Appear Scene/Great Eye Open 2");
                    if (greatEyeOpen2)
                    {
                        GameObject sdSharpFlash = greatEyeOpen2.Child("SD Sharp Flash (1)");
                        GameObject eyeWhiteFlash = greatEyeOpen2.Child("White Flash");
                        
                        if(sdSharpFlash) sdSharpFlash.GetComponent<MeshRenderer>().enabled = false;
                        if(eyeWhiteFlash) eyeWhiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                
                GameObject teleInEffects = GameObject.Find("Tele In Effects");
                if (teleInEffects)
                {
                    teleInEffects.Child("Flame Pillar").GetComponent<MeshRenderer>().enabled = false;
                }
                
                GameObject grimmNightmareFabricLantern = GameObject.Find("Grimm_nightmare_fabric_lantern");
                if (grimmNightmareFabricLantern)
                {
                    grimmNightmareFabricLantern.Child("grimm_fader").GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            
            else if (enemy.name.Contains("Real Bat"))
            {
                enemy.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
            }
            
            return isalreadydead;
        }

        private void OnHeroAwake(On.HeroController.orig_Awake orig, HeroController self)
        {
            orig(self);
            _emptyGo = new GameObject();
            Object.DontDestroyOnLoad(_emptyGo);
            _dontDestroyOnLoadScene = _emptyGo.scene;
            RemoveHeroFlashes();
        }

        private void RemoveHeroFlashes()
        {
            GameObject knight = GameObject.Find("Knight");
            if(Gs.RemoveGenericHeroFlashes) knight.Child("white_light_donut").GetComponent<SpriteRenderer>().enabled = false; 

            if(Gs.RemoveSpellFlashes)
            {
                GameObject spells = knight.Child("Spells");
                spells.Child("Q Flash Slam").GetComponent<SpriteRenderer>().enabled = false;
                spells.Child("Q Flash Start").GetComponent<SpriteRenderer>().enabled = false;
                spells.Child("Q Slam 2").GetComponent<MeshRenderer>().enabled = false;
            }

            if(Gs.RemoveHealFlashes)
            {
                GameObject focusEffects = knight.Child("Focus Effects");
                focusEffects.Child("Heal Anim").GetComponent<MeshRenderer>().enabled = false;
            }

            GameObject effects = knight.Child("Effects");
            List<GameObject> flashesInKnight = new List<GameObject>();
            foreach (Transform flashEffect in effects.GetComponentsInChildren<Transform>(true))
            {
                if (flashEffect.gameObject.name.Contains("White Flash") ||
                    flashEffect.gameObject.name.Contains("SD Burst Glow") ||
                    flashEffect.gameObject.name.Contains("SD Sharp Flash") ||
                    flashEffect.gameObject.name.Contains("Soul Burst"))
                {
                    flashesInKnight.Add(flashEffect.gameObject);
                }
            }

            foreach (var knightFlash in flashesInKnight)
            {
                if (knightFlash.name.Contains("SD Sharp Flash") && Gs.RemoveCrystalDashFlashes)
                {
                    knightFlash.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (Gs.RemoveGenericHeroFlashes)
                {
                    //knightFlash.GetComponent<SpriteRenderer>().enabled = false;
                    knightFlash.TryGetComponent(out SpriteRenderer sRenderer);
                    knightFlash.TryGetComponent(out MeshRenderer mRenderer);
                    if (mRenderer) mRenderer.enabled = false;
                    if (sRenderer) sRenderer.enabled = false;
                    // ReSharper disable InconsistentNaming
                    bool hasFSM = knightFlash.TryGetComponent<PlayMakerFSM>(out var flashFSM);
                    // ReSharper restore InconsistentNaming
                    if(hasFSM) flashFSM.enabled = false;
                }
            }

            GameObject pool = GameObject.Find("_GameManager").Child("GlobalPool");
            List<GameObject> flashesInPool = new List<GameObject>();
            foreach (Transform flashEffect in pool.GetComponentsInChildren<Transform>(true))
            {
                if (flashEffect.gameObject.name.Contains("White Flash"))
                {
                    flashesInPool.Add(flashEffect.gameObject);
                }
            }

            if (Gs.RemoveGenericHeroFlashes)
            {
                foreach (var poolFlash in flashesInPool)
                {
                    poolFlash.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            if (Gs.RemoveDamageFlickering)
            {
                knight.GetComponent<SpriteFlash>().enabled = false;
            }

            if(Gs.RemoveCrystalDashFlashes)
            {
                // ReSharper disable once InconsistentNaming
                GameObject SDBurst = _dontDestroyOnLoadScene.FindGameObject("Knight/Effects/SD Burst");
                SDBurst.GetComponent<MeshRenderer>().enabled = false;
                SDBurst.GetComponent<PlayMakerFSM>().enabled = false;

                // ReSharper disable once InconsistentNaming
                GameObject SDBling = effects.Child("SD Bling");
                SDBling.GetComponent<MeshRenderer>().enabled = false;
            }
            
            if(Gs.RemoveSoulOrbFlashes)
            {
                GameObject soulOrb =
                    _dontDestroyOnLoadScene.FindGameObject("_GameCameras/HudCamera/Hud Canvas/Soul Orb/White Flash");
                soulOrb.GetComponent<SpriteRenderer>().enabled = false;
                soulOrb.GetComponent<PlayMakerFSM>().enabled = false;
            }
        }

        private void RemoveMageLordFlashes()
        {
            if (!Gs.ToneDownMageLordFight) return;
            IEnumerable<GameObject> whiteFlashes = _dontDestroyOnLoadScene.GetAllGameObjects()
                .Where(o => o.name.Contains("White Flash R"));
            IEnumerable<GameObject> appearFlashes = _dontDestroyOnLoadScene.GetAllGameObjects()
                .Where(o => o.name.Contains("Appear Flash R"));
            foreach (var whiteFlash in whiteFlashes)
            {
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
            }

            foreach (var appearFlash in appearFlashes)
            {
                appearFlash.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private IEnumerator RemoveAncestralMoundFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject props = GameObject.Find("_Props");
            GameObject knightGetFireball = props.Child("Knight Get Fireball");
            
            if (knightGetFireball) 
            {
                knightGetFireball.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                knightGetFireball.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
            }
                
            GameObject shamanMeeting = props.Child("Shaman Meeting");
            
            if(shamanMeeting)
            {
                GameObject summonFx = shamanMeeting.Child("VS Summon Fx");
                
                if (summonFx)
                {
                    summonFx.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                    summonFx.Child("Get Glow").GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        private IEnumerator RemoveOvergrownMoundFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject knightGetScream = GameObject.Find("Scream Control").Child("Knight Get Scream");
            
            if (knightGetScream)
            {
                knightGetScream.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                knightGetScream.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        private IEnumerator RemoveFakeQuakeFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject quakeFakeParent = GameObject.Find("Quake Fake Parent");

            if (quakeFakeParent)
            {
                GameObject knightGetQuakeFake = quakeFakeParent.Child("Knight Get Quake Fake");

                if (knightGetQuakeFake)
                {
                    knightGetQuakeFake.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                    knightGetQuakeFake.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                    knightGetQuakeFake.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        private IEnumerator RemoveRealQuakeFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject quakeRealParent = GameObject.Find("Quake Real Parent");

            if (quakeRealParent)
            {
                GameObject knightGetQuake = quakeRealParent.Child("Knight Get Quake");

                if (knightGetQuake)
                {
                    knightGetQuake.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                    knightGetQuake.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                    knightGetQuake.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
                }
                
                GameObject quakePickup = quakeRealParent.Child("Quake Pickup");

                if (quakePickup)
                {
                    quakePickup.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                    
                    GameObject quakeItem = quakePickup.Child("Quake Item");

                    if (quakeItem)
                    {
                        quakeItem.Child("White Wave Default").GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }

        private IEnumerator RemoveBirthPlaceCutsceneFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject.Find("cd_room_beam_glow").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("haze2 (1)").GetComponent<SpriteRenderer>().enabled = false;
            
            GameObject flasher = GameObject.Find("End Cutscene").Child("Flasher");
            
            flasher.GetComponent<SpriteRenderer>().enabled = false;
            flasher.GetComponent<PlayMakerFSM>().enabled = false;
        }

        private IEnumerator RemoveBirthPlaceTriggerFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject dreamEnterAbyss = GameObject.Find("Dream Enter Abyss");

            if (dreamEnterAbyss)
            {
                GameObject whiteFlash = dreamEnterAbyss.Child("White Flash");
                GameObject impact = dreamEnterAbyss.Child("Impact");
                impact.GetComponent<MeshRenderer>().enabled = false;
                impact.GetComponent<PlayMakerFSM>().enabled = false;
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                whiteFlash.GetComponent<PlayMakerFSM>().enabled = false;
            }
        }

        private IEnumerator RemoveQuirrelArchivesCutsceneFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject quirrelFlash = GameObject.Find("/Dreamer Monomon/Quirrel/White Flash");
            GameObject arriveFlash0 = GameObject.Find("/Dreamer Monomon/Arrive Flash/White Flash");
            GameObject arriveFlash1 = GameObject.Find("/Dreamer Monomon/Arrive Flash/White Flash (1)");

            if (quirrelFlash)
            {
                quirrelFlash.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (arriveFlash0)
            {
                arriveFlash0.GetComponent<SpriteRenderer>().enabled = false;
            }
            
            if (arriveFlash1)
            {
                arriveFlash1.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        private IEnumerator RemoveGrimmLanternFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject sycophantHitFlash = GameObject.Find("Sycophant Dream").Child("Hit Flash");
            if (sycophantHitFlash)
            {
                sycophantHitFlash.GetComponent<PlayMakerFSM>().enabled = false;
                sycophantHitFlash.GetComponent<SpriteRenderer>().enabled = false;
            }
            
            GameObject grimmBrazier = GameObject.Find("/Nightmare Lantern/lantern_dream/big_lantern/grimm_brazier");
            if (grimmBrazier)
            {
                GameObject lightFlash = grimmBrazier.Child("Light Flash");
                GameObject sharpFlash = grimmBrazier.Child("Sharp Flash");

                if (lightFlash)
                {
                    lightFlash.GetComponent<SpriteRenderer>().enabled = false;
                    lightFlash.GetComponent<PlayMakerFSM>().enabled = false;
                }

                if (sharpFlash)
                {
                    sharpFlash.GetComponent<MeshRenderer>().enabled = false;
                    sharpFlash.GetComponent<PlayMakerFSM>().enabled = false;
                }
            }
        }

        public void OnLoadGlobal(GlobalSettings s) => Gs = s;

        public GlobalSettings OnSaveGlobal() => Gs;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return ModMenu.CreateModMenu(modListMenu, toggleDelegates);
        }
    }
}