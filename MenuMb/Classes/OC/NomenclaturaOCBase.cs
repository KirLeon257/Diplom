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
        public NomenclaturaOCBase() { }
        public NomenclaturaOCBase(string name, string inventory_Number, string oCType, string mOL, decimal initialCost)
        {
            Name = name;
            Inventory_Number = inventory_Number;
            OCType = oCType;
            MOL = mOL;
            InitialCost = initialCost;
        }
    }

    public class NomenclaturaOCFull : NomenclaturaOCBase
    {
        public string Amortisation_type { get; set; }
        public decimal AmortisationYearNorm { get; set; }
        public int Equpment_code { get; set; }
        public DateTime CreateDate { get; set; }
        public int SPI { get; set; }
        public int MolCode { get; set; }
        public int OCTypeCode { get; set; }
        public NomenclaturaOCFull() { }
        public NomenclaturaOCFull(string name, string inventory_Number, string oCType, string mOL, decimal initialCost, DateTime CreateDate, string amortisation_type, int equpment_code, int sPI) : base(name, inventory_Number, oCType, mOL, initialCost)
        {
            Amortisation_type = amortisation_type;
            Equpment_code = equpment_code;
            this.CreateDate = CreateDate;
            SPI = sPI;
        }
    }
}
