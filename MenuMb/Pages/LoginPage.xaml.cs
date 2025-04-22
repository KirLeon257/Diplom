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
using DotNetEnv;
using MenuMb.Classes.Users;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        HttpClient HttpClient;
        RegistrationPage registrationPage;
        public LoginPage()
        {
            InitializeComponent();
            Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {

            Env.Load();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegistrationPage());
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false;
            try
            {
                string address = Env.GetString("SERVER_IP");
                HttpClient = new HttpClient() { BaseAddress = new Uri(address) };
                string param = $"login={LoginTxt.Text}&pwd={PwdTxt.Password}";
                var response = await HttpClient.GetAsync("/user/login?" + param);
                if (response.IsSuccessStatusCode)
                {
                     var user = await response.Content.ReadFromJsonAsync<User>();
                    if (user != null)
                    {
                        Application.Current.MainWindow.Hide();

                        LoginUser.User = user;
                        var newWindow = new MainWindow();
                        newWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось войти :(");
                    }
                }
                else if (response.StatusCode == (System.Net.HttpStatusCode)422)
                {
                    MessageBox.Show("Вы ввели некоректные данные");
                }
                else if (response.StatusCode == ((System.Net.HttpStatusCode)500))
                {
                    MessageBox.Show("Произошла ошибка на сервере");
                }
                else
                {
                    MessageBox.Show("Не удалось подключится");
                }
            }
            finally { LoginBtn.IsEnabled = true; }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ConnSettingPage());
        }
    }
}
