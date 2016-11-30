using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGbot.Modules.RPG
{
    public class Race
    {
        public string Name { get; set; } = "";
        public List<string> Alias { get; set; } = new List<string>();
        public string Description { get; set; } = "-";
        public ulong Size { get; set; } = 100;
        public Attributes BaseAttributes { get; set; } = new Attributes();
        public Modifiers Modifiers { get; set; } = new Modifiers();
        public List<BodyPartTemplate> BodyParts { get; set; } = new List<BodyPartTemplate>();
        public List<Item> Backpack { get; set; } = new List<Item>();
    }

    public class Modifiers
    {
        public double HP { get; set; }
        public double MP { get; set; }
    }

    public class BodyPartTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SlotType { get; set; }
        public double Criticalness { get; set; }
    }
    
}
