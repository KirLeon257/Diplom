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
    /// Логика взаимодействия для OcMovingPage.xaml
    /// </summary>
    public partial class OcMovingPage : Page
    {
        ObservableCollection<OcMoving> ocMovings;
        public OcMovingPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var param = "?ApiToken=" + LoginUser.User.ApiToken;
                ocMovings = await HttpRequestHelper.GetAsync<ObservableCollection<OcMoving>>("/oc_moving/list", param);
                if (ocMovings != null)
                {
                    OcMovingDataGrid.ItemsSource = ocMovings;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = OcMovingDataGrid.SelectedItem as OcMoving;
            if (selected != null)
            {
                OcMovingWindow ocMovingWindow = new OcMovingWindow(selected);
                if (ocMovingWindow.ShowDialog() == true)
                {
                    this.NavigationService.Refresh();
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void OcMovingDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedId = (OcMovingDataGrid.SelectedItem as OcMoving).NomenId;
            if (selectedId != null)
            {
                OcMovingHistory ocMovingHistory = new OcMovingHistory(selectedId);
                ocMovingHistory.Show();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var SearchList = ocMovings.Where(x => x.Inventory_number.StartsWith(SearchTextBox.Text)).ToList();
            if (SearchList.Count > 0) 
            {
                OcMovingDataGrid.ItemsSource = SearchList;
            } 
        }
    }
}
