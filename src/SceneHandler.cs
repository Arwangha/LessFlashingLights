using System.Collections;
using Satchel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ReSharper disable InconsistentNaming

namespace LessFlashingLights
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class LessFlashingLights
    {
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (_ghostExploding) _ghostExploding = false;
            if(_inGrimmFight)  _inGrimmFight = false;//we've exited the fight. set to true when grimm/nkg gets enabled
            if(_inShadeSoulPickup) _inShadeSoulPickup = false;
            if(_inDreamerCutscene) _inDreamerCutscene = false;
            
            if (newScene.name == "Crossroads_ShamanTemple" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveAncestralMoundFlashes());
            }
            
            else if (newScene.name == "Room_Fungus_Shaman" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveOvergrownMoundFlashes());
            }
            
            else if (newScene.name == "Ruins1_24" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveRealQuakeFlashes());
            }
            
            else if (newScene.name == "Dream_Abyss" && Gs.ToneDownBirthPlaceFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveBirthPlaceCutsceneFlashes());
            }
            
            else if (newScene.name == "Abyss_15" && Gs.ToneDownBirthPlaceFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveBirthPlaceTriggerFlashes());
            }
            
            else if (newScene.name.Contains("GG_End_Sequence") && Gs.RemoveGodhomeFlashes)
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
            
            else if (newScene.name == "Fungus3_archive_02" && Gs.RemoveDreamerCutsceneFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveQuirrelArchivesCutsceneFlashes());
            }
            
            else if (newScene.name == "GG_Hollow_Knight" && Gs.RemoveGodhomeFlashes)//PV got lucky enough to have their own transition flashes on top of the rest
            {
                GameManager.instance.StartCoroutine(RemovePVTransitionFlash());
            }
            
            else if (newScene.name == "Abyss_21" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveWingsPickupFlashes());
            }
            
            else if (newScene.name == "Ruins1_31b")
            {
                _inShadeSoulPickup = true;
            }
            
            else if (newScene.name == "Dream_Nailcollection" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveDreamNailPickupFlashes());
            }
            
            else if (newScene.name == "GG_Atrium" && Gs.RemoveGodhomeFlashes)
            {
                //no idea why there's 5 in Atrium but let's be safe
                GameManager.instance.StartCoroutine(RemoveAtriumPantheonUnlockFlashes("GG_Challenge_Door"));
                GameManager.instance.StartCoroutine(RemoveAtriumPantheonUnlockFlashes("GG_Challenge_Door (1)"));
                GameManager.instance.StartCoroutine(RemoveAtriumPantheonUnlockFlashes("GG_Challenge_Door (2)"));
                GameManager.instance.StartCoroutine(RemoveAtriumPantheonUnlockFlashes("GG_Challenge_Door (3)"));
                GameManager.instance.StartCoroutine(RemoveAtriumPantheonUnlockFlashes("GG_Challenge_Door (4)"));
            }
            
            else if (newScene.name == "GG_Atrium_Roof" && Gs.RemoveGodhomeFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveRoofPantheonUnlockFlashes());
            }
            
            else if (newScene.name == "RestingGrounds_04" && Gs.RemoveDreamerCutsceneFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveDreamersBindingShieldFlashes());
                _inDreamerCutscene = true;
            }
            
            else if (newScene.name == "Fungus1_04")
            {
                _inDreamerCutscene = true;
            }
            
            else if (newScene.name == "RestingGrounds_07" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveDreamNailUpgradeFlashes());
            }
            
            else if (newScene.name == "Waterways_13" && Gs.RemoveMajorItemPickupsFlashes)
            {
                GameManager.instance.StartCoroutine(RemoveIsmaObtentionFlashes());
            }
            
            else if (newScene.name == "Room_Final_Boss_Core" && Gs.RemoveTHKSpecificFlashes)
            {
                IEnumerable<GameObject> hollowKnightChains =
                    newScene.GetAllGameObjects().Where(o => o.name.Contains("hollow_knight_chain_base") && !o.name.Contains("("));

                foreach (GameObject chain in hollowKnightChains)
                {
                    GameObject sharpFlash = chain.Child("Sharp Flash");
                    GameObject burst = chain.Child("Burst");

                    if (sharpFlash)
                    {
                        sharpFlash.GetComponent<MeshRenderer>().enabled = false;
                        sharpFlash.GetComponent<PlayMakerFSM>().enabled = false;
                    }

                    if (burst)
                    {
                        burst.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                
                GameObject bossControl = GameObject.Find("Boss Control");
                GameObject shieldShatter = bossControl.Child("Shield Shatter");
                GameObject hornetFlash = bossControl.Child("Hollow Knight Boss").Child("Hornet Flash");
                GameObject counterFlash = bossControl.Child("Hollow Knight Boss").Child("Counter Flash");

                if (shieldShatter)
                {
                    shieldShatter.GetComponent<SpriteRenderer>().enabled = false;
                    GameObject shieldSharpFlash = shieldShatter.Child("Sharp Flash");
                    if (shieldSharpFlash)
                    {
                        shieldSharpFlash.GetComponent<MeshRenderer>().enabled = false;
                        shieldSharpFlash.GetComponent<PlayMakerFSM>().enabled = false;
                    }
                }

                if (hornetFlash)
                {
                    hornetFlash.GetComponent<MeshRenderer>().enabled = false;
                    hornetFlash.GetComponent<PlayMakerFSM>().enabled = false;
                }

                if (counterFlash)
                {
                    counterFlash.GetComponent<MeshRenderer>().enabled = false;
                    counterFlash.GetComponent<PlayMakerFSM>().enabled = false;
                }
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

        private IEnumerator RemoveIsmaObtentionFlashes()
        {
            yield return new WaitForFinishedEnteringScene();

            GameObject shinyItemAcid = GameObject.Find("Shiny Item Acid");

            if (!shinyItemAcid) yield break;
            
            GameObject acidFlash = shinyItemAcid.Child("Flash");
            
            if (acidFlash)
            {
                acidFlash.GetComponent<SpriteRenderer>().enabled = false;
                acidFlash.GetComponent<PlayMakerFSM>().enabled = false;
            }
        }

        private IEnumerator RemoveDreamNailUpgradeFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject dreamMoth = GameObject.Find("Dream Moth");
            if (dreamMoth)
            {
                GameObject mothFlash = dreamMoth.Child("Flash");
                GameObject dreamImpact = dreamMoth.Child("Dream Impact");

                if (dreamImpact && mothFlash)
                {
                    dreamImpact.GetComponent<MeshRenderer>().enabled = false;
                    dreamImpact.GetComponent<PlayMakerFSM>().enabled = false;
                    mothFlash.GetComponent<MeshRenderer>().enabled = false;
                    mothFlash.GetComponent<PlayMakerFSM>().enabled = false;
                }
            }
        }

        private IEnumerator RemoveDreamersBindingShieldFlashes()
        {
            yield return null;
            
            GameObject dreamerScene = GameObject.Find("Dreamer Scene 2");
            GameObject initBlast = dreamerScene.Child("Init Blast");
            GameObject bindingShieldActivate = GameObject.Find("Binding Shield Activate");
            GameObject bindingShieldStatues = bindingShieldActivate.Child("Binding Shield Statues");
            
            if(initBlast)
            {
                initBlast.GetComponent<MeshRenderer>().enabled = false;
                initBlast.Child("Attack Pt").GetComponent<ParticleSystemRenderer>().enabled = false;
            }

            if (bindingShieldStatues)
            {
                bindingShieldStatues.Child("white_fader").GetComponent<SpriteRenderer>().enabled = false; 
                bindingShieldStatues.Child("binding shield flash").GetComponent<SpriteRenderer>().enabled = false;
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

        private IEnumerator RemovePVTransitionFlash()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject whiteSceneGlow = GameObject.Find("/white_scene_glow");
            GameObject whiteSceneGlow1 = GameObject.Find("/white_scene_glow (1)");
            if (whiteSceneGlow && whiteSceneGlow1)
            {
                whiteSceneGlow.SetActive(false);
                whiteSceneGlow1.SetActive(false);
            }
            
            GameObject counterFlash = GameObject.Find("/Battle Scene/HK Prime/Counter Flash");
            if (counterFlash)
            {
                counterFlash.RemoveComponent<MeshRenderer>();
                counterFlash.RemoveComponent<tk2dSprite>();
                counterFlash.RemoveComponent<tk2dSpriteAnimator>();
                counterFlash.RemoveComponent<DeactivateAfter2dtkAnimation>();
            }
        }

        private IEnumerator RemoveWingsPickupFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject shinyItemDJ = GameObject.Find("Shiny Item DJ");
            if (shinyItemDJ)
            {
                GameObject djWhiteFlash = shinyItemDJ.Child("White Flash");
                if (djWhiteFlash) djWhiteFlash.GetComponent<SpriteRenderer>().enabled = false;
                
                GameObject djWhiteWave = shinyItemDJ.Child("White Wave Get");
                if (djWhiteWave) djWhiteWave.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        private IEnumerator RemoveDreamNailPickupFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject dreamNailGet = GameObject.Find("Witch Control/Dream Nail Get");

            if (dreamNailGet)
            {
                dreamNailGet.Child("Pickup Glow").GetComponent<MeshRenderer>().enabled = false;
                dreamNailGet.Child("Get Flash").GetComponent<SpriteRenderer>().enabled = false;
                
                dreamNailGet.Child("Dream Nail").Child("Death Glow").GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private IEnumerator RemoveAtriumPantheonUnlockFlashes(string bossDoorName)
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject door = GameObject.Find(bossDoorName);
            
            GameObject lockBreak = door.Child("Door").Child("Lock Break");
            GameObject lockSet = door.Child("Door").Child("Lock Set");
            
            if (!lockBreak || !lockSet) yield break;
            
            lockSet.Child("white_fader").GetComponent<SpriteRenderer>().enabled = false;
            lockSet.Child("Lock Break Antic Pt").GetComponent<ParticleSystemRenderer>().enabled = false;
            
            lockBreak.Child("White Flash R").SetActive(false);
            lockBreak.Child("Death Glow").SetActive(false);
            lockBreak.Child("glow pt").GetComponent<ParticleSystemRenderer>().enabled = false;
        }
        
        //because of course this one has a different hierarchy
        private IEnumerator RemoveRoofPantheonUnlockFlashes()
        {
            yield return new WaitForFinishedEnteringScene();
            
            GameObject door = GameObject.Find("GG_Final_Challenge_Door");
            
            GameObject lockBreak = door.Child("Lock Break");
            GameObject lockSet = door.Child("Lock Set");
            
            if (!lockBreak || !lockSet) yield break;
            
            lockSet.Child("white_fader").GetComponent<SpriteRenderer>().enabled = false;
            lockSet.Child("Lock Break Antic Pt").GetComponent<ParticleSystemRenderer>().enabled = false;
            
            lockBreak.Child("White Flash R").SetActive(false);
            lockBreak.Child("Death Glow").SetActive(false);
            lockBreak.Child("glow pt").GetComponent<ParticleSystemRenderer>().enabled = false;
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
    }
}