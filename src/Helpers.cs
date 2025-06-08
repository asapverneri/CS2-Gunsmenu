using CounterStrikeSharp.API.Core;

namespace GunsMenu
{
    internal class Helpers
    {
        public enum WeaponType
        {
            Primary,
            Secondary
        }
        public class Weapon
        {
            public string GiveName { get; set; }
            public WeaponType Type { get; set; }

            public int Slot { get; }

            public Weapon(string giveName, WeaponType type, int slot)
            {
                GiveName = giveName;
                Type = type;
                Slot = slot;
            }
        }

        public static readonly Dictionary<string, Weapon> Weapons = new()
        {

            { "M4A4", new Weapon("weapon_m4a1", WeaponType.Primary, 0) },
            { "M4A1", new Weapon("weapon_m4a1_silencer", WeaponType.Primary, 0) },
            { "FAMAS", new Weapon("weapon_famas", WeaponType.Primary, 0) },
            { "AUG", new Weapon("weapon_aug", WeaponType.Primary, 0) },
            { "AK47", new Weapon("weapon_ak47", WeaponType.Primary, 0) },
            { "GALIL", new Weapon("weapon_galilar", WeaponType.Primary, 0) },
            { "MP9", new Weapon("weapon_mp9", WeaponType.Primary, 0) },
            { "MP7", new Weapon("weapon_mp7", WeaponType.Primary, 0) },
            { "MP5SD", new Weapon("weapon_mp5sd", WeaponType.Primary, 0) },
            { "UMP45", new Weapon("weapon_ump45", WeaponType.Primary, 0) },
            { "P90", new Weapon("weapon_p90", WeaponType.Primary, 0) },
            { "BIZON", new Weapon("weapon_bizon", WeaponType.Primary, 0) },
            { "MAC10", new Weapon("weapon_mac10", WeaponType.Primary, 0) },
            { "XM1014", new Weapon("weapon_xm1014", WeaponType.Primary, 0) },
            { "MAG7", new Weapon("weapon_mag7", WeaponType.Primary, 0) },
            { "SAWEDOFF", new Weapon("weapon_sawedoff", WeaponType.Primary, 0) },
            { "NOVA", new Weapon("weapon_nova", WeaponType.Primary, 0) },
            { "M249", new Weapon("weapon_m249", WeaponType.Primary, 0) },
            { "NEGEV", new Weapon("weapon_negev", WeaponType.Primary, 0) },
            { "SG556", new Weapon("weapon_sg556", WeaponType.Primary, 0) },
            { "SCAR20", new Weapon("weapon_scar20", WeaponType.Primary, 0) },
            { "AWP", new Weapon("weapon_awp", WeaponType.Primary, 0) },
            { "SSG08", new Weapon("weapon_ssg08", WeaponType.Primary, 0) },
            { "G3SG1", new Weapon("weapon_g3sg1", WeaponType.Primary, 0) },


            { "USP", new Weapon("weapon_usp_silencer", WeaponType.Secondary, 1) },
            { "P2000", new Weapon("weapon_hkp2000", WeaponType.Secondary, 1) },
            { "GLOCK", new Weapon("weapon_glock", WeaponType.Secondary, 1) },
            { "DUAL", new Weapon("weapon_elite", WeaponType.Secondary, 1) },
            { "P250", new Weapon("weapon_p250", WeaponType.Secondary, 1) },
            { "FIVESEVEN", new Weapon("weapon_fiveseven", WeaponType.Secondary, 1) },
            { "CZ75A", new Weapon("weapon_cz75a", WeaponType.Secondary, 1) },
            { "TEC9", new Weapon("weapon_tec9", WeaponType.Secondary, 1) },
            { "REVOLVER", new Weapon("weapon_revolver", WeaponType.Secondary, 1) },
            { "DEAGLE", new Weapon("weapon_deagle", WeaponType.Secondary, 1) }

        };

        //Credits to slayer47
        public static void GiveSelectedWeapon(CCSPlayerController player, string weaponKey)
        {
            if (player == null || !player.IsValid || player.IsBot)
                return;

            if (!Weapons.TryGetValue(weaponKey, out var selectedWeapon))
                return;

            var weaponServices = player.PlayerPawn?.Value?.WeaponServices;
            if (weaponServices == null)
                return;

            var weaponInSameSlot = weaponServices.MyWeapons
                .FirstOrDefault(w =>
                    w?.IsValid == true &&
                    w.Value != null &&
                    w.Value.As<CCSWeaponBase>().VData?.GearSlot == (gear_slot_t)selectedWeapon.Slot);

            if (weaponInSameSlot?.IsValid == true)
            {
                DropWeapon(player, weaponInSameSlot.Value.DesignerName);
            }

            player.GiveNamedItem(selectedWeapon.GiveName);
        }
        //Credits to slayer47
        public static void DropWeapon(CCSPlayerController player, string weaponName, bool removeWeapon = true)
        {
            var weaponServices = player.PlayerPawn?.Value?.WeaponServices;
            if (weaponServices == null)
                return;

            var matchedWeapon = weaponServices.MyWeapons
                .FirstOrDefault(w => w?.IsValid == true && w.Value != null && w.Value.DesignerName == weaponName);

            if (matchedWeapon?.IsValid == true)
            {
                weaponServices.ActiveWeapon.Raw = matchedWeapon.Raw;

                var weaponEntity = weaponServices.ActiveWeapon.Value?.As<CBaseEntity>();
                if (weaponEntity == null || !weaponEntity.IsValid)
                    return;

                player.DropActiveWeapon();

                if (removeWeapon)
                {
                    weaponEntity.AddEntityIOEvent("Kill", weaponEntity, null, "", 0.1f);
                }
            }
        }

    }
}
