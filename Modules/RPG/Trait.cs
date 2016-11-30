using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules.RPG
{
    public class Trait
    {
        public string Name { get; set; }
        public List<string> Alias { get; set; }
        public List<string> FlavorText { get; set; }
        public string Description { get; set; }
    }
}
