using Modding.Utils;
using Satchel;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace LessFlashingLights;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class LessFlashingLights
{
    //helper for type inference
    private static (Func<string, bool>, Action<PlayMakerFSM>) FSMPredicate(Func<string, bool> predicate, Action<PlayMakerFSM> handler)
        => (predicate, handler);

    private static readonly (Func<string, bool> predicate, Action<PlayMakerFSM> handler)[] FsmPredicateHandlers = new[]
    {
        FSMPredicate(
            name => name.Contains("white_light") && Gs!.RemoveMajorItemPickupsFlashes && _inShadeSoulPickup,
            fsm => fsm.gameObject.SetActive(false)
        ),
        
        FSMPredicate(
            name => name.Contains("Gas Explosion Uumuu") && Gs!.ToneDownUumuuFight,
            _ => _dontDestroyOnLoadScene.FindGameObject("Gas Explosion Uumuu(Clone)").Child("orange flash")
                .GetComponent<SpriteRenderer>().enabled = false
        ),
        
        FSMPredicate(
            name => name.Contains("Ghost Warrior") && Gs!.ToneDownWarriorDreamsFlashes,
            fsm => fsm.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false
        ),
        
        FSMPredicate(
            name => name.Contains("Ghost Death") && Gs!.ToneDownWarriorDreamsFlashes,
            RemoveGhostDeathFlashes
        ),
        
        FSMPredicate(
            name => name.Contains("End Flash 2"),//Seen in major item pickups and during soul master fight
            RemoveEndFlash2
        ),
        
        FSMPredicate(
            name => name.Contains("Tele Out Corpse R(Clone)") && Gs!.ToneDownMageLordFight,
            RemoveMageLordFlashes
        ),
        
        FSMPredicate(
            name => name.Contains("Corpse Dream Mage Lord 1(Clone)") && Gs!.ToneDownMageLordFight,
            _ => RemoveSoulTyrantCorpseFlashes()
        ),
        
        FSMPredicate(
            name => name.Contains("Fireball2 Spiral(Clone)") && Gs!.RemoveSpellFlashes,
            fsm => fsm.gameObject.Child("white_light").GetComponent<SpriteRenderer>().enabled = false
        ),
    };
    
    private void RemoveGrimmchildFlashes(PlayMakerFSM fsm)
    {
        GameObject grimmChildBurst = fsm.gameObject.Child("Burst");
        GameObject grimmChildBurst2 = fsm.gameObject.Child("Burst 2");

        if (grimmChildBurst && grimmChildBurst2)
        {
            grimmChildBurst.RemoveComponent<MeshRenderer>();
            grimmChildBurst.RemoveComponent<tk2dSprite>();
            grimmChildBurst.RemoveComponent<tk2dSpriteAnimator>();
            grimmChildBurst.RemoveComponent<DeactivateAfter2dtkAnimation>();
                
            grimmChildBurst2.RemoveComponent<MeshRenderer>();
            grimmChildBurst2.RemoveComponent<tk2dSprite>();
            grimmChildBurst2.RemoveComponent<tk2dSpriteAnimator>();
            grimmChildBurst2.RemoveComponent<DeactivateAfter2dtkAnimation>();
        }
    }

    private void RemoveGodhomeFlashes(PlayMakerFSM fsm)
    {
        GameObject battleEnter = fsm.gameObject.Child("battle_enter");
        GameObject battleEnd = fsm.gameObject.Child("battle_end");//also used in HoG when starting a battle
        GameObject finalBattle = fsm.gameObject.Child("battle_final");
            
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
    
    private static void RemoveMageLordFlashes(PlayMakerFSM fsm)
    {
        fsm.gameObject.GetComponent<MeshRenderer>().enabled = false;
        
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

    private static void RemoveGhostDeathFlashes(PlayMakerFSM fsm)
    {
        if (fsm.name == "Ghost Death(Clone)")
        {
            fsm.gameObject.Child("Dream Impact").GetComponent<MeshRenderer>().enabled = false;
        }

        if (!_ghostExploding)
        {
            _ghostExploding = true;//I don't remember why I did that but sure
            fsm.gameObject.Child("White Wave").SetActive(false);
        }

        fsm.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
    }

    private static void RemoveEndFlash2(PlayMakerFSM fsm)
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

    private static void RemoveSoulTyrantCorpseFlashes()
    {
        GameObject secondCorpse = GameObject.Find("Corpse Dream Mage Lord 1(Clone)");
        secondCorpse.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
        secondCorpse.Child("white_light 1").GetComponent<SpriteRenderer>().enabled = false;
        secondCorpse.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
    }
    
    private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        orig(self);

        switch (self.name)
        {
            case "Tele Flash" when Gs.ToneDownRadianceFightsFlashes:
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;

            case "Radiant Orb(Clone)" when Gs.ToneDownRadianceFightsFlashes:
                self.gameObject.Child("Impact").GetComponent<MeshRenderer>().enabled = false;
                break;

            case "white_solid" when Gs.ToneDownRadianceFightsFlashes:
                self.gameObject.RemoveComponent<SpriteRenderer>();
                GameObject bossControl = GameObject.Find("Boss Control");
                bossControl.Child("Light Solid").RemoveComponent<SpriteRenderer>();
                break;

            case "Shade Hit Vignette" when Gs.ToneDownRadianceFightsFlashes:
                self.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                break;
            
            case  "Roar Wave Emitter Scream(Clone)" when Gs.RemoveBossScreams:
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
                break;

            case "Q Slam" when Gs.RemoveSpellFlashes:
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;

            case "Death Explode Boss(Clone)" when Gs.ToneDownDeathExplosions:
                self.gameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
                self.gameObject.Child("Splat Explode Orange").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("Orange Flash").GetComponent<SpriteRenderer>().enabled = false;
                break;

            case "Roar Wave Emitter(Clone)" when Gs.RemoveBossScreams:
                self.gameObject.Child("lines").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 1").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("wave 2").GetComponent<SpriteRenderer>().enabled = false;
                break;
            
            case "Silhouette" when Gs.ToneDownMageLordFight://part of soul master
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                break;

            case "White Flash" when Gs.RemoveGenericFlashingEffects:
                self.gameObject.TryGetComponent(out SpriteRenderer sRenderer);
                if (sRenderer)
                {
                    sRenderer.enabled = false;
                }
                break;
        
            //NKG bats. Also used in plenty of other places but breaks stuff outside the fights hence the verification
            case "Spawn Flash" when Gs.ToneDownGrimmKinFights && _inGrimmFight:
                self.enabled = false;
                self.gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;
            
            case "Cutscene Dreamer(Clone)" when Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene:
                self.gameObject.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                self.gameObject.Child("Burst Pt").GetComponent<ParticleSystemRenderer>().enabled = false;
                break;
        
            case "Blast" when Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene:
                self.gameObject.SetActive(false);
                break;
            
            case "dream_area_effect(Clone)" when Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene:
                self.gameObject.Child("lantern_glow_074").RemoveComponent<SpriteRenderer>();
                break;
        
            case "Init Blast" when Gs.RemoveDreamerCutsceneFlashes && _inDreamerCutscene:
                self.gameObject.SetActive(false);
                break;
        
            case "SelfStab Flash" when Gs.RemoveTHKSpecificFlashes:
                self.gameObject.SetActive(false);
                break;
            
            //for soul vessel/mash shards
            case "Get Glow" when Gs.RemoveMajorItemPickupsFlashes:
                self.gameObject.SetActive(false);
                break;
            
            //crossroads explosions
            case "Gas Explosion L(Clone)" when Gs.ToneDownExplosions:
                self.gameObject.Child("orange flash").GetComponent<SpriteRenderer>().enabled = false;
                break;
            
            //grimmchild spawn/despawn
            case "Grimmchild(Clone)" when Gs.RemoveGrimmChildFlashes:
                RemoveGrimmchildFlashes(self);
                break;
            
            case "gg_battle_transitions(Clone)" when Gs.RemoveGodhomeFlashes:
                RemoveGodhomeFlashes(self);
                break;
            
            //hornet counter & thread storm
            case "Counter Flash":
            case "Flash Effect":
                if (_inHornetFight && Gs.ToneDownHornetfights)
                {
                    self.gameObject.SetActive(false);
                }
                break;
            
            case "Fireball(Clone)":
            case "Fireball Top(Clone)":
            case "Fireball2 Top(Clone)":
                if(Gs.RemoveSpellFlashes) self.gameObject.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
                break;
        }
        
        foreach (var (predicate, handler) in FsmPredicateHandlers)
        {
            if (predicate(self.name)) handler(self);
        }
    }
}