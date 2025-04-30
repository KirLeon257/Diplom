using MenuMb.Classes.OC;
using MenuMb.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using MenuMb.Classes.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OCTypePage.xaml
    /// </summary>
    public partial class OCTypePage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<OCType> OCTypeList;
        public OCTypePage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                ContextMenu menu = new ContextMenu();
                var deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Удалить";
                deleteMenuItem.Click += async (o, e) =>
                {
                    var SelectedType = OCTypeDataGrid.SelectedItems.Cast<OCType>().ToList();
                    if (SelectedType != null)
                    {
                        string msg;
                        if (SelectedType.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить вид ОС \"{SelectedType[0].Name}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedType)
                            {
                                delparam += $"&Code={item.Code}";
                            }

                            var response = await client.DeleteAsync("/oc_type/delete" + delparam);
                            if (response.IsSuccessStatusCode)
                            {
                                var text = await response.Content.ReadAsStringAsync();
                                if (text == "OK")
                                {
                                    foreach (var item in SelectedType)
                                    {

                                        OCTypeList.Remove(item);
                                    }
                                }
                            }
                        }
                    }
                };
                menu.Items.Add(deleteMenuItem);
                OCTypeDataGrid.ContextMenu = menu;
            }

            await LoadOCType();
        }

        private async Task LoadOCType()
        {

            try
            {
                var param = "?ApiToken=" + LoginUser.User.ApiToken;
                OCTypeList = await client.GetFromJsonAsync<ObservableCollection<OCType>>("/oc_type/list" + param);
                if (OCTypeList != null && OCTypeList.Count != 0)
                {
                    OCTypeDataGrid.ItemsSource = OCTypeList;
                }
                else
                {
                    StatusUpdater.UpdateStatusBar("Данных нет");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить данные :(");
            }
        }

        private async void MenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var win = new OCTypeWindow();
            if (win.ShowDialog() == true)
            {

                var Oc_Type = new
                {
                    Code = win.Code,
                    Name = win.Name,
                    SPI = win.SPI,
                    ApiToken = LoginUser.User.ApiToken
                };

                string jsonString = JsonSerializer.Serialize(Oc_Type);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/oc_type/add", content);
                if (response.IsSuccessStatusCode)
                {
                    if (await response.Content.ReadAsStringAsync() == "OK")
                    {
                        OCType type = new OCType(Oc_Type.Code, Oc_Type.Name,Oc_Type.SPI);
                        OCTypeList.Add(type);
                    }
                }
                else if (response.StatusCode == (System.Net.HttpStatusCode)500)
                {
                    MessageBox.Show("Произошла ошибка на сервере");
                }
            }
        }
    }
}
