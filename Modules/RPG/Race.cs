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
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SlotTypes SlotType { get; set; }
        public double Criticalness { get; set; }

        //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum SlotTypes
        {
            undefined, Head, Neck,
            //[JsonProperty("Upper body")]
            UpperBody,
            //[JsonProperty("Lower body")]
            LowerBody,
            Leg, Hand, Wing, Tail
        }
        public static SlotTypes GetSlotType(string strSlotType)
        {
            switch (strSlotType.ToLowerInvariant())
            {
                case "head":
                    return SlotTypes.Head;
                case "neck":
                    return SlotTypes.Neck;
                case "upperbody":
                    return SlotTypes.UpperBody;
                case "lowerbody":
                    return SlotTypes.LowerBody;
                case "leg":
                    return SlotTypes.Leg;
                case "hand":
                    return SlotTypes.Hand;
                case "wing":
                    return SlotTypes.Wing;
                case "tail":
                    return SlotTypes.Tail;
                default:
                    return SlotTypes.undefined;
            }
        }
    }

}
