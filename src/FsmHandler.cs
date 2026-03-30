using Modding.Utils;
using Satchel;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace LessFlashingLights
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class LessFlashingLights
    {
        private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

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
                
                if(Gs.RemoveMajorItemPickupsFlashes)
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
            
            //NKG bats. Also used in plenty of other places but breaks stuff outside the fights hence the verification
            else if (self.name == "Spawn Flash" && Gs.ToneDownGrimmKinFights && _inGrimmFight)
            {
                self.enabled = false;
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (self.name == "gg_battle_transitions(Clone)" && Gs.RemoveGodhomeFlashes)
            {
                GameObject battleEnter = self.gameObject.Child("battle_enter");
                GameObject battleEnd = self.gameObject.Child("battle_end");//also used in HoG when starting a battle
                GameObject finalBattle = self.gameObject.Child("battle_final");
                
                GameObject enterTransitionFlash = battleEnter.Child("pale_glower (2)");
                
                GameObject endTransitionFlash1 = battleEnd.Child("pale_glower (1)");
                GameObject endTransitionFlash0 = battleEnd.Child("pale_glower");
                GameObject battleEndParticles = battleEnd.Child("white_palace_particles");
                
                GameObject finalTransitionFlash1 = finalBattle.Child("pale_glower (1)");
                GameObject finalTransitionFlash0 = finalBattle.Child("pale_glower");
                GameObject finalTransitionParticles = finalBattle.Child("white_palace_particles");
                //I haven't found yet where the "final" ones are used but let's remove them anyway
                
                if (enterTransitionFlash)
                {
                    enterTransitionFlash.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (endTransitionFlash1)
                {
                    endTransitionFlash1.GetComponent<SpriteRenderer>().enabled = false;
                }
                
                if (endTransitionFlash0)
                {
                    endTransitionFlash0.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (finalTransitionFlash1)
                {
                    finalTransitionFlash1.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (finalTransitionFlash0)
                {
                    finalTransitionFlash0.GetComponent<SpriteRenderer>().enabled = false;
                }
                
                if(battleEndParticles) battleEndParticles.SetActive(false);
                if(finalTransitionParticles) finalTransitionParticles.SetActive(false);
                
                GameObject transitionParticlesFG1 = battleEnter.Child("Particle System FG (1)");
                GameObject transitionParticlesBG1 = battleEnter.Child("Particle System BG (1)");
                GameObject transitionParticlesBG2 = battleEnter.Child("Particle System BG (2)");
                GameObject transitionKnightMagicParticles = battleEnter.Child("knight_follow_magic");

                if (transitionParticlesBG1 && transitionParticlesBG2 && 
                    transitionParticlesFG1 && transitionKnightMagicParticles)
                {
                    transitionParticlesBG1.GetComponent<ParticleSystemRenderer>().enabled = false;
                    transitionParticlesBG2.GetComponent<ParticleSystemRenderer>().enabled = false;
                    transitionParticlesFG1.GetComponent<ParticleSystemRenderer>().enabled = false;
                    transitionKnightMagicParticles.GetComponent<ParticleSystemRenderer>().enabled = false;
                }
            }

            else if (self.name.Contains("white_light") && Gs.RemoveMajorItemPickupsFlashes && _inShadeSoulPickup)
            {
                self.gameObject.SetActive(false);
            }
            
            else if (self.name == "Cutscene Dreamer(Clone)" && Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene)
            {
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("Burst Pt").GetComponent<ParticleSystemRenderer>().enabled = false;
            }
            
            else if (self.name == "Blast" && Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene)
            {
                self.gameObject.SetActive(false);
            }
            
            else if (self.name == "dream_area_effect(Clone)" && Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene)
            {
                self.gameObject.Child("lantern_glow_074").RemoveComponent<SpriteRenderer>();
            }
            
            else if (self.name == "Init Blast" && Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene)
            {
                self.gameObject.SetActive(false);
            }
            
            else if (self.name == "SelfStab Flash" && Gs.RemoveTHKSpecificFlashes)
            {
                self.gameObject.SetActive(false);
            }
            
            else if ((self.name == "Counter Flash" || self.name == "Flash Effect") && _inHornetFight && Gs.ToneDownHornetfights)
            {
                self.gameObject.SetActive(false);
            }
            
            //crossroads explosions
            if (self.name == "Gas Explosion L(Clone)" && Gs.ToneDownExplosions)
            {
                self.gameObject.Child("orange flash").GetComponent<SpriteRenderer>().enabled = false;
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
    }
}