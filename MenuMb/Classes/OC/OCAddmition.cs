using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class OCAddmitionBase
    {
        public int AddId { get; set; }
        public string NomenName { get; set; }
        public string Inventory_number { get; set; }
        public DateTime EnterDate { get; set; }
        public string Basis { get; set; }
        public string Basis_number { get; set; }
        public DateTime Basis_date { get; set; }

        public OCAddmitionBase()
        {
        }

        public OCAddmitionBase(int addId, string name, string inventory_number, DateTime enterDate, string basis, string basis_number, DateTime basis_date)
        {
            AddId = addId;
            NomenName = name;
            Inventory_number = inventory_number;
            EnterDate = enterDate;
            Basis = basis;
            Basis_number = basis_number;
            Basis_date = basis_date;
        }
    }

    public class OCAddmitionFull : OCAddmitionBase
    {
        public string Department { get; set; }
        public DateTime AddmissionDate { get; set; }
        public DateTime TestDate { get; set; }
        public string IsTechnicalCorrect { get; set; }
        public string Dorabotka { get; set; }
        public decimal InitialCost { get; set; }
        public string SPI { get; set; }
        public string AmortisationType { get; set; }
        public decimal AmortisationYearNorm { get; set; }
        public string EquipmentCode { get; set; }

        public OCAddmitionFull() { }

        public OCAddmitionFull(string department, DateTime addmissionDate, DateTime testDate, string isTechnicalCorrect, string dorabotka, decimal initialCost, string sPI, string amortisationType, string equipmentCode)
        {
            Department = department;
            AddmissionDate = addmissionDate;
            TestDate = testDate;
            IsTechnicalCorrect = isTechnicalCorrect;
            Dorabotka = dorabotka;
            InitialCost = initialCost;
            SPI = sPI;
            AmortisationType = amortisationType;
            EquipmentCode = equipmentCode;
        }
    }
}
