using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    internal class Coefficient
    {
        public double Value { get; set; }
        public DateTime OnDate { get; set; }
        public int OC_Type_Code { get; set; }
        string oc_name;
        public string Name
        {
            get => oc_name;
            set => oc_name = value;
        }
        public Coefficient() { }
    }
}
