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
        public string Name { get; set; }
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
            Name = name;
            Inventory_number = inventory_number;
            EnterDate = enterDate;
            Basis = basis;
            Basis_number = basis_number;
            Basis_date = basis_date;
        }

        //public OCAddmition(string name, string inventory_Number, string oCType, string mOL, decimal initialCost, DateTime CreateDate, string amortisation_type, int equpment_code, int sPI)
        //{
        //}


    }
}
