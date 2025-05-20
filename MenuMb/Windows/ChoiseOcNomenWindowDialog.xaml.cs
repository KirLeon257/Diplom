using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
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

namespace MenuMb.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChoiseOcNomenWindowDialog.xaml
    /// </summary>
    public partial class ChoiseOcNomenWindowDialog : Window
    {
        ObservableCollection<NomenclaturaOCBase> nomenclaturas;
        NomenclaturaOCBase SelectedItem; 

        public ChoiseOcNomenWindowDialog()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOc();
        }

        private async Task LoadOc()
        {
            var param = "?ApiToken=" + LoginUser.User?.ApiToken;
            try
            {
                nomenclaturas = await HttpRequestHelper.GetAsync<ObservableCollection<NomenclaturaOCBase>>("/oc_nomenclatura/list", param);
                if (nomenclaturas != null)
                {
                    OcDataGrid.ItemsSource = nomenclaturas;
                }
            }
            catch (Exception)
            {
                DialogResult = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(OcDataGrid.SelectedItem != null)
            {
                SelectedItem = (NomenclaturaOCBase)OcDataGrid.SelectedItem;
                DialogResult = true;
            }
        }
    }
}
