using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    class NomenclaturaOC
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InventoryNumber { get; set; }
        public int ResponsiblePersonId { get; set; }
        public decimal InitialCost { get; set; }
        public decimal DepreciationAmount { get; set; }
        public DateTime DateOfEntry { get; set; }
        public int DepartmentId { get; set; }
        public string DepreciationType { get; set; }
        public string EquipmentCode { get; set; }
        public int EnterDate { get; set; }
        public int RemainingPeriod { get; set; }
        public int YearOfManufacture { get; set; }
        public int PreciousMetalsId { get; set; }
    }
}
