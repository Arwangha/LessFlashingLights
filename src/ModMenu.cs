using Modding;
using Satchel.BetterMenus;

namespace NoFlashingLights;

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
                applySetting: index => { NoFlashingLights.Gs.RemoveDamageFlickering = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveDamageFlickering ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove spells flashes",
                description: "Toggle for the flash effects triggered when casting spells",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveSpellFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveSpellFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove spell pickups flashes",
                description: "Toggle for the flash effects triggered when collecting spells",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveSpellPickupsFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveSpellPickupsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove CDash flashes",
                description: "Toggles CDash usage flashes. Partially affects Nail Arts",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveCrystalDashFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveCrystalDashFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove focus flash",
                description: "Toggle for the flash effect triggered when focusing soul",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveHealFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveHealFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Soul Orb flashes",
                description: "Remove the flash effects of the soul orb on the HUD",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveSoulOrbFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveSoulOrbFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Lifeblood flashes",
                description: "Remove the flash effect when collecting lifeblood",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveLifebloodCollectionFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveLifebloodCollectionFlashes ? 0 : 1),
                        
            new HorizontalOption(
                name: "Tone down shade collection",
                description: "targets knight flashes on shade collection",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveShadeGetFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveShadeGetFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove misc. knight flashes",
                description: "Toggles miscellaneous effects with a generic filter. Includes nail arts",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveGenericHeroFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveGenericHeroFlashes ? 0 : 1),
            
            new TextPanel(name: "Enemy hit effects"),
                        
            new HorizontalOption(
                name: "Remove SporeShroom flashes",
                description: "Remove the effect on enemies hit by the spore cloud",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveSporeShroomFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveSporeShroomFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Defender's Crest flashes",
                description: "Remove the effect on enemies hit by the dung cloud",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveCrestFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveCrestFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Infected hit flashes",
                description: "Remove the flash effect on infected enemies",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveInfectedHitFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveInfectedHitFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove GrimmChild hit flashes",
                description: "Remove the flash effect on enemies hit by GrimmChild",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveGrimmChildHitFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveGrimmChildHitFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove white hit flashes",
                description: "Remove the flash effect on most other enemies",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveGenericEnemyHitFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveGenericEnemyHitFlashes ? 0 : 1),
            
            new TextPanel(name: "Fight-specific effects"),
            
            new HorizontalOption(
                name: "Tone down Soul Master/Tyrant",
                description: "notably teleportation, spells and fake death. Includes s. twister",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownMageLordFight = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownMageLordFight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down boss screams",
                description: "Removes the \"shockwave\" visual effect",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveBossScreams = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveBossScreams ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down boss deaths",
                description: "Targets the final explosion following the particles burst",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownDeathExplosions = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownDeathExplosions ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down warrior dreams",
                description: "Targets their teleportation and deaths",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownWarriorDreamsFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownWarriorDreamsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Radiance fights",
                description: "Targets attacks, teleportation and death. Absrad included",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownRadianceFightsFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownRadianceFightsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Uumuu's fight",
                description: "Targets Uumuu's explosions & stagger",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownUumuuFight = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownUumuuFight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down Hornet 2 fight",
                description: "Reduces the wind intensity",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownHornet2Fight = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownHornet2Fight ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down GrimmKin fights",
                description: "Mainly targets their teleportation flashes",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownGrimmKinFights = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownGrimmKinFights ? 0 : 1),
            
            new TextPanel(name: "Miscellaneous effects"),
            
            new HorizontalOption(
                name: "Tone down birthplace flashes",
                description: "Reduces dream nail impact and ledge grab flashes",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownBirthPlaceFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownBirthPlaceFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down explosions",
                description: "Includes bubbles, belflies, spores",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownExplosions = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownExplosions ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down pantheon victories",
                description: "Targets flashes on the completion & bindings cutscene",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemovePantheonCompletionFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemovePantheonCompletionFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Godhome Statues flashes",
                description: "includes spawn, dream lever switch, completion",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownGodhomeStatues = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownGodhomeStatues ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down essence collection",
                description: "when collecting them from whispering roots",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownDreamOrbs = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownDreamOrbs ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down grimm lantern",
                description: "removes most flashes when activating it",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownGrimmLanternActivation = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownGrimmLanternActivation ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove Quirrel Cutscene flashes",
                description: "For flashes occurring while revealing Monomon",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveQuirrelArchivesCutsceneFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveQuirrelArchivesCutsceneFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove miscellaneous effects",
                description: "Targets other flashes caught by a generic filter",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveGenericFlashingEffects = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveGenericFlashingEffects ? 0 : 1),
        });
        return _menuRef.GetMenuScreen(modlistmenu);
    }
}