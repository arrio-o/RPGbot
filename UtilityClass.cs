using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGbot
{
    public static class UtilityClass
    {
        public static void Serialize<T>(T c, string filepath)
        {
            string res = JsonConvert.SerializeObject(c, Formatting.Indented);
            //Directory.CreateDirectory(filepath);
            File.WriteAllText(filepath, res);
        }
        public static T Deserialize<T>(string filepath)
        {
            string json = File.ReadAllText(filepath);
            var res = JsonConvert.DeserializeObject<T>(json);
            return res;
        }
    }
}
