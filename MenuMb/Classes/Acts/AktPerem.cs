using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.Acts
{
    internal class AktPerem
    {
        public int MoveId { get; set; }
        public DateTime Date { get; set; }
        public string MOL { get; set; }
        public string NomenName { get; set; }
        public string? Old_dep { get; set; }
        public string New_dep { get; set; }
        public decimal PereocenCost { get; set; }
        public decimal OstatichCoast { get; set; }
        public decimal InitialCost { get;set; }
    }
}
