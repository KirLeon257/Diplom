using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    class OcRevaluation
    {
        public int oc_nomenclature_Id { get; set; }
        public string Name { get; set; }
        public string Inventory_number { get; set; }
        public decimal PereocenCost { get; set; }
        public DateTime LastDate { get; set; }
    }

    class OcRevaluationItem
    {
        public int RevId { get; set; }
        public string Name { get; set; }
        public Decimal Old_PereocenCost { get; set; }
        public Decimal PereocenSum { get; set; }
        public Decimal New_PereocenCost { get; set; }
        public DateTime OnDate { get; set; }
    }
}
