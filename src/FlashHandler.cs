namespace NoFlashingLights;

public static class FlashHandler
{
    public static void OnFlashWhitePulse(On.SpriteFlash.orig_flashWhitePulse orig, SpriteFlash self)//whispering roots "wake up"
    {
        if(!NoFlashingLights.Gs.ToneDownDreamOrbs) orig(self);
    }

    public static void OnFlashSpore(On.SpriteFlash.orig_flashSporeQuick orig, SpriteFlash self)//all sporeshroom effects
    {
        if(!NoFlashingLights.Gs.RemoveSporeShroomFlashes) orig(self);
    }

    public static void OnFlashShadeGet(On.SpriteFlash.orig_flashShadeGet orig, SpriteFlash self)//collecting shade
    {
        if(!NoFlashingLights.Gs.RemoveShadeGetFlashes) orig(self);
    }

    public static void OnFlashSuperDash(On.SpriteFlash.orig_FlashingSuperDash orig, SpriteFlash self)//when superdashing
    {
        if(!NoFlashingLights.Gs.RemoveCrystalDashFlashes) orig(self);
    }

    public static void OnFlashInfected(On.SpriteFlash.orig_flashInfected orig, SpriteFlash self)//when hitting an infected foe
    {
        if(!NoFlashingLights.Gs.RemoveInfectedHitFlashes) orig(self);
    }

    public static void OnFlashHealBlue(On.SpriteFlash.orig_flashHealBlue orig, SpriteFlash self)//collecting lifeblood
    {
        if(!NoFlashingLights.Gs.RemoveLifebloodCollectionFlashes) orig(self);
    }

    public static void OnFlashGrimmHit(On.SpriteFlash.orig_FlashGrimmHit orig, SpriteFlash self)//when grimmchild hits something
    {
        if(!NoFlashingLights.Gs.RemoveGrimmChildHitFlashes) orig(self);
    }

    public static void OnFlashGrimmFlame(On.SpriteFlash.orig_FlashGrimmflame orig, SpriteFlash self)//collecting grimm flames
    {
        if(!NoFlashingLights.Gs.ToneDownGrimmKinFights) orig(self);
    }

    public static void OnFlashHeal(On.SpriteFlash.orig_flashFocusHeal orig, SpriteFlash self)//when healing & hitting grimm/nkg/warrior dreams/radiance/lots of uninfected foes
    {
        if(!NoFlashingLights.Gs.RemoveGenericEnemyHitFlashes) orig(self);
    }

    public static void OnFlashFocusGet(On.SpriteFlash.orig_flashFocusGet orig, SpriteFlash self)//when getting enough soul to focus
    {
        if(!NoFlashingLights.Gs.RemoveGenericHeroFlashes) orig(self);
    }

    public static void OnFlashDungQuick(On.SpriteFlash.orig_flashDungQuick orig, SpriteFlash self)//hitting something with crest
    {
        if(!NoFlashingLights.Gs.RemoveCrestFlashes) orig(self);
    }

    public static void OnFlashDream(On.SpriteFlash.orig_flashDreamImpact orig, SpriteFlash self)//radiance spawn
    {
        if(!NoFlashingLights.Gs.ToneDownRadianceFightsFlashes) orig(self);
    }

    public static void OnFlashBench(On.SpriteFlash.orig_flashBenchRest orig, SpriteFlash self)//triggered when benching
    {
        if(!NoFlashingLights.Gs.RemoveHealFlashes) orig(self);
    }

    public static void OnFlashArmoured(On.SpriteFlash.orig_flashArmoured orig, SpriteFlash self)//false knight
    {
        if(!NoFlashingLights.Gs.RemoveGenericEnemyHitFlashes) orig(self);
    }
}