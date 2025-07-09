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
                description: "Toggle for the flash effects triggered when using CDash",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveCrystalDashFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveCrystalDashFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove nail arts flashes",
                description: "Toggle for the flash effects triggered when using nail arts",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveNailArtsFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveNailArtsFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove focus flash",
                description: "Toggle for the flash effect triggered when focusing soul",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveHealFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveHealFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove misc. knight flashes",
                description: "Toggle for miscellaneous flash effects caught by a generic filter",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemoveGenericHeroFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemoveGenericHeroFlashes ? 0 : 1),
            
            new TextPanel(name: "Boss-specific effects"),
            
            new HorizontalOption(
                name: "Tone down Soul Master/Tyrant",
                description: "notably teleportation, spells and fake death. Includes Soul Tyrant",
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
            
            new TextPanel(name: "Miscellaneous effects"),
            
            new HorizontalOption(
                name: "Tone down birthplace flashes",
                description: "Reduces dream nail impact and ledge grab flashes",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownBirthPlaceFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownBirthPlaceFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down jellyfish explosions",
                description: "Includes explosive bubbles",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownJellyfishExplosions = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownJellyfishExplosions ? 0 : 1),
            
            new HorizontalOption(
                name: "Tone down pantheon victories",
                description: "Targets flashes on the completion & bindings cutscene",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.RemovePantheonCompletionFlashes = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.RemovePantheonCompletionFlashes ? 0 : 1),
            
            new HorizontalOption(
                name: "Remove dream lever anims",
                description: "Targets the flashes when hitting a dream lever in hall of gods",
                values: new[] { "Yes", "No" },
                applySetting: index => { NoFlashingLights.Gs.ToneDownGodhomeDreamStatues = index == 0; },
                loadSetting: () => NoFlashingLights.Gs.ToneDownGodhomeDreamStatues ? 0 : 1),
            
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