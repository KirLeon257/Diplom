using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class NomenclaturaOCInfoClass
    {
        public NomenclaturaOCFull oc_info { get; set; }
        public PreciousMetals? metals { get; set; }
    }
}
