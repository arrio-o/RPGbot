using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules.RPG
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string[] AcceptableSlots { get; set; }
    }
}
