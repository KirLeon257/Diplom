using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class OcCharacteristic
    {
        public OcCharacteristic()
        {
        }

        public OcCharacteristic(string nameCharacteristic, int count)
        {
            NameCharacteristic = nameCharacteristic;
            Count = count;
        }

        public string NameCharacteristic { get; set; }
        public int Count {  get; set; }
    }
}
