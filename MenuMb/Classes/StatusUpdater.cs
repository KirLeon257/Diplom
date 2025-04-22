using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MenuMb.Classes
{
    public static class StatusUpdater
    {
        public static TextBlock StatusTextBlock;

        public static void UpdateStatusBar(string status)
        {
            StatusTextBlock.Text = status;
        }
    }
}
