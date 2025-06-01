using MenuMb.Classes;
using MenuMb.Classes.Users;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MenuMb
{

    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private async void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidFields())
            {
                var userData = new NewUserInfo()
                {
                    Name = NameTxt.Text,
                    Surname = SurnameTxt.Text,
                    Patronymic = PatronymicTxt.Text,
                    DateOfBirth = DateOfBirth.SelectedDate.Value,
                    Role = RoleComboBox.Text,
                    Login = NewLoginTxt.Text,
                    Password = PasswordHash.GetPasswordHash(NewPwdTxt1.Password),
                    ApiTokenAdmin = LoginUser.User.ApiToken
                };

                StatusUpdater.UpdateStatusBar("Регистрация нового пользователя");
                var response = await HttpRequestHelper.PostAsync("/user/registration", userData);
                if (response == "OK")
                {
                    StatusUpdater.UpdateStatusBar("Пользователь зарегистрирован");
                    this.NavigationService.GoBack();
                }
            }

        }

        bool ValidFields()
        {
            if (string.IsNullOrWhiteSpace(NameTxt.Text))
            {
                MessageBox.Show("Поле 'Имя' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SurnameTxt.Text))
            {
                MessageBox.Show("Поле 'Фамилия' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (string.IsNullOrWhiteSpace(PatronymicTxt.Text))
            {
                MessageBox.Show("Поле 'Отчество' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (DateOfBirth.SelectedDate == null)
            {
                MessageBox.Show("Поле 'Дата рождения' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (string.IsNullOrWhiteSpace(NewLoginTxt.Text))
            {
                MessageBox.Show("Поле 'Новый логин' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewPwdTxt1.Password))
            {
                MessageBox.Show("Поле 'Новый пароль' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewPwdTxt2.Password))
            {
                MessageBox.Show("Поле 'Повторите пароль' не заполнено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (NewPwdTxt1.Password != NewPwdTxt2.Password)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (RoleComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Не выбрана роль пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        async Task<bool> RegisterNewUserAsync(string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync("/user/registration", content);
                if (response.IsSuccessStatusCode)
                {
                    var text = await response.Content.ReadAsStringAsync();
                    if (text == "OK")
                    {
                        return true;
                    }
                }
                StatusUpdater.UpdateStatusBar("Не удалось зарегистрировать пользователя :(");
                return false;
            }
            catch (Exception)
            {
                StatusUpdater.UpdateStatusBar("Не удалось зарегистрировать пользователя :(");
                return false;
            }
        }
    }
}
