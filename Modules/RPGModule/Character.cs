using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace RPGbot.Modules//.RPGModule
{
    //[JsonObject("Character")]
    public class Character
    {
        //[JsonIgnore]
        //public User Owner { get; private set; }
        [JsonProperty("OwnerId")]
        public ulong OwnerId { get; set; }
        [JsonProperty("OwnerName")]
        public string OwnerName { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Race")]
        public string Race { get; set; } = "default";
        [JsonProperty("Attributes")]
        public Attributes Attributes { get; set; }
        [JsonProperty("Status")]
        public Status Status { get; set; }
        [JsonProperty("Backpack")]
        public List<Item> Backpack { get; set; }

        //TODO: создание персонажа
        [JsonConstructor]
        private Character()
        {

        }

        public Character(User owner, string name)
        {
            //Owner = owner;
            OwnerName = owner.Name;
            OwnerId = owner.Id;
            Name = name;
            Attributes = new Attributes();
            Status = new Status();
            Status.Hitpoints = Attributes.Vitality*10/*RaceModifier*/ + 10;
            Status.Manapoints = Attributes.Intelligence*10/*RaceModifier*/ + 10;
            Backpack = new List<Item>();
        }

        

        //[J("Attributes")]

        //[JsonProperty("Attributes")]
        //public Attributes attributes = new Attributes();

        //public class Equipment
        //{
        //    public Item head { get; set; }
        //    public Item body { get; set; }
        //}
        //[JsonProperty("Equipment")]
        //public Equipment equipment = new Equipment();

    }
    
    public class Attributes
    {
        [JsonProperty("Strengh")]
        public int Strengh { get; set; } = 0;
        [JsonProperty("Vitality")]
        public int Vitality { get; set; } = 0;
        [JsonProperty("Dexterity")]
        public int Dexterity { get; set; } = 0;
        [JsonProperty("Intelligence")]
        public int Intelligence { get; set; } = 0;

        public Attributes()
        {

        }
    }

    public class Status
    {
        [JsonProperty("HP")]
        public int Hitpoints { get; set; } = 0;
        [JsonProperty("MP")]
        public int Manapoints { get; set; } = 0;        

        public Status()
        {

        }
    }
}
