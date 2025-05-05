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
using System.Text.Json;
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
    /// Логика взаимодействия для CoefficientsPage.xaml
    /// </summary>
    public partial class CoefficientsPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp)};
        public ObservableCollection<Coefficient> coefficients { get; set; }
        public ObservableCollection<OCType> OCTypesCodes { get; set; }
        public CoefficientsPage()
        {
            InitializeComponent();
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CoefWindow window = new CoefWindow();
            if (window.ShowDialog() == true)
            {
                this.NavigationService.Refresh();
            }
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
                    var SelectedCoef = CoefDataGrid.SelectedItems.Cast<Coefficient>().ToList();
                    if (SelectedCoef != null)
                    {
                        string msg;
                        if (SelectedCoef.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить Коэф \"{SelectedCoef[0].NameOC}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedCoef)
                            {
                                delparam += $"&CoefId={item.Id}";
                            }

                            var response = await client.DeleteAsync("/coefficient/delete" + delparam);
                            if (response.IsSuccessStatusCode)
                            {
                                var text = await response.Content.ReadAsStringAsync();
                                if (text == "OK")
                                {
                                    foreach (var item in SelectedCoef)
                                    {
                                        coefficients.Remove(item);
                                    }
                                }
                            }
                        }
                    }
                };
                var updateMenuItem = new MenuItem();
                updateMenuItem.Header = "Изменить";
                updateMenuItem.Click += async (o, e) =>
                {
                    var SelectedType = CoefDataGrid.SelectedItem as OCType;
                    if (SelectedType != null)
                    {
                        var win = new OCTypeWindow(SelectedType.Code, SelectedType.Name, SelectedType.SPI);
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
                            var response = await client.PostAsync("/oc_type/edit", content);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await response.Content.ReadAsStringAsync() == "OK")
                                {
                                    NavigationService.Refresh();
                                }
                            }
                        }
                    }
                };
                //menu.Items.Add(updateMenuItem);
                menu.Items.Add(deleteMenuItem);
                CoefDataGrid.ContextMenu = menu;
            }

            //Parallel.Invoke(LoadCoef/*,LoadOcCodes*/);
            await LoadCoef();
        }

        private async Task LoadCoef()
        {
            var param = "?ApiToken=" + LoginUser.User?.ApiToken;
            coefficients = await client.GetFromJsonAsync<ObservableCollection<Coefficient>>("/coefficient/list" + param);
            if (coefficients == null || coefficients.Count == 0)
            {
                StatusUpdater.UpdateStatusBar("Данных нет");
            }
            CoefDataGrid.ItemsSource = coefficients;
        }
    }
}
