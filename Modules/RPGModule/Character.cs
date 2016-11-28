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
        [JsonIgnore]
        public User Owner { get { return _owner; } private set { _owner = value; } }
        private User _owner;
        [JsonProperty("OwnerName")]
        public string OwnerName { get { return _owner.Name; } }        
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Race")]
        public string Race { get; set; }
        
        //TODO: создание персонажа
        public Character(User owner, string name)
        {
            _owner = owner;
            Name = name;
        }        

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
