using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using MenuMb.Classes;
using MenuMb.Classes.Users;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        HttpClient HttpClient;
        MainWindow newWindow;
        public LoginPage()
        {
            InitializeComponent();
            Env.Load();
            Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {

            ConnectionServerSetings.ServerIp = Env.GetString("SERVER_IP");
            HttpClient = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
           MessageBox.Show("Для регистрации обратитесь к администратору");
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false;
            try
            {
               await GetUserInfo();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключится");
            }
            finally { LoginBtn.IsEnabled = true; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ConnSettingPage());
        }

        async Task GetUserInfo()
        {
            string param = $"login={LoginTxt.Text}&pwd={PasswordHash.GetPasswordHash(PwdTxt.Password)}";
            var response = await HttpClient.GetAsync("/user/login?" + param);
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                if (user != null)
                {
                    // Сохранение ссылки на NavigationWindow
                    var navigationWindow = Application.Current.MainWindow as NavigationWindow;

                    // Скрываем NavigationWindow
                    if (navigationWindow != null)
                    {
                        navigationWindow.Hide();
                    }

                    // Сохраняем данные пользователя
                    LoginUser.User = user;

                    // Открываем MainWindow
                    var mainWindow = new MainWindow
                    {
                        Owner = navigationWindow // Устанавливаем владельца окна
                    };
                    ClearFields();
                    mainWindow.Show();
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
            else if (response.StatusCode == (System.Net.HttpStatusCode)404)
            {
                MessageBox.Show("Неправильный логин и/или пароль");
            }
            else if (response.StatusCode == ((System.Net.HttpStatusCode)500))
            {
                MessageBox.Show("Произошла ошибка на сервере");
            }
        }

        private void ClearFields()
        {
            LoginTxt.Clear();
            PwdTxt.Clear();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginUser.User = null;
            newWindow.Close();
            
        }
    }
}
