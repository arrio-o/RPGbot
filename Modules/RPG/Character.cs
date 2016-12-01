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
        [JsonProperty("Skills")]
        public List<string> SkillsNames { get; set; }
        [JsonProperty("Backpack")]
        public List<Item> Backpack { get; set; }
        //public List<string> BackpackItemNames { get; set; }

        [JsonIgnore]
        //TODO: use size
        private ulong MaxHP { get { return (ulong)Math.Ceiling(Attributes.Vitality * Race.Modifiers.HP + 10); } }
        [JsonIgnore]
        private Race Race { get; set; }
        //[JsonIgnore]
        //private List<Item> Backpack { get; set; }
        [JsonIgnore]
        private List<Skill> Skills { get; set; }

        [JsonConstructor]
        private Character()
        {

        }

        public Character(User owner, string name, Race race)
        {
            //Owner = owner;
            Race = race;
            OwnerName = owner.Name;
            OwnerId = owner.Id;
            Name = name;
            RaceName = race.Name;
            Attributes = new Attributes();
            Attributes = race.BaseAttributes;
            Size = (uint)Math.Ceiling(race.Size * (1 + ((new Random()).NextDouble()/5 - 0.1)));

            Status = new Status();
            Status.Hitpoints = (int)Math.Ceiling(Attributes.Vitality * race.Modifiers.HP + 10);
            Status.Manapoints = (int)Math.Ceiling(Attributes.Intelligence * race.Modifiers.MP + 10);

            BodyParts = new List<BodyPart>();
            foreach (var bodyPart in race.BodyParts)
                BodyParts.Add(new BodyPart(bodyPart));
            //BackpackItemNames = new List<string>();
            Backpack = new List<Item>();
            SkillsNames = new List<string>();
            Skills = new List<Skill>();
        }

        public bool Dodge(Character attacker, Skill skill, bool isTargeted, ref string report)
        {
            if (report == null) report = "";
            var targetingPenalty = isTargeted ? 0.8 : 1;

            var attackRating = new Dice(attacker.Attributes.Dexterity, attacker.Attributes.Perception, attacker.Attributes.Intelligence);
            var dodgeRating = new Dice(Attributes.Dexterity, Attributes.Perception, Attributes.Intelligence);
            var hitchance = targetingPenalty * attackRating.Roll() * skill.BaseHitChance / dodgeRating.Roll();

            report += Environment.NewLine;
            report += ($"{attacker.Name} rolling {attackRating} to hit {this.Name} with {skill.Name}: {attackRating.LastRoll}; ");
            //report += Environment.NewLine;
            report += ($"{this.Name} rolling {dodgeRating} to dodge {skill.Name}: {dodgeRating.LastRoll}");
            report += Environment.NewLine;
            bool hit = hitchance * 100 >= (new Dice(1, 100, 0)).Roll();
            report += ($"Base hit chance for {skill.Name} is {skill.BaseHitChance}. {(targetingPenalty!=1?($"Targeting penalty: {targetingPenalty}. "):"")}Calculated chance to hit: {hitchance}. Result: **{(hit?"HIT":"DODGE")}**");
            
            return !hit;
        }

        public void ApplyDamage(Character attacker, Skill skill, BodyPart targetPart, ref string report)
        {
            if (report == null) report = "";
            //ChooseTargetBodyPart(targetPartType);

            //var armorAbsorb = targetPart.Holding.;
            Dice dice = new Dice(skill.BaseDamage);
            dice.Count += (byte)Math.Round(attacker.Attributes.Strengh * Scaling.GetScalingNumber(skill.Scaling.Strength));
            dice.Sides += (byte)Math.Round(attacker.Attributes.Dexterity * Scaling.GetScalingNumber(skill.Scaling.Dexterity));
            dice.Modifier += (byte)Math.Round(attacker.Attributes.Intelligence * Scaling.GetScalingNumber(skill.Scaling.Intelligence));

            report += Environment.NewLine;
            report += ($"Base damage of {skill.Name} is {skill.BaseDamage}. Damage scalings are [str:{skill.Scaling.Strength};dex:{skill.Scaling.Dexterity};int:{skill.Scaling.Intelligence}]");
            report += Environment.NewLine;
            report += ($"Rolling dice: {dice} => {dice.Roll()}. ");
            var damage = dice.LastRoll;
            report += ($"Armor absorbtion is 0. Armor mitigation is 0. Resist to {skill.DamageType} is 0");
            report += Environment.NewLine;
            if (damage < 0) damage = 0;
            report += ($"Applying **{damage}** damage.");
            //TODO: use Traits and Armor items to reduce damage
            //var armorAbsorb = targetPart.Holding.;
            //var armorMitigation = 

            this.Status.Hitpoints -= damage;

        }

        public BodyPart ChooseTargetBodyPart(BodyPartTemplate.SlotTypes targetPartType)
        {
            if (targetPartType != BodyPart.SlotTypes.undefined)
            {
                try
                {
                    var matchingParts = BodyParts.FindAll(s => s.SlotType == targetPartType && s.Status != "missing");//.Select((s, i) => i == 1);                    
                    int partIndex = (new Random()).Next(matchingParts.Count() - 1);
                    if (partIndex>=0)
                        return matchingParts[partIndex];
                }
                catch { }
            }
            return BodyParts?[(new Random()).Next(BodyParts.Count() - 1)];
        }
    }
    
    public class Attributes
    {
        public byte Strengh { get; set; } = 0;
        public byte Vitality { get; set; } = 0;
        public byte Dexterity { get; set; } = 0;
        public byte Intelligence { get; set; } = 0;
        public byte Perception { get; set; } = 0;

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
