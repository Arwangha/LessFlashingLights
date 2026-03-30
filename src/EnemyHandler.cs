using Satchel;
using UnityEngine;

namespace LessFlashingLights
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class LessFlashingLights
    {
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
            
            else if (enemy.name.Contains("Ceiling Dropper") && Gs.ToneDownExplosions)
            {
                GameObject explosion = enemy.Child("Gas Explosion M2(Clone)");
                if(explosion) explosion.Child("orange flash").SetActive(false);
                //Log("Ceiling dropper flash removed");
            }
            
            else if (enemy.name.Contains("Grimm Boss") && Gs.ToneDownGrimmKinFights)
            {
                _inGrimmFight = true;
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
            
            else if (enemy.name.Contains("Real Bat") && Gs.ToneDownDeathExplosions)
            {
                enemy.Child("white_light").GetComponent<SpriteRenderer>().enabled = false;
            }
            
            else if (enemy.name == "White Defender" && Gs.RemoveWhiteDefenderFlashes)
            {
                GameObject roarEffects = enemy.Child("Roar Effects");
                
                if (roarEffects)
                {
                    roarEffects.Child("White Flash").GetComponent<SpriteRenderer>().enabled = false;
                    roarEffects.Child("White Wave").GetComponent<SpriteRenderer>().enabled = false;
                    roarEffects.Child("Pt Roar").GetComponent<ParticleSystemRenderer>().enabled = false;
                }
                
                enemy.Child("Entry Glow").GetComponent<MeshRenderer>().enabled = false;
                enemy.Child("Antic Flash").GetComponent<MeshRenderer>().enabled = false;
            }
            
            else if (enemy.name.Contains("Hornet Boss"))
            {
                _inHornetFight = true;
            }
            
            return isalreadydead;
        }
    }
}