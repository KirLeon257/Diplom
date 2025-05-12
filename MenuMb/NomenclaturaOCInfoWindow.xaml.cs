using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
    /// Логика взаимодействия для NomenclaturaOCInfoWindow.xaml
    /// </summary>
    public partial class NomenclaturaOCInfoWindow : Window
    {
        
        public NomenclaturaOCInfoWindow() { }
        ObservableCollection<OCType> osTypes;
        ObservableCollection<ResponseblePerson> persons;
        NomenclaturaOCInfoClass NomenclaturaOCInfo;
        public NomenclaturaOCInfoWindow(NomenclaturaOCBase nomenclaturaOC)
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadInfo();
            await LoadOsType();
            await LoadMOL();
        }

        private async Task LoadInfo()
        {
            var param = $"?ApiToken=" + LoginUser.User.ApiToken;
            NomenclaturaOCInfo = await HttpRequestHelper.GetAsync<NomenclaturaOCInfoClass>("/oc_nomenclatura/item_info", param);

        }


        async Task LoadOsType()
        {
            var param = $"?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                //osTypes = await client.GetFromJsonAsync<ObservableCollection<OCType>>("/oc_type/list" + param);
                osTypes = await HttpRequestHelper.GetAsync<ObservableCollection<OCType>>("/oc_type/list", param);
                OCTypeBox.ItemsSource = osTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        async Task LoadMOL()
        {
            var param = $"?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                //persons = await client.GetFromJsonAsync<ObservableCollection<ResponseblePerson>>("/responsibleperson/list" + param);
                persons = await HttpRequestHelper.GetAsync<ObservableCollection<ResponseblePerson>>("/responsibleperson/list", param);
                MOLBox.ItemsSource = persons;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
