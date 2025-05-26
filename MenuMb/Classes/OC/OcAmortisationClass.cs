using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    internal class OcAmortisation
    {
        public int AdmissionId { get; set; } // oc_admission.Id
        public int NomenclatureId { get; set; } // oc_nomenclature.Id
        public string Name { get; set; } // Name
        public string InventoryNumber { get; set; } // Inventory_number
        public string AmortisationType { get; set; } // Amortisation_type
        public decimal AmortisationSum{ get; set; } // Amortisation_type
        public double AmortisationYearNorm { get; set; } // AmortisationYearNorm
        public DateTime EnterDate { get; set; } // EnterDate
        public OcAmortisation() { }

        public OcAmortisation(int admissionId, int nomenclatureId, string name, string inventoryNumber, string amortisationType, double amortisationYearNorm, DateTime enterDate, decimal amortisationSum)
        {
            AdmissionId = admissionId;
            NomenclatureId = nomenclatureId;
            Name = name;
            InventoryNumber = inventoryNumber;
            AmortisationType = amortisationType;
            AmortisationYearNorm = amortisationYearNorm;
            EnterDate = enterDate;
            AmortisationSum = amortisationSum;
        }
    }
}
