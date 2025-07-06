using Modding;
using Satchel.BetterMenus;

namespace NoFlashingLights;

public static class ModMenu
{
    private static Menu? _menuRef;

    public static MenuScreen CreateModMenu(MenuScreen modlistmenu, ModToggleDelegates? toggleDelegates)
    {
        _menuRef ??= new Menu("Quiet Hallownest's Options", new Element[]
        {
        });
        return _menuRef.GetMenuScreen(modlistmenu);
    }
}