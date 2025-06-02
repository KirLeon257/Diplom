using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class OcMoving
    {
        public OcMoving(int nomenId, int departmentId, string nomenName, string inventory_number, string current_Department, DateTime lastMoveDate)
        {
            NomenId = nomenId;
            DepartmentId = departmentId;
            NomenName = nomenName;
            Inventory_number = inventory_number;
            Current_Department = current_Department;
            LastMoveDate = lastMoveDate;
        }

        public int NomenId { get; set; }
        public int DepartmentId { get; set; }
        public string NomenName { get; set; }
        public string Inventory_number { get; set; }
        public string Current_Department { get; set; }
        public DateTime LastMoveDate { get; set; }
    }

    public class OcMovingItem
    {
        public int NomenId { get; set; }
        public int MoveId { get; set; }
        public string NomenName { get; set; }
        public string Inventory_number { get; set; }
        public string Old_dep { get; set; }
        public string New_dep { get; set; }
        public DateTime MoveDate { get; set; }
    }
}
