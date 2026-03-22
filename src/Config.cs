using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace GunsMenu
{
    public class GunsMenuConfig : BasePluginConfig
    {
        [JsonPropertyName("Menutype")]
        public string Menutype { get; set; } = "ScreenMenu";

        [JsonPropertyName("PermissionForCommands")]
        public string FlagForCommands { get; set; } = "";

        [JsonPropertyName("WeaponCommands")]
        public bool WeaponCommands { get; set; } = true;

        [JsonPropertyName("Blacklist")]
        public List<string> WeaponBlacklist { get; set; } = new();
    }
}
