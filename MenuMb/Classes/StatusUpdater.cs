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
        private static TextBlock _statusTextBlock;
        public static TextBlock TextBlock { set { _statusTextBlock = value; } }
        public static void UpdateStatusBar(string status)
        {
            _statusTextBlock.Text = status;
        }
    }
}
