using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace RPGbot.Modules.RPG
{
    //[JsonObject("Character")]
    public class Character
    {
        public ulong OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        [JsonProperty("Race")]
        public string RaceName { get; set; }
        public ulong Size { get; set; } = 100;
        public Attributes Attributes { get; set; }
        public Status Status { get; set; }
        public List<BodyPart> BodyParts { get; set; }
        public List<string> Skills { get; set; }
        public List<Item> Backpack { get; set; }
        
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
            Skills = new List<string>();
        }
    }
    
    public class Attributes
    {
        public int Strengh { get; set; } = 0;
        public int Vitality { get; set; } = 0;
        public int Dexterity { get; set; } = 0;
        public int Intelligence { get; set; } = 0;

        public Attributes()
        {

        }        
    }

    public class Status
    {
        [JsonProperty("HP")]
        public long Hitpoints { get; set; } = 0;
        [JsonProperty("MP")]
        public long Manapoints { get; set; } = 0;
        [JsonProperty("Condition")]
        public string Condition { get; set; } = "OK";

        public Status()
        {

        }
    }

    public class BodyPart:BodyPartTemplate
    {
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
