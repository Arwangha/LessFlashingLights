namespace LessFlashingLights;

public static class FlashHandler
{
    public static void OnFlashWhitePulse(On.SpriteFlash.orig_flashWhitePulse orig, SpriteFlash self)//whispering roots "wake up"
    {
        if(!LessFlashingLights.Gs.ToneDownDreamOrbs) orig(self);
    }

    public static void OnFlashSpore(On.SpriteFlash.orig_flashSporeQuick orig, SpriteFlash self)//all sporeshroom effects
    {
        if(!LessFlashingLights.Gs.RemoveSporeShroomFlashes) orig(self);
    }

    public static void OnFlashShadeGet(On.SpriteFlash.orig_flashShadeGet orig, SpriteFlash self)//collecting shade
    {
        if(!LessFlashingLights.Gs.RemoveShadeGetFlashes) orig(self);
    }

    public static void OnFlashSuperDash(On.SpriteFlash.orig_FlashingSuperDash orig, SpriteFlash self)//when superdashing
    {
        if(!LessFlashingLights.Gs.RemoveCrystalDashFlashes) orig(self);
    }

    public static void OnFlashInfected(On.SpriteFlash.orig_flashInfected orig, SpriteFlash self)//when hitting an infected foe
    {
        if(!LessFlashingLights.Gs.RemoveInfectedHitFlashes) orig(self);
    }

    public static void OnFlashHealBlue(On.SpriteFlash.orig_flashHealBlue orig, SpriteFlash self)//collecting lifeblood
    {
        if(!LessFlashingLights.Gs.RemoveLifebloodCollectionFlashes) orig(self);
    }

    public static void OnFlashGrimmHit(On.SpriteFlash.orig_FlashGrimmHit orig, SpriteFlash self)//when grimmchild hits something
    {
        if(!LessFlashingLights.Gs.RemoveGrimmChildHitFlashes) orig(self);
    }

    public static void OnFlashGrimmFlame(On.SpriteFlash.orig_FlashGrimmflame orig, SpriteFlash self)//collecting grimm flames
    {
        if(!LessFlashingLights.Gs.ToneDownGrimmKinFights) orig(self);
    }

    public static void OnFlashHeal(On.SpriteFlash.orig_flashFocusHeal orig, SpriteFlash self)//when healing & hitting grimm/nkg/warrior dreams/radiance/lots of uninfected foes
    {
        if(!LessFlashingLights.Gs.RemoveGenericEnemyHitFlashes) orig(self);
    }

    public static void OnFlashFocusGet(On.SpriteFlash.orig_flashFocusGet orig, SpriteFlash self)//when getting enough soul to focus
    {
        if(!LessFlashingLights.Gs.RemoveGenericHeroFlashes) orig(self);
    }

    public static void OnFlashDungQuick(On.SpriteFlash.orig_flashDungQuick orig, SpriteFlash self)//hitting something with crest
    {
        if(!LessFlashingLights.Gs.RemoveCrestFlashes) orig(self);
    }

    public static void OnFlashDream(On.SpriteFlash.orig_flashDreamImpact orig, SpriteFlash self)//radiance spawn
    {
        if(!LessFlashingLights.Gs.ToneDownRadianceFightsFlashes) orig(self);
    }

    public static void OnFlashBench(On.SpriteFlash.orig_flashBenchRest orig, SpriteFlash self)//triggered when benching
    {
        if(!LessFlashingLights.Gs.RemoveHealFlashes) orig(self);
    }

    public static void OnFlashArmoured(On.SpriteFlash.orig_flashArmoured orig, SpriteFlash self)//false knight
    {
        if(!LessFlashingLights.Gs.RemoveGenericEnemyHitFlashes) orig(self);
    }
}