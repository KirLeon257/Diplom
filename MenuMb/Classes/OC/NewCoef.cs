using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    internal class NewCoef
    {
        public NewCoef(DateTime month, double? newValue)
        {
            Month = month;
            NewValue = newValue;
        }

        public DateTime Month { get; set; }
        public double? NewValue { get; set; }
    }

    internal class NewCoefToSend
    {
        
    }
}
