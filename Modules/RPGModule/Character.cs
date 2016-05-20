using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace RPGbot.Modules//.RPGModule
{
    public class Character
    {
        [JsonProperty("OwnerName")]
        public string OwnerName { get; set; }
        //public User Owner { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Race")]
        public string Race { get; set; }

        
        public class Attributes
        {
            public int Strengh { get; set; }
            public int Vitality { get; set; }
            public int Dexterity { get; set; }
            public int Intelligence { get; set; }
        }
        [JsonProperty("Attributes")]
        public Attributes attributes = new Attributes();

        public class Equipment
        {
            public Item head { get; set; }
            public Item body { get; set; }
        }
        [JsonProperty("Equipment")]
        public Equipment equipment = new Equipment();

        public List<Item> backpack { get; set; }
    }
}
