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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для NomenclaturaOCInfoWindow.xaml
    /// </summary>
    public partial class NomenclaturaOCInfoWindow : Window
    {

        public NomenclaturaOCInfoWindow() { }
        List<OCType> ocTypes;
        List<ResponseblePerson> persons;
        NomenclaturaOCInfoClass NomenclaturaOCInfo;
        NomenclaturaOCBase baseNomen;
        NavigationService navigationService;
        public NomenclaturaOCInfoWindow(NomenclaturaOCBase nomenclaturaOC, NavigationService navigationService)
        {
            InitializeComponent();
            baseNomen = nomenclaturaOC;
            this.navigationService = navigationService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginUser.User.Role != RoleEnum.Admin.ToString())
            {
                CreateBtn.IsEnabled = false;
            }
            await LoadOsType();
            await LoadMOL();
            await LoadInfo(baseNomen.Id);
        }

        private async Task LoadInfo(int OcId)
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}&Id={OcId}";
            NomenclaturaOCInfo = await HttpRequestHelper.GetAsync<NomenclaturaOCInfoClass>("/oc_nomenclatura/item_info", param);
            if (NomenclaturaOCInfo.metals == null)
            {
                NomenclaturaOCInfo.metals = new PreciousMetals();
            }

            var mol = persons.FirstOrDefault(p => p.Code == NomenclaturaOCInfo.oc_info.MolCode);
            if (mol != null)
            {
                MOLBox.SelectedItem = mol;
            }
            var type = ocTypes.FirstOrDefault(p => p.Code == NomenclaturaOCInfo.oc_info.OCTypeCode);
            if (type != null)
            {
                OCTypeBox.SelectedItem = type;
            }

            if (NomenclaturaOCInfo.oc_info.Amortisation_type == "Liner")
            {
                AmortisationBox.Text = "Линейная";
            }

            DataContext = NomenclaturaOCInfo;
        }

        async Task LoadOsType()
        {
            var param = $"?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                ocTypes = await HttpRequestHelper.GetAsync<List<OCType>>("/oc_type/list", param);
                OCTypeBox.ItemsSource = ocTypes;
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
                persons = await HttpRequestHelper.GetAsync<List<ResponseblePerson>>("/responsibleperson/list", param);
                MOLBox.ItemsSource = persons;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = new
            {
                oc_info = new
                {
                    baseNomen.Id,
                    Name = NameBox.Text,
                    InitialCost = InitialCostBox.Text,
                    CreateDate = CreateDateBox.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    Amortisation = ((AmortisationType)AmortisationBox.SelectedIndex).ToString(),
                    EqupmentCode = EqupmentCodeBox.Text,
                    Inventory = InventoryBox.Text,
                    MolCode = ((ResponseblePerson)MOLBox.SelectedItem).Code,
                    MolName = ((ResponseblePerson)MOLBox.SelectedItem).Name,
                    OCTypeCode = ((OCType)OCTypeBox.SelectedItem).Code,
                    OCTypeName = ((OCType)OCTypeBox.SelectedItem).Name
                },

                metals = new
                {
                    Gold = GoldText.Text,
                    Silver = SilverText.Text,
                    Paladium = PaladiumText.Text,
                    Platinum = PlatinumText.Text,
                    Indium = IndiumText.Text,
                    DiamondCarat = DiamondCountText.Text,
                    DiamondCount = DiamondCountText.Text,
                    Other = OtherText.Text
                },
                ApiToken = LoginUser.User.ApiToken
            };

            try
            {
                var response = await HttpRequestHelper.PostAsync("/oc_nomenclatura/edit", data);
                if (response != null && response == "OK")
                {
                    this.navigationService.Refresh();
                    MessageBox.Show("Данные обновлены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
