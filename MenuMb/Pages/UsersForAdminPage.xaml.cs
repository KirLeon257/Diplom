using System;
using System.Collections.Generic;
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
using MenuMb.Classes;
using MenuMb.Classes.Users;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersForAdminPage.xaml
    /// </summary>
    public partial class UsersForAdminPage : Page
    {

        public UsersForAdminPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegistrationPage());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) })
            {
                string param = $"?ApiToken={LoginUser.User.ApiToken}";
                var UserList = await client.GetFromJsonAsync<List<User>>("/user/list" + param);
                if (UserList != null)
                {
                    UsersDataGrid.ItemsSource = UserList;
                }
            }
        }

        private async void UserDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var SelectedUser = UsersDataGrid.SelectedItem as User;
            if (SelectedUser != null)
            {
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp)}) 
                {
                    var param = $"?UserId={SelectedUser.Id}&ApiToken={LoginUser.User.ApiToken}";
                    var responseMessage = await client.DeleteAsync("/user/delete"+param);
                    if (responseMessage.IsSuccessStatusCode) 
                    { 
                        var text = await responseMessage.Content.ReadAsStringAsync();
                        if (text == "OK") 
                        { 
                            this.NavigationService.Refresh();
                        }
                    }
                }
            }
        }
    }
}
