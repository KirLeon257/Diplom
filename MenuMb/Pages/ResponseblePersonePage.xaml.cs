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
using MenuMb.Classes;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResponseblePersonePage.xaml
    /// </summary>
    public partial class ResponseblePersonePage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<ResponseblePerson> personsList;
        public ResponseblePersonePage()
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
                    var SelectedPerson = ResponseblePersonsDataGrid.SelectedItem as ResponseblePerson;
                    if (MessageBox.Show($"Вы уверены что хотите удалить МОЛ {SelectedPerson.Name}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var param = $"?Code={SelectedPerson?.Code}&ApiToken={LoginUser.User.ApiToken}";
                        var response = await client.DeleteAsync("/responsibleperson/delete" + param);
                        if (response.IsSuccessStatusCode)
                        {
                            var text = await response.Content.ReadAsStringAsync();
                            if (text == "OK")
                            {
                                personsList.Remove(SelectedPerson);
                            }
                        }
                    }

                };
                menu.Items.Add(deleteMenuItem);
                ResponseblePersonsDataGrid.ContextMenu = menu;
            }

            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            personsList = await client.GetFromJsonAsync<ObservableCollection<ResponseblePerson>>("/responsibleperson/list" + param);
            if (personsList != null && personsList.Count != 0)
            {
                ResponseblePersonsDataGrid.ItemsSource = personsList;
            }
            else
            {
                StatusUpdater.UpdateStatusBar("Данных нет");
            }

        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win = new ResponseblePersoneWindow();
            if (win.ShowDialog() == true)
            {

                var person = new
                {
                    Code = win.Person.Code,
                    Name = win.Person.Name,
                    Surname = win.Person.Surname,
                    Patronymic = win.Person.Patronymic,
                    ApiToken = LoginUser.User.ApiToken
                };

                string jsonString = JsonSerializer.Serialize(person);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/responsibleperson/add", content);
                if (response.IsSuccessStatusCode)
                {
                    if (await response.Content.ReadAsStringAsync() == "OK")
                    {
                        personsList.Add(win.Person);
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
