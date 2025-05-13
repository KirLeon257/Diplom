using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    enum AmortisationType
    {
        Liner = 0
    }
    /// <summary>
    /// Логика взаимодействия для OcNomenclaturaWindow.xaml
    /// </summary>
    public partial class OcNomenclaturaWindow : Window
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<OCType> osTypes;
        ObservableCollection<ResponseblePerson> persons;
        internal NomenclaturaOCBase NewNomen;
        bool isAProgramm = false;
        public OcNomenclaturaWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOsType();
            await LoadMOL();
        }

        async Task LoadOsType()
        {
            var param = $"?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                osTypes = await client.GetFromJsonAsync<ObservableCollection<OCType>>("/oc_type/list" + param);
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
                persons = await client.GetFromJsonAsync<ObservableCollection<ResponseblePerson>>("/responsibleperson/list" + param);
                MOLBox.ItemsSource = persons;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OCTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isAProgramm)
            {
                return;
            }
            var selectedItem = OCTypeBox.SelectedItem as OCType;
            if (selectedItem != null)
            {
                UsfuleLifeBox.Text = selectedItem.SPI.ToString();
            }
        }

        private void OCTypeCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = false;
            }

        }

        private void OCTypeCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (OCTypeBox.SelectedIndex != -1 && !string.IsNullOrEmpty(OCTypeCode.Text))
            {
                var selected = OCTypeBox.SelectedItem as OCType;
                if (selected != null)
                {
                    isAProgramm = true;
                    OCTypeBox.Text = selected.Code.ToString();

                    isAProgramm = false;
                }
            }
        }

        private void OCTypeCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsTextAllowed(e.Text) && e.Text.Length <= 5)
            {
                var octype = osTypes.Where(x => x.Code.ToString().StartsWith(OCTypeCode.Text)).FirstOrDefault();
                if (octype != null)
                {
                    OCTypeBox.SelectedValue = octype;
                }
                else
                {
                    OCTypeBox.SelectedValue = null;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private bool IsTextAllowed(string text)
        {
            return text.All(char.IsDigit); // Разрешаем только цифры
        }

        private async void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            //Id,
            //Name,+
            //InitialCost,+
            //EnterDate,+
            //Remaining_term,----
            //Year_of_creating,+
            //Amortisation_type,+
            //Equpment_code,+
            //Inventory_number,+
            //responsible_persons_Code,+
            //OC_Type_Code+
            var Octype = OCTypeBox.SelectedItem as OCType;
            var data = new
            {
                oc_info = new
                {
                    Name = NameBox.Text,
                    InitialCost = InitialCostBox.Text,
                    EnterDate = EnterDateBox.SelectedDate.Value.ToString("yyyy-MM-dd"),
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

            //var json = JsonSerializer.Serialize(data);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //try
            //{
            //    var response = await client.PostAsync("/oc_nomenclatura/add", content);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        if (await response.Content.ReadAsStringAsync() == "OK")
            //        {
            //            NewNomen = new NomenclaturaOCBase(data.oc_info.Name, data.oc_info.Inventory, data.oc_info.OCTypeName, data.oc_info.MolName, decimal.Parse(data.oc_info.InitialCost),DateTime.Parse(data.oc_info.EnterDate));
            //            DialogResult = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //   MessageBox.Show(ex.Message);
            //}
            try
            {
                var responseText = await HttpRequestHelper.PostAsync("/oc_nomenclatura/add", data);
                if (!string.IsNullOrEmpty(responseText))
                {
                    NewNomen = new NomenclaturaOCBase(data.oc_info.Name, data.oc_info.Inventory, data.oc_info.OCTypeName, data.oc_info.MolName, decimal.Parse(data.oc_info.InitialCost), DateTime.Parse(data.oc_info.EnterDate));
                    NewNomen.Id = int.Parse(responseText);
                    DialogResult = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
