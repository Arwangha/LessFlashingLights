using Modding;
using Modding.Utils;
using Satchel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vasi;
using Object = UnityEngine.Object;

namespace NoFlashingLights
{
    public class NoFlashingLights : Mod
    {
        public new string GetName() => "No Flashing Lights";
        public override string GetVersion() => "0.6.3";
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
        }

        private GameObject OnObjectSpawn(GameObject arg)//general effects removal
        {
            if (arg.name.Contains("Flash") || arg.name.Contains("flash") || arg.name.Contains("White Wave"))
            {
                arg.TryGetComponent(out SpriteRenderer sRenderer);
                arg.TryGetComponent(out MeshRenderer mRenderer);
                if (mRenderer != null) mRenderer.enabled = false;
                if (sRenderer != null) sRenderer.enabled = false;
            }

            return arg;
        }

        private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            //Log(self.name);
            if (self.name.Contains("Tele Out Corpse R(Clone)"))
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
                RemoveMageLordFlashes();
            }

            else if (self.name.Contains("End Flash 2"))
            {
                GameObject endFlashes = GameObject.Find("Corpse Mage Lord 1(Clone)").Child("End Flash");
                endFlashes.Child("End Flash 1").GetComponent<SpriteRenderer>().enabled = false;
                endFlashes.Child("End Flash 2").GetComponent<SpriteRenderer>().enabled = false;
                GameObject fakeQuakeParent = GameObject.Find("Quake Fake Parent");
                if (fakeQuakeParent != null)
                {
                    GameObject fakeQuake = fakeQuakeParent.Child("Knight Get Quake Fake");
                    fakeQuake.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                    fakeQuake.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
                    fakeQuake.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            else if (self.name.Contains("Gas Explosion Uumuu"))
            {
                _dontDestroyOnLoadScene.FindGameObject("Gas Explosion Uumuu(Clone)").Child("orange flash")
                    .GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Tele Flash")
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "Radiant Orb(Clone)")
            {
                self.gameObject.Child("Impact").GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "white_solid")
            {
                self.gameObject.RemoveComponent<SpriteRenderer>();
                GameObject bossControl = GameObject.Find("Boss Control");
                bossControl.Child("Light Solid").RemoveComponent<SpriteRenderer>();
            }

            else if (self.name == "Shade Hit Vignette")
            {
                self.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Fireball(Clone)" || self.name == "Fireball Top(Clone)" ||
                     self.name == "Fireball2 Top(Clone)" || self.name.Contains("Fireball2 Spiral(Clone)"))
            {
                self.gameObject.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Roar Wave Emitter Scream(Clone)")
            {
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Q Slam")
            {
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            else if (self.name == "Death Explode Boss(Clone)")
            {
                self.gameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
                self.gameObject.Child("Splat Explode Orange").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("Orange Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "Roar Wave Emitter(Clone)")
            {
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 1").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 2").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("Ghost Death"))
            {
                if (self.name == "Ghost Death(Clone)")
                {
                    self.gameObject.Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                }

                if (!_ghostExploding)
                {
                    _ghostExploding = true;
                    self.gameObject.Child("White Wave").SetActive(false);
                }

                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name.Contains("Ghost Warrior"))
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                _dontDestroyOnLoadScene.FindGameObject("Dream Impact(Clone)").GetComponent<MeshRenderer>().enabled =
                    false;
            }

            else if (self.name == "Silhouette")
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (self.name == "White Flash")
            {
                self.gameObject.TryGetComponent(out SpriteRenderer sRenderer);
                if (sRenderer != null)
                {
                    sRenderer.enabled = false;
                }
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (_ghostExploding) _ghostExploding = false;
            if (newScene.name.Contains("GG_End_Sequence"))
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

            else if (newScene.name == "Deepnest_East_Hornet")
            {
                IEnumerable<GameObject> blizzardParticles =
                    newScene.GetAllGameObjects().Where(o => o.name.Contains("blizzard_particles"));
                foreach (var blizzardParticle in blizzardParticles)
                {
                    blizzardParticle.SetActive(false);
                }
            }

            else if (newScene.name == "Dream_Final_boss")
            {
                GameObject.Find("Boss Control").Child("Radiance Roar").RemoveComponent<SpriteFlash>();
            }
        }

        private bool OnEnableEnemy(GameObject enemy, bool isalreadydead)
        {
            //Log(enemy.name);
            if (enemy.name.Contains("Absolute Radiance"))
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

            else if (enemy.name == "Mage Lord")
            {
                enemy.Child("Appear Flash").GetComponent<MeshRenderer>().enabled = false;
            }

            else if (enemy.name.Contains("Mega Jellyfish"))
            {
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
            }

            else if (enemy.name == "Radiance")
            {
                enemy.Child("Slash Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Final Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Death Eye Glow").GetComponent<SpriteRenderer>().enabled = false;
                enemy.Child("Break Effect").Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Death").Child("Knight Split").GetComponent<MeshRenderer>().enabled = false;
            }

            else if (enemy.name == "Mage Lord Phase2")
            {
                enemy.Child("Appear Flash").GetComponent<MeshRenderer>().enabled = false;
            }

            return isalreadydead;
        }

        private void OnHeroAwake(On.HeroController.orig_Awake orig, HeroController self)
        {
            orig(self);
            RemoveHeroFlashes();
            _emptyGo = new GameObject();
            Object.DontDestroyOnLoad(_emptyGo);
            _dontDestroyOnLoadScene = _emptyGo.scene;
        }

        private void RemoveHeroFlashes()
        {
            GameObject knight = GameObject.Find("Knight");

            GameObject spells = knight.Child("Spells");
            spells.Child("Q Flash Slam").GetComponent<SpriteRenderer>().enabled = false;
            spells.Child("Q Flash Start").GetComponent<SpriteRenderer>().enabled = false;
            spells.Child("Q Slam 2").GetComponent<MeshRenderer>().enabled = false;

            GameObject focusEffects = knight.Child("Focus Effects");
            focusEffects.Child("Heal Anim").GetComponent<MeshRenderer>().enabled = false;

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
                if (knightFlash.name.Contains("SD Sharp Flash"))
                {
                    knightFlash.GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    knightFlash.GetComponent<SpriteRenderer>().enabled = false;
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

            foreach (var poolFlash in flashesInPool)
            {
                poolFlash.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        private void RemoveMageLordFlashes()
        {
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
    }
}