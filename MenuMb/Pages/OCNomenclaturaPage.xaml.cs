using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Логика взаимодействия для OCNomenclaturaPage.xaml
    /// </summary>
    public partial class OCNomenclaturaPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<NomenclaturaOCBase> nomenclaturaOCs;
        public OCNomenclaturaPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcNomenclaturaWindow window = new OcNomenclaturaWindow();
            if (window.ShowDialog() == true)
            {
                nomenclaturaOCs.Add(window.NewNomen);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOcNomen();
        }

        private async Task LoadOcNomen()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                nomenclaturaOCs = await client.GetFromJsonAsync<ObservableCollection<NomenclaturaOCBase>>("/oc_nomenclatura/list" + param);
                if (nomenclaturaOCs == null)
                {
                    StatusUpdater.UpdateStatusBar("Данных нет");
                }
                else
                {
                    OcNomenDataGrid.ItemsSource = nomenclaturaOCs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OcNomenDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OcNomenDataGrid.SelectedItem != null)
            {
                var selectedItem = (NomenclaturaOCFull)OcNomenDataGrid.SelectedItem;
                Console.WriteLine($"Двойной клик по: {selectedItem.Name}, Код: {selectedItem.Id}");

                // Открываем новое окно с деталями
                var detailsWindow = new NomenclaturaOCInfoWindow(selectedItem);
                detailsWindow.Show();
            }
        }
    }
}
