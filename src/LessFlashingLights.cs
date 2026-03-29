using System.Collections;
using Modding;
using Modding.Utils;
using Satchel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LessFlashingLights
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class LessFlashingLights : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
    {
        public new string GetName() => "Less Flashing Lights";
        public override string GetVersion() => "1.0.1.9";
        
        public static GlobalSettings Gs { get; private set; } = new();
        
        public bool ToggleButtonInsideMenu => true;
        
        private Scene _dontDestroyOnLoadScene;
        private GameObject? _emptyGo;
        private bool _ghostExploding;//I do not remember what was the logic behind this, but I'm too scared to remove it
        private bool _inGrimmFight;//For disabling an FSM used in other places which breaks stuff when disabled outside the fight
        private bool _inShadeSoulPickup;//Again, to only disable an FSM if we know what we're doing
        private bool _inDreamerCutscene;

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
            On.BossDoorChallengeUI.Show += OnBossDoorChallengeUIShow;
            On.BossDoorChallengeUIBindingButton.SetAllSelected += OnAllBindingsSelected;
            
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
            On.BossDoorChallengeUI.Show -= OnBossDoorChallengeUIShow;
            On.BossDoorChallengeUIBindingButton.SetAllSelected -= OnAllBindingsSelected;
            
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
        
        private void OnAllBindingsSelected(On.BossDoorChallengeUIBindingButton.orig_SetAllSelected orig, BossDoorChallengeUIBindingButton self, bool value)
        {
            orig(self, value);

            if (!value || !Gs.RemoveGodhomeFlashes) return;
            
            self.gameObject.Child("AllFlashEffect").SetActive(false);
        }

        private void OnBossDoorChallengeUIShow(On.BossDoorChallengeUI.orig_Show orig, BossDoorChallengeUI self)
        {
            orig(self);

            if (!Gs.RemoveGodhomeFlashes) return;

            GameObject bossDoorSelectAllFlash = self.gameObject.Child("Panel").Child("Select All Flash");
            if (!bossDoorSelectAllFlash) return;
            
            GameObject deathGlow = bossDoorSelectAllFlash.Child("Death Glow");
                
            deathGlow.RemoveComponent<MeshRenderer>();
            deathGlow.RemoveComponent<tk2dSprite>();
            deathGlow.RemoveComponent<tk2dSpriteAnimator>();
            deathGlow.RemoveComponent<DeactivateAfter2dtkAnimation>();

            GameObject whiteFlashR = bossDoorSelectAllFlash.Child("White Flash R");
            
            whiteFlashR.RemoveComponent<SimpleSpriteFade>();
            whiteFlashR.RemoveComponent<SpriteRenderer>();
        }
        
        //Targets the completion effect when returning to hall of gods
        private void OnBossTrophyTierCompleteEffect(On.BossStatueTrophyPlaque.orig_DoTierCompleteEffect orig, BossStatueTrophyPlaque self, BossStatueTrophyPlaque.DisplayType type)
        {
            if(!Gs.RemoveGodhomeFlashes) orig(self, type);
        }

        //Spawn anim
        private IEnumerator OnBossStatueFlashRoutine(On.BossStatueFlashEffect.orig_FlashRoutine orig, BossStatueFlashEffect self)
        {
            if(Gs.RemoveGodhomeFlashes) self.transform.Translate(new Vector3(2000f, 0f, 0f));//yeet the animation offscreen
            yield return orig(self);
        }
        
        //Dream toggle
        private IEnumerator OnDreamToggleFade(On.BossStatueDreamToggle.orig_Fade orig, BossStatueDreamToggle self, bool usingDreamVersion)
        {
            if (Gs.RemoveGodhomeFlashes) self.dreamBurstSpawnPoint.position = new Vector3(-200f, -200f, 0f);//puts our problems far away
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
            if (Gs.RemoveGenericFlashingEffects)
            {
                if(self.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) spriteRenderer.enabled = false;
            }
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
                //arg.TryGetComponent(out PlayMakerFSM fsm);
                //if(fsm) fsm.enabled = false;
            }

            if (arg.name == "Grimm_flare_pillar(Clone)" && Gs.ToneDownGrimmKinFights)
            {
                arg.Child("Pillar").Child("haze2").GetComponent<SpriteRenderer>().enabled = false;
            }
            
            //Log(arg.name);
            return arg;
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

            GameObject heroDeath = knight.Child("Hero Death");
            if (heroDeath && Gs.ToneDownHeroDeath)
            {
                GameObject dreamImpact = heroDeath.Child("Dream Impact");
                dreamImpact.RemoveComponent<MeshRenderer>();
                dreamImpact.RemoveComponent<tk2dSprite>();
                dreamImpact.RemoveComponent<tk2dSpriteAnimator>();
                dreamImpact.RemoveComponent<DeactivateAfter2dtkAnimation>();
                
                heroDeath.Child("Death Crack").GetComponent<SpriteRenderer>().enabled = false;
                heroDeath.Child("Hit Crack").GetComponent<SpriteRenderer>().enabled = false;
            }
            
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

                GameObject hitCrack = GameObject.Find("Knight/Effects/Damage Effect/Hit Crack");
                hitCrack.GetComponent<SpriteRenderer>().enabled = false;
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
                GameObject soulOrbWhiteFlash =
                    _dontDestroyOnLoadScene.FindGameObject("_GameCameras/HudCamera/Hud Canvas/Soul Orb/White Flash");
                GameObject soulOrbBurstAnim =
                    _dontDestroyOnLoadScene.FindGameObject("_GameCameras/HudCamera/Hud Canvas/Soul Orb/Burst Anim");
                
                soulOrbWhiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                soulOrbWhiteFlash.GetComponent<PlayMakerFSM>().enabled = false;
                soulOrbBurstAnim.GetComponent<MeshRenderer>().enabled = false;
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