using UnityEngine;
namespace NoFlashingLights;

public class EnemyHitEffectHandler
{
    public static void OnReceiveGhostHitEffect(On.EnemyHitEffectsGhost.orig_RecieveHitEffect orig, EnemyHitEffectsGhost self, float attackDirection)
    {
        Modding.Logger.Log("Ghost Hit Effect");//warrior dreams
        orig(self, attackDirection);
    }

    public static void OnReceiveBlackKnightHitEffect(On.EnemyHitEffectsBlackKnight.orig_RecieveHitEffect orig, EnemyHitEffectsBlackKnight self, float attackDirection)
    {
        Modding.Logger.Log("Black Knight Hit Effect");//watcher knights
        orig(self, attackDirection);
    }

    public static void OnReceiveArmouredHitEffect(On.EnemyHitEffectsArmoured.orig_RecieveHitEffect orig, EnemyHitEffectsArmoured self, float attackDirection)
    {
        Modding.Logger.Log("Armoured Hit Effect");//false knight
        orig(self, attackDirection);
    }

    public static void OnReceiveShadeHitEffect(On.EnemyHitEffectsShade.orig_RecieveHitEffect orig, EnemyHitEffectsShade self, float attackDirection)
    {
        Modding.Logger.Log("Shade Hit Effect");//at least collector & shade
        orig(self, attackDirection);
    }

    public static void OnReceiveUninfectedHitEffect(On.EnemyHitEffectsUninfected.orig_RecieveHitEffect orig, EnemyHitEffectsUninfected self, float attackDirection)
    {
        Modding.Logger.Log("Uninfected Hit Effect");//at least hornet
        orig(self, attackDirection);
    }
}