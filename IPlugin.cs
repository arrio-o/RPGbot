using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules
{
    interface IPlugin
    {
        string Name { get; }
        void Do(ref string report);
    }
}
