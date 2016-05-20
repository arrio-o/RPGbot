using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace RPGbot
{
    public sealed class GlobalConfig
    {
        public static string path { get; private set; } = "./config/global.json";
        private static GlobalConfig _instance = new GlobalConfig();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} is missing.");
            _instance = JsonConvert.DeserializeObject<GlobalConfig>(File.ReadAllText(path));
        }

        public static void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path)); 
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(_instance, Formatting.Indented));
        }

        public class DiscordSettings
        {
            [JsonProperty("token")]
            public string Token;
        }
        [JsonProperty("discord")]
        private DiscordSettings _discord = new DiscordSettings();
        public static DiscordSettings Discord => _instance._discord;

    }
}
