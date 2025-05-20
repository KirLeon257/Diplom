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
    /// Логика взаимодействия для SuppliersPage.xaml
    /// </summary>
    public partial class SuppliersPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<Supplier> suppliersList;
        public SuppliersPage()
        {
            InitializeComponent();
            Loaded += SuppliersPage_Loaded;
        }

        private async void SuppliersPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                ContextMenu menu = new ContextMenu();
                var deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Удалить";
                deleteMenuItem.Click += async (o, e) =>
                {
                    var SelectedSupplier = SuppliersDataGrid.SelectedItems.Cast<Supplier>().ToList();

                    string msg;
                    if (SelectedSupplier.Count == 1)
                    {
                        msg = $"Вы уверены что хотите удалить вид ОС \"{SelectedSupplier[0].Name}\"?";
                    }
                    else
                    {
                        msg = $"Вы уверены что хотите удалить выбранные элементы?";
                    }

                    if (MessageBox.Show(msg,"Предупреждение",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                    {
                        var delparam = $"?ApiToken={LoginUser.User.ApiToken}";

                        foreach (var item in SelectedSupplier) 
                        {
                            delparam += $"&SupId={item.Id}";
                            
                        }


                        var response = await client.DeleteAsync("/supplier/delete" + delparam);
                        if (response.IsSuccessStatusCode)
                        {
                            var text = await response.Content.ReadAsStringAsync();
                            if (text == "OK")
                            {
                                foreach (var item in SelectedSupplier)
                                {
                                    suppliersList.Remove(item);

                                }
                            }
                        }
                    }
                    
                };
                menu.Items.Add(deleteMenuItem);
                SuppliersDataGrid.ContextMenu = menu;
            }

            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            suppliersList = await client.GetFromJsonAsync<ObservableCollection<Supplier>>("/supplier/list" + param);
            if (suppliersList != null && suppliersList.Count != 0)
            {
                SuppliersDataGrid.ItemsSource = suppliersList;
            }
            else
            {
                StatusUpdater.UpdateStatusBar("Данных нет");
            }
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win = new SuppliersWindow();
            if (win.ShowDialog() == true)
            {
                var sup = new
                {
                    Name = win.SupplierName,
                    YNP = win.YNP,
                    ApiToken = LoginUser.User.ApiToken
                };

                string jsonString = JsonSerializer.Serialize(sup);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/supplier/add", content);
                if (response.IsSuccessStatusCode)
                {
                    int Id = Convert.ToInt32(await response.Content.ReadAsStringAsync());
                    Supplier supplier = new Supplier(Id, sup.Name,sup.YNP);
                    suppliersList.Add(supplier);
                }
                else if (response.StatusCode == (System.Net.HttpStatusCode)500)
                {
                    MessageBox.Show("Произошла ошибка на сервере");
                }
            }
        }
    }
}
