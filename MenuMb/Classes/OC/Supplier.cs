using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    internal class Supplier
    {
        public Supplier() { }

        public Supplier(int id, string name, string yNP)
        {
            Id = id;
            Name = name;
            YNP = yNP;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string YNP { get; set; }
    }
}
