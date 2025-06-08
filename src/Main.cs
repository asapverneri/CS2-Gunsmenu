using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using CS2MenuManager.API;
using CS2MenuManager.API.Menu;
using CS2MenuManager.API.Class;
using static GunsMenu.Helpers;

namespace GunsMenu;

public class GunsMenu : BasePlugin, IPluginConfig<GunsMenuConfig>
{
    public override string ModuleName => "GunsMenu";
    public override string ModuleDescription => "Gunsmenu for CS2";
    public override string ModuleAuthor => "verneri";
    public override string ModuleVersion => "1.0";

    public GunsMenuConfig Config { get; set; } = new();

    public void OnConfigParsed(GunsMenuConfig config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        Logger.LogInformation($"Loaded (version {ModuleVersion})");


        if (!string.IsNullOrWhiteSpace(Config.Menutype))
        AddCommand($"css_guns", "", GunsCommand);

        if (!string.IsNullOrWhiteSpace(Config.Menutype))
        AddCommand($"css_secondary", "", SecondaryCommand);

        if (!string.IsNullOrWhiteSpace(Config.Menutype))
        AddCommand($"css_primary", "", PrimaryCommand);

        if (string.IsNullOrWhiteSpace(Config.Menutype))
        {
            Logger.LogError("Commands disabled, menutype configuration is incorrect. Check the configuration file.");
        }

    }
    public void GunsCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid)
            return;

        if (!player.PawnIsAlive)
        {
            player.PrintToChat($"{Localizer["not.alive"]}");
        }

        if (!string.IsNullOrEmpty(Config.FlagForCommands))
        {
            if (Config.FlagForCommands.StartsWith("#"))
            {
                if (!AdminManager.PlayerInGroup(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
            else if (Config.FlagForCommands.StartsWith("@"))
            {
                if (!AdminManager.PlayerHasPermissions(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
        }

        var menu = CreateMenu($"{Localizer["gunsmenu.title"]}");
        if (menu == null)
            return;

        foreach (var weapon in Helpers.Weapons.Where(w => !Config.WeaponBlacklist.Contains(w.Key, StringComparer.OrdinalIgnoreCase)))
        {
            string displayName = weapon.Key;
            menu.AddItem(displayName, (client, option) =>
            {
                Helpers.GiveSelectedWeapon(player, weapon.Key);
                player.PrintToChat($"{Localizer["gun.selected", displayName]}");
                MenuManager.CloseActiveMenu(player);
            });
        }

        menu.Display(player, 0);
    }
    public void SecondaryCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid)
            return;

        if (!player.PawnIsAlive)
        {
            player.PrintToChat($"{Localizer["not.alive"]}");
        }

        if (!string.IsNullOrEmpty(Config.FlagForCommands))
        {
            if (Config.FlagForCommands.StartsWith("#"))
            {
                if (!AdminManager.PlayerInGroup(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
            else if (Config.FlagForCommands.StartsWith("@"))
            {
                if (!AdminManager.PlayerHasPermissions(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
        }

        var menu = CreateMenu($"{Localizer["secondarymenu.title"]}");
        if (menu == null)
            return;

        foreach (var weapon in Helpers.Weapons.Where(w => w.Value.Type == WeaponType.Secondary && !Config.WeaponBlacklist.Any(bl => bl.Equals(w.Key, StringComparison.OrdinalIgnoreCase))))
        {
            string displayName = weapon.Key;
            string weaponKey = weapon.Key;

            menu.AddItem(displayName, (client, option) =>
            {
                Helpers.GiveSelectedWeapon(player, weaponKey);
                player.PrintToChat($"{Localizer["gun.selected", displayName]}");
                MenuManager.CloseActiveMenu(player);
            });
        }

        menu.Display(player, 0);
    }
    public void PrimaryCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid)
            return;

        if(!player.PawnIsAlive)
        {
            player.PrintToChat($"{Localizer["not.alive"]}");
        }

        if (!string.IsNullOrEmpty(Config.FlagForCommands))
        {
            if (Config.FlagForCommands.StartsWith("#"))
            {
                if (!AdminManager.PlayerInGroup(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
            else if (Config.FlagForCommands.StartsWith("@"))
            {
                if (!AdminManager.PlayerHasPermissions(player, Config.FlagForCommands))
                {
                    player.PrintToChat($"{Localizer["noaccess"]}");
                    return;
                }
            }
        }

        var menu = CreateMenu($"{Localizer["primarymenu.title"]}");
        if (menu == null)
            return;

        foreach (var weapon in Helpers.Weapons.Where(w => w.Value.Type == WeaponType.Primary && !Config.WeaponBlacklist.Any(bl => bl.Equals(w.Key, StringComparison.OrdinalIgnoreCase))))
        {
            string displayName = weapon.Key;
            string weaponKey = weapon.Key;

            menu.AddItem(displayName, (client, option) =>
            {
                Helpers.GiveSelectedWeapon(player, weaponKey);
                player.PrintToChat($"{Localizer["gun.selected", displayName]}");
                MenuManager.CloseActiveMenu(player);
            });
        }

        menu.Display(player, 0);
    }

    private BaseMenu? CreateMenu(string menuName)
    {
        return Config.Menutype switch
        {
            "ChatMenu" => new ChatMenu(menuName, this),
            "ConsoleMenu" => new ConsoleMenu(menuName, this),
            "CenterHtml" => new CenterHtmlMenu(menuName, this),
            "WasdMenu" => new WasdMenu(menuName, this),
            "ScreenMenu" => new ScreenMenu(menuName, this),
            _ => null
        };
    }
}