using System.Collections;
using Modding;
using Modding.Utils;
using Satchel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NoFlashingLights
{
    public class NoFlashingLights : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
    {
        public new string GetName() => "No Flashing Lights";
        public override string GetVersion() => "0.10.3";
        
        public static GlobalSettings Gs { get; private set; } = new();
        
        public bool ToggleButtonInsideMenu => true;
        
        private Scene _dontDestroyOnLoadScene;
        private GameObject? _emptyGo;
        private bool _ghostExploding;

        public override void Initialize()
        {
            On.HeroController.Awake += OnHeroAwake;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            ModHooks.OnEnableEnemyHook += OnEnableEnemy;
            ModHooks.ObjectPoolSpawnHook += OnObjectSpawn;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
            On.InvulnerablePulse.startInvulnerablePulse += OnInvulnerablePulse;
            On.WaveEffectControl.OnEnable += OnWaveEffectStart;
            On.BossStatueDreamToggle.Fade += OnDreamToggleFade;
            On.DreamPlantOrb.Start += OnDreamPlantOrbStart;
            
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
            On.SpriteFlash.FlashingGhostWounded += FlashHandler.OnFlashingGhostWounded;
            On.SpriteFlash.FlashingSuperDash += FlashHandler.OnFlashSuperDash;
            On.SpriteFlash.flashShadeGet += FlashHandler.OnFlashShadeGet;
            On.SpriteFlash.flashSporeQuick += FlashHandler.OnFlashSpore;
            On.SpriteFlash.flashWhitePulse += FlashHandler.OnFlashWhitePulse;

            On.EnemyHitEffectsGhost.RecieveHitEffect += EnemyHitEffectHandler.OnReceiveGhostHitEffect;
            On.EnemyHitEffectsBlackKnight.RecieveHitEffect += EnemyHitEffectHandler.OnReceiveBlackKnightHitEffect;
            On.EnemyHitEffectsArmoured.RecieveHitEffect += EnemyHitEffectHandler.OnReceiveArmouredHitEffect;
            On.EnemyHitEffectsShade.RecieveHitEffect += EnemyHitEffectHandler.OnReceiveShadeHitEffect;
            On.EnemyHitEffectsUninfected.RecieveHitEffect += EnemyHitEffectHandler.OnReceiveUninfectedHitEffect;
        }

        public void Unload()
        {
            On.HeroController.Awake -= OnHeroAwake;
            On.PlayMakerFSM.OnEnable -= OnFsmEnable;
            ModHooks.OnEnableEnemyHook -= OnEnableEnemy;
            ModHooks.ObjectPoolSpawnHook -= OnObjectSpawn;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChange;
            On.InvulnerablePulse.startInvulnerablePulse -= OnInvulnerablePulse;
            On.WaveEffectControl.OnEnable -= OnWaveEffectStart;
            On.BossStatueDreamToggle.Fade -= OnDreamToggleFade;
            On.DreamPlantOrb.Start -= OnDreamPlantOrbStart;
            
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
            On.SpriteFlash.FlashingGhostWounded -= FlashHandler.OnFlashingGhostWounded;
            On.SpriteFlash.FlashingSuperDash -= FlashHandler.OnFlashSuperDash;
            On.SpriteFlash.flashShadeGet -= FlashHandler.OnFlashShadeGet;
            On.SpriteFlash.flashSporeQuick -= FlashHandler.OnFlashSpore;
            On.SpriteFlash.flashWhitePulse -= FlashHandler.OnFlashWhitePulse;

            On.EnemyHitEffectsGhost.RecieveHitEffect -= EnemyHitEffectHandler.OnReceiveGhostHitEffect;
            On.EnemyHitEffectsBlackKnight.RecieveHitEffect -= EnemyHitEffectHandler.OnReceiveBlackKnightHitEffect;
            On.EnemyHitEffectsArmoured.RecieveHitEffect -= EnemyHitEffectHandler.OnReceiveArmouredHitEffect;
            On.EnemyHitEffectsShade.RecieveHitEffect -= EnemyHitEffectHandler.OnReceiveShadeHitEffect;
            On.EnemyHitEffectsUninfected.RecieveHitEffect -= EnemyHitEffectHandler.OnReceiveUninfectedHitEffect;
        }
        
        private void OnDreamPlantOrbStart(On.DreamPlantOrb.orig_Start orig, DreamPlantOrb self)
        {
            self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            GameObject pickupAnim = self.gameObject.Child("PickupAnim");
            pickupAnim.GetComponent<SpriteRenderer>().enabled = false;
            pickupAnim.GetComponent<BasicSpriteAnimator>().enabled = false;
            orig(self);
        }

        private IEnumerator OnDreamToggleFade(On.BossStatueDreamToggle.orig_Fade orig, BossStatueDreamToggle self, bool usingDreamVersion)
        {
            if (Gs.ToneDownGodhomeDreamStatues) self.dreamBurstSpawnPoint.position = new Vector3(-200f, -200f, 0f);//puts our problems far away
            yield return orig(self, usingDreamVersion);
        }

        private void OnWaveEffectStart(On.WaveEffectControl.orig_OnEnable orig, WaveEffectControl self)
        {
            if (Gs.RemoveGenericFlashingEffects) self.spriteRenderer.enabled = false;
            orig(self);
        }

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

            if (arg.name.Contains("Gas Explosion Recycle"))
            {
                if (!Gs.ToneDownJellyfishExplosions) return arg;
                arg.Child("orange flash").GetComponent<SpriteRenderer>().enabled = false;
            }

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
                Log("white_solid");
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
                    _ghostExploding = true;//I don't fcking remember why I did that but sure
                    self.gameObject.Child("White Wave").SetActive(false);
                }

                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("Ghost Warrior") && Gs.ToneDownWarriorDreamsFlashes)
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                _dontDestroyOnLoadScene.FindGameObject("Dream Impact(Clone)").GetComponent<MeshRenderer>().enabled =
                    false;
            }

            else if (self.name == "Silhouette")
            {
                //TODO identify
                Log("Silhouette");
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
            
            else if (newScene.name == "Cliffs_06")//grimm lantern
            {
                GameManager.instance.StartCoroutine(RemoveGrimmLanternFlashes());
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

            else if (enemy.name == "Mage Lord" && Gs.ToneDownRadianceFightsFlashes)
            {
                enemy.Child("Appear Flash").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Fire Effect").GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (enemy.name == "Dream Mage Lord" && Gs.ToneDownMageLordFight)
            {
                GameObject appearFlash = enemy.Child("Appear Flash");
                GameObject whiteFlash = enemy.Child("White Flash");
                GameObject fireEffect = enemy.Child("Fire Effect");
                
                appearFlash.GetComponent<MeshRenderer>().enabled = false;//for some reason there's additional fsms compared to SM
                whiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                fireEffect.GetComponent<MeshRenderer>().enabled = false;
                
                appearFlash.GetComponent<PlayMakerFSM>().enabled = false;
                whiteFlash.GetComponent<PlayMakerFSM>().enabled = false;
                fireEffect.GetComponent<PlayMakerFSM>().enabled = false;
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
            
            else if (enemy.name.Contains("Flamebearer"))//grimmkin 
            {
                GameObject redFlash = enemy.gameObject.Child("Red Flash 1");
                redFlash.GetComponent<SimpleSpriteFade>().enabled = false;
                redFlash.GetComponent<SpriteRenderer>().enabled = false;
                Log("removing grimmkin warp flash");
            }
            
            else if (enemy.name.Contains("Mage Knight"))
            {
                GameObject fireEffect = enemy.gameObject.Child("Fire Effect");
                fireEffect.GetComponent<MeshRenderer>().enabled = false;
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
                    flashEffect.gameObject.name.Contains("SD Sharp Flash"))
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
                    knightFlash.GetComponent<SpriteRenderer>().enabled = false;
                    bool hasFSM = knightFlash.TryGetComponent<PlayMakerFSM>(out var flashFSM);
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

            GameObject SDBurst = _dontDestroyOnLoadScene.FindGameObject("SD Burst");
            SDBurst.GetComponent<MeshRenderer>().enabled = false;
            SDBurst.GetComponent<PlayMakerFSM>().enabled = false;
            
            GameObject soulOrb = _dontDestroyOnLoadScene.FindGameObject("_GameCameras/HudCamera/Hud Canvas/Soul Orb");
            soulOrb.GetComponent<SpriteRenderer>().enabled = false;
            soulOrb.GetComponent<PlayMakerFSM>().enabled = false;
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

        private IEnumerator RemoveGrimmLanternFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject sycophantHitFlash = GameObject.Find("Sycophant Dream").Child("Hit Flash");
            if (sycophantHitFlash)
            {
                sycophantHitFlash.GetComponent<PlayMakerFSM>().enabled = false;
                sycophantHitFlash.GetComponent<SpriteRenderer>().enabled = false;
                Log("removing lantern flashes 1");
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
                    Log("removing lantern flashes 2");
                }

                if (sharpFlash)
                {
                    sharpFlash.GetComponent<MeshRenderer>().enabled = false;
                    sharpFlash.GetComponent<PlayMakerFSM>().enabled = false;
                    Log("removing lantern flashes 3");
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