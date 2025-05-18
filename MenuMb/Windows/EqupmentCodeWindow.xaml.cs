using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для EqupmentCodeWindow.xaml
    /// </summary>
    public partial class EqupmentCodeWindow : Window
    {
        public int EqCode;
        public string EqName;
        public EqupmentCodeWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EqCodeCodeTxt.Text) && !string.IsNullOrEmpty(EqCodeNameTxt.Text))
            {
                EqName = EqCodeNameTxt.Text;
                EqCode = Convert.ToInt32(EqCodeCodeTxt.Text);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите название подразделения!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
