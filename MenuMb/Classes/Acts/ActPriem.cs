using MenuMb.Classes.OC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.Acts
{
    internal class ActPriem
    {
        public OCAddmitionFull AddInfo { get; set; }
        public List<OcCharacteristic> xaract { get; set; }
        public Supplier? supplier { get; set; }
        public Supplier excepter { get; set; }
    }
}
