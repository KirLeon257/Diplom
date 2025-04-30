using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
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
using MenuMb.Classes;
using System.Windows.Shapes;
using MenuMb.Classes.OC;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<Department> departmentList;
        public DepartmentPage()
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
                    var SelectedDepartment = DepartmentDataGrid.SelectedItems.Cast<Department>().ToList();

                    if (SelectedDepartment != null)
                    {
                        string msg;
                        if (SelectedDepartment.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить подразделение \"{SelectedDepartment[0].Name}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?&ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedDepartment)
                            {
                                delparam += $"&DepId={item.Id}";
                            }

                            var response = await client.DeleteAsync("/department/delete" + delparam);
                            if (response.IsSuccessStatusCode)
                            {
                                var text = await response.Content.ReadAsStringAsync();
                                if (text == "OK")
                                {
                                    foreach (var item in SelectedDepartment)
                                    {
                                        departmentList.Remove(item);
                                    }

                                }
                            }
                        }

                    };
                    
                };
                menu.Items.Add(deleteMenuItem);
                DepartmentDataGrid.ContextMenu = menu;
            }
            await LoadDepartments();
        }




        private async Task LoadDepartments()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            departmentList = await client.GetFromJsonAsync<ObservableCollection<Department>>("/department/list" + param);
            if (departmentList == null || departmentList.Count == 0)
            {
                StatusUpdater.UpdateStatusBar("Данных нет");

            }
            DepartmentDataGrid.ItemsSource = departmentList;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win = new ResponseblePersonWindow();
            if (win.ShowDialog() == true)
            {
                //Department department = new Department();
                //department.Name = win.Name;
                await AddDepartment(win.DepName);


            }
        }

        private async Task AddDepartment(string DepName)
        {
            var dep = new
            {
                Name = DepName,
                ApiToken = LoginUser.User.ApiToken
            };
            string jsonString = JsonSerializer.Serialize(dep);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/department/add", content);
            if (response.IsSuccessStatusCode)
            {
                int Id = Convert.ToInt32(await response.Content.ReadAsStringAsync());
                Department department = new Department(Id, dep.Name);
                departmentList.Add(department);
            }
            else if (response.StatusCode == (System.Net.HttpStatusCode)500)
            {
                MessageBox.Show("Произошла ошибка на сервере");
            }
        }
    }
}
