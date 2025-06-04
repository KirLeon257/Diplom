using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcAmortisationPage.xaml
    /// </summary>
    public partial class OcAmortisationPage : Page
    {
        ObservableCollection<OcAmortisation> ocAmortisations;
        public OcAmortisationPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ocAmortisations = await HttpRequestHelper.GetAsync<ObservableCollection<OcAmortisation>>("/oc_amortisation/list", $"?ApiToken={LoginUser.User.ApiToken}");
                if (ocAmortisations != null)
                {
                    OcAmortisationDataGrid.ItemsSource = ocAmortisations;
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcAmortisationWindow window = new OcAmortisationWindow();
            if (window.ShowDialog()==true)
            {
                this.NavigationService.Refresh();
            }
        }

        private void OcAmortisationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = OcAmortisationDataGrid.SelectedItem as OcAmortisation;
            if (selectedItem != null)
            {
                OcAmortisationItemWindow window = new OcAmortisationItemWindow(selectedItem.NomenclatureId);
                window.Show();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var SearchList = ocAmortisations.Where(x => x.InventoryNumber.StartsWith(SearchTextBox.Text)).ToList();
            if (SearchList.Count > 0)
            {
                OcAmortisationDataGrid.ItemsSource = SearchList;
            }
        }
    }
}
