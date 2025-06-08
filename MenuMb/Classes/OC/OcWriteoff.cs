using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public  class OcWriteoff
    {
        public OcWriteoff() { }
        public int WriteOffId { get; set; }
        public int NomenId { get; set; }
        public string NomenName { get; set; }
        public string Inventory_number { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime LeaveDate { get; set; }
    }

    public class OcNomenToWrite
    {
        public OcNomenToWrite() { }
        public int NomenId { get; set; }
        public string NomenName { get; set; }
        public string Inventory_number { get; set; }
        public DateTime EnterDate { get; set; }
        public string MOL { get; set; }
        public decimal PereocenCost { get; set; }
    }

}
