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

        [JsonPropertyName("Blacklist")]
        public List<string> WeaponBlacklist { get; set; } = new();
    }
}
