using MenuMb.Classes.OC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для OcAddmintionWindow.xaml
    /// </summary>
    public partial class OcAddmintionWindow : Window
    {
        ObservableCollection<FormCode> FormCodes = new ObservableCollection<FormCode>()
        {
            new FormCode() {Code = 0501030,Name="Форма по ОКУД"},
            new FormCode() {Code = 0,Name = "по ОКЮЛП"},
        };
        ObservableCollection<FormCode> RecipientCodes = new ObservableCollection<FormCode>()
        {
            new FormCode() {Code = 0,Name = "по ОКЮЛП"},
        };
        public OcAddmintionWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FormCodesDataGrid.ItemsSource = FormCodes;
            RecipientCodeDataGrid.ItemsSource = RecipientCodes;
        }
    }
}
