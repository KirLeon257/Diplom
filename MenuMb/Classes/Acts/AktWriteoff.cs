using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.Acts
{
    internal class AktWriteoff
    {
        public AktWriteoff() { }
        public int WriteoffId { get; set; }
        public DateTime WriteOffDate { get;set; }
        public string NomenName { get; set; }
        public string InventoryNum { get; set; }
        public string WriteOffBasis { get; set; }
        public decimal PereocenCost { get; set; }
        public decimal AmortSum { get; set; }
        public decimal OstatochCost { get; set; }

    }
}
