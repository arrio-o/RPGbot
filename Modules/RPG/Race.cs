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
            switch (strSlotType)
            {
                case "Head":
                    return SlotTypes.Head;
                case "Neck":
                    return SlotTypes.Neck;
                case "UpperBody":
                    return SlotTypes.UpperBody;
                case "LowerBody":
                    return SlotTypes.LowerBody;
                case "Leg":
                    return SlotTypes.Leg;
                case "Hand":
                    return SlotTypes.Hand;
                case "Wing":
                    return SlotTypes.Wing;
                case "Tail":
                    return SlotTypes.Tail;
                default:
                    return SlotTypes.undefined;
            }
        }
    }

}
