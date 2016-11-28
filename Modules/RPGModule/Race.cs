using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules
{
    class Race
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public class Modifiers
        {
            public double Hitpoints;
            public double Energy;
        }
    }
}
