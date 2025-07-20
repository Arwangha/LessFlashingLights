using UnityEngine;
namespace NoFlashingLights;

public static class FlashHandler
{
    public static void OnFlashWhitePulse(On.SpriteFlash.orig_flashWhitePulse orig, SpriteFlash self)//whispering roots "wake up"
    {
        Modding.Logger.Log("OnFlashWhitePulse");
        orig(self);
    }

    public static void OnFlashSpore(On.SpriteFlash.orig_flashSporeQuick orig, SpriteFlash self)//all sporeshroom effects
    {
        Modding.Logger.Log("OnFlashSpore");
        orig(self);
    }

    public static void OnFlashShadeGet(On.SpriteFlash.orig_flashShadeGet orig, SpriteFlash self)//collecting shade
    {
        Modding.Logger.Log("OnFlashShadeGet");
        orig(self);
    }

    public static void OnFlashSuperDash(On.SpriteFlash.orig_FlashingSuperDash orig, SpriteFlash self)//when superdashing
    {
        Modding.Logger.Log("OnFlashSuperDash");
        orig(self);
    }

    public static void OnFlashingGhostWounded(On.SpriteFlash.orig_FlashingGhostWounded orig, SpriteFlash self)//???
    {
        Modding.Logger.Log("OnFlashingGhostWounded");
        orig(self);
    }

    public static void OnFlashInfected(On.SpriteFlash.orig_flashInfected orig, SpriteFlash self)//when hitting an infected foe
    {
        Modding.Logger.Log("OnFlashInfected");
        orig(self);
    }

    public static void OnFlashHealBlue(On.SpriteFlash.orig_flashHealBlue orig, SpriteFlash self)//collecting lifeblood
    {
        Modding.Logger.Log("OnFlashHealBlue");
        orig(self);
    }

    public static void OnFlashGrimmHit(On.SpriteFlash.orig_FlashGrimmHit orig, SpriteFlash self)//when grimmchild hits something
    {
        Modding.Logger.Log("OnFlashGrimmHit");
        orig(self);
    }

    public static void OnFlashGrimmFlame(On.SpriteFlash.orig_FlashGrimmflame orig, SpriteFlash self)//collecting grimm flames
    {
        Modding.Logger.Log("OnFlashGrimmFlame");
        orig(self);
    }

    public static void OnFlashHeal(On.SpriteFlash.orig_flashFocusHeal orig, SpriteFlash self)//when healing & hitting grimm/nkg/warrior dreams/radiance
    {
        Modding.Logger.Log("OnFlashHeal");
        orig(self);
    }

    public static void OnFlashFocusGet(On.SpriteFlash.orig_flashFocusGet orig, SpriteFlash self)//when getting enough soul to focus
    {
        Modding.Logger.Log("OnFlashFocusGet");
        orig(self);
    }

    public static void OnFlashDungQuick(On.SpriteFlash.orig_flashDungQuick orig, SpriteFlash self)//hitting something with crest
    {
        Modding.Logger.Log("OnFlashDungQuick");
        orig(self);
    }

    public static void OnFlashDream(On.SpriteFlash.orig_flashDreamImpact orig, SpriteFlash self)//radiance spawn
    {
        Modding.Logger.Log("OnFlashDream");
        orig(self);
    }

    public static void OnFlashBench(On.SpriteFlash.orig_flashBenchRest orig, SpriteFlash self)//triggered when benching
    {
        Modding.Logger.Log("OnFlashBench");
        orig(self);
    }

    public static void OnFlashArmoured(On.SpriteFlash.orig_flashArmoured orig, SpriteFlash self)//false knight
    {
        Modding.Logger.Log("OnFlashArmoured");
        orig(self);
    }
}