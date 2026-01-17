using Modding;
using Satchel.BetterMenus;

namespace LessFlashingLights;

public static class ModMenu
{
    private static Menu? _menuRef;

    public static MenuScreen CreateModMenu(MenuScreen modlistmenu, ModToggleDelegates? toggleDelegates)
    {
        _menuRef ??= new Menu("Less Flashing Lights' Options", new Element[]
        {
            new TextPanel(name: "Note: please reload your save after changing options"),
            
            Blueprints.CreateToggle(
                toggleDelegates: toggleDelegates!.Value,
                name: "Less Flashing Lights",
                description: "Mod Toggle"),
            
            new TextPanel(name: "Knight-specific effects"),
            
            new HorizontalOption(
                name: "Remove flickering on damage",
                description: "Toggle for the flickering effect triggered upon taking damage",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveDamageFlickering = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveDamageFlickering ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove spells flashes",
                description: "Toggle for the flash effects triggered when casting spells",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveSpellFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveSpellFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove spell pickups flashes",
                description: "Toggle for the flash effects triggered when collecting spells",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveSpellPickupsFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveSpellPickupsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove CDash flashes",
                description: "Toggles CDash usage flashes. Partially affects Nail Arts",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveCrystalDashFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveCrystalDashFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove focus flash",
                description: "Toggle for the flash effect triggered when focusing soul",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveHealFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveHealFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Soul Orb flashes",
                description: "Remove the flash effects of the soul orb on the HUD",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveSoulOrbFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveSoulOrbFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Lifeblood flashes",
                description: "Remove the flash effect when collecting lifeblood",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveLifebloodCollectionFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveLifebloodCollectionFlashes ? 0 : 1),
                        
            new HorizontalOption(
                name: "Tone down shade collection",
                description: "targets knight flashes on shade collection",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveShadeGetFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveShadeGetFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove misc. knight flashes",
                description: "Toggles miscellaneous effects with a generic filter. Includes nail arts",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveGenericHeroFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveGenericHeroFlashes ? 0 : 1),
            
            new TextPanel(name: "Enemy hit effects"),
                        
            new HorizontalOption(
                name: "Remove SporeShroom flashes",
                description: "Remove the effect on enemies hit by the spore cloud",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveSporeShroomFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveSporeShroomFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Defender's Crest flashes",
                description: "Remove the effect on enemies hit by the dung cloud",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveCrestFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveCrestFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Infected hit flashes",
                description: "Remove the flash effect on infected enemies",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveInfectedHitFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveInfectedHitFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove GrimmChild hit flashes",
                description: "Remove the flash effect on enemies hit by GrimmChild",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveGrimmChildHitFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveGrimmChildHitFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove white hit flashes",
                description: "Remove the flash effect on most other enemies",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveGenericEnemyHitFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveGenericEnemyHitFlashes ? 0 : 1),
            
            new TextPanel(name: "Fight-specific effects"),
            
            new HorizontalOption(
                name: "Tone down Soul Master/Tyrant",
                description: "notably teleportation, spells and fake death. Includes s. twister",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownMageLordFight = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownMageLordFight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down boss screams",
                description: "Removes the \"shockwave\" visual effect",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveBossScreams = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveBossScreams ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down boss deaths",
                description: "Targets the final explosion following the particles burst",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownDeathExplosions = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownDeathExplosions ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down warrior dreams",
                description: "Targets their teleportation and deaths",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownWarriorDreamsFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownWarriorDreamsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Radiance fights",
                description: "Targets attacks, teleportation and death. Absrad included",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownRadianceFightsFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownRadianceFightsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Uumuu's fight",
                description: "Targets Uumuu's explosions & stagger",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownUumuuFight = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownUumuuFight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Hornet 2 fight",
                description: "Reduces the wind intensity",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownHornet2Fight = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownHornet2Fight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down GrimmKin fights",
                description: "Mainly targets their teleportation flashes",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownGrimmKinFights = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownGrimmKinFights ? 0 : 1),
            
            new TextPanel(name: "Miscellaneous effects"),
            
            new HorizontalOption(
                name: "Tone down birthplace flashes",
                description: "Reduces dream nail impact and ledge grab flashes",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownBirthPlaceFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownBirthPlaceFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down explosions",
                description: "Includes bubbles, belflies, spores",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownExplosions = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownExplosions ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down pantheon victories",
                description: "Targets flashes on the completion & bindings cutscene",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemovePantheonCompletionFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemovePantheonCompletionFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Godhome Statues flashes",
                description: "includes spawn, dream lever switch, completion",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownGodhomeStatues = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownGodhomeStatues ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down essence collection",
                description: "when collecting them from whispering roots",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownDreamOrbs = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownDreamOrbs ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down grimm lantern",
                description: "removes most flashes when activating it",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.ToneDownGrimmLanternActivation = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.ToneDownGrimmLanternActivation ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Quirrel Cutscene flashes",
                description: "For flashes occurring while revealing Monomon",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveQuirrelArchivesCutsceneFlashes = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveQuirrelArchivesCutsceneFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove miscellaneous effects",
                description: "Targets other flashes caught by a generic filter",
                values: new[] { "Yes", "No" },
                applySetting: index => { LessFlashingLights.Gs.RemoveGenericFlashingEffects = index == 0; },
                loadSetting: () => LessFlashingLights.Gs.RemoveGenericFlashingEffects ? 0 : 1),
        });
        return _menuRef.GetMenuScreen(modlistmenu);
    }
}