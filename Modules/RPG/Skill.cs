using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules.RPG
{
    public class Skill
    {
        public string Name { get; set; }
        public List<string> Alias { get; set; }
        public List<string> FlavorText { get; set; }
        public string Description { get; set; }
        public string BaseDamage { get; set; }
        public double BaseHitChance { get; set; }
        public int Manacost { get; set; }
        public uint Cooldown { get; set; }
        public string SkillType { get; set; }
        public string DamageType { get; set; }
        public List<string> ItemTypeRequired { get; set; }
        public List<string> RaceRequired { get; set; }
        public List<string> BodyPartRequired { get; set; }
        public Scaling Scaling { get; set; }
    }

    public class Scaling
    {
        public string Dexterity { get; set; }
        public string Strength { get; set; }
        public string Intelligence { get; set; }
    }
}
