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
        //[JsonProperty("OwnerId")]
        public ulong OwnerId { get; set; }
        //[JsonProperty("OwnerName")]
        public string OwnerName { get; set; }
        //[JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Race")]
        public string RaceName { get; set; }
        //[JsonProperty("Size")]
        public uint Size { get; set; } = 100;
        //[JsonProperty("Attributes")]
        public Attributes Attributes { get; set; }
        //[JsonProperty("Status")]
        public Status Status { get; set; }
        //[JsonProperty("BodyParts")]
        public List<BodyPart> BodyParts { get; set; }
        //[JsonProperty("Backpack")]
        public List<Item> Backpack { get; set; }

        //TODO: создание персонажа
        [JsonConstructor]
        private Character()
        {

        }

        public Character(User owner, string name, Race race)
        {
            //Owner = owner;
            OwnerName = owner.Name;
            OwnerId = owner.Id;
            Name = name;
            RaceName = race.Name;
            Attributes = new Attributes();
            Attributes = race.BaseAttributes;
            Size = (uint)Math.Round(race.Size * (1 + ((new Random()).NextDouble()/5 - 0.1)));

            Status = new Status();
            Status.Hitpoints = (int)Math.Round(Attributes.Vitality * race.Modifiers.HP + 10);
            Status.Manapoints = (int)Math.Round(Attributes.Intelligence * race.Modifiers.MP + 10);

            BodyParts = new List<BodyPart>();
            foreach (var bodyPart in race.BodyParts)
                BodyParts.Add(new BodyPart(bodyPart));
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
        //[JsonProperty("Strengh")]
        public int Strengh { get; set; } = 0;
        //[JsonProperty("Vitality")]
        public int Vitality { get; set; } = 0;
        //[JsonProperty("Dexterity")]
        public int Dexterity { get; set; } = 0;
        //[JsonProperty("Intelligence")]
        public int Intelligence { get; set; } = 0;

        public Attributes()
        {

        }

        //public static Attributes operator = (Attributes a, BaseAttributes b)
        //{
        //    a.Dexterity = b.Dexterity;
        //}
        
    }

    public class Status
    {
        [JsonProperty("HP")]
        public int Hitpoints { get; set; } = 0;
        [JsonProperty("MP")]
        public int Manapoints { get; set; } = 0;
        [JsonProperty("Condition")]
        public string Condition { get; set; } = "OK";

        public Status()
        {

        }
    }

    public class BodyPart:BodyPartTemplate
    {
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string SlotType { get; set; }
        public Item Holding { get; set; }
        public string Status { get; set; }

        [JsonConstructor]
        public BodyPart()
        {

        }

        public BodyPart (BodyPartTemplate b)
        {
            this.Name = b.Name;
            this.Description = b.Description;
            this.SlotType = b.SlotType;
            this.Criticalness = b.Criticalness;
            this.Holding = null;
            this.Status = "OK";
        }
    }
}
