using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class NomenclaturaOCBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inventory_Number { get; set; }
        public string OCType { get; set; }
        public string MOL { get; set; }
        public decimal InitialCost { get; set; }
        public DateTime EnterDate { get; set; }
        public NomenclaturaOCBase() { }
        public NomenclaturaOCBase(string name, string inventory_Number, string oCType, string mOL, decimal initialCost, DateTime enterDate)
        {
            Name = name;
            Inventory_Number = inventory_Number;
            OCType = oCType;
            MOL = mOL;
            InitialCost = initialCost;
            EnterDate = enterDate;
        }
    }

    public class NomenclaturaOCFull : NomenclaturaOCBase
    {
        public string Amortisation_type { get; set; }
        public string Equpment_code { get; set; }
        public NomenclaturaOCFull(string name, string inventory_Number, string oCType, string mOL, decimal initialCost, DateTime enterDate, string amortisation_type, string equpment_code) : base(name, inventory_Number, oCType, mOL, initialCost, enterDate)
        {
            Amortisation_type = amortisation_type;
            Equpment_code = equpment_code;
        }
    }
}
