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
    /// Логика взаимодействия для SuppliersWindow.xaml
    /// </summary>
    public partial class SuppliersWindow : Window
    {
        public string SupplierName;
        public string YNP;
        public SuppliersWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SupplierNameTxt.Text) && !string.IsNullOrEmpty(YNPTxt.Text))
            {
                SupplierName = SupplierNameTxt.Text;
                YNP = YNPTxt.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите название подразделения!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
