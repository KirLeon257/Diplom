using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class Coefficient
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime OnDate { get; set; }
        public int OC_Type_Code { get; set; }
        string oc_name;
        public string NameOC
        {
            get => oc_name;
            set => oc_name = value;
        }
        public Coefficient() { }

        public Coefficient(double value, DateTime onDate, int oC_Type_Code, string name)
        {
            Value = value;
            OnDate = onDate;
            OC_Type_Code = oC_Type_Code;
            NameOC = name;
        }
    }
}
