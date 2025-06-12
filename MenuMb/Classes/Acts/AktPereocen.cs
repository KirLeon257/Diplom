using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Classes.Acts
{
    class AktPereocen
    {
        public AktPereocen() { }
        public string Inventory_number { get; set; }
        public string Name { get; set; }
        public DateTime EnterDate { get; set; }
        public int OC_Type_Code { get; set; }
        public decimal Old_PereocenCost { get; set; }
        public decimal Value { get; set; }
        public decimal New_PereocenCost { get; set; }
        public decimal YdelVecAmort { get; set; }
        public decimal NewAmortSum { get; set; }
        public decimal NewOstatichCoast { get; set; }

    }
}
