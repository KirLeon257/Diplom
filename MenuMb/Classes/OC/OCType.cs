using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class OCType
    {
        public int Code { get; set; }
        public string Name
        {
            get;
            set;
        }

        public int SPI
        {
            get;
            set;
        }
        public OCType() { }

        public OCType(int code, string name, int SPI)
        {
            Code = code;
            Name = name;
            this.SPI = SPI;
        }
    }
}
