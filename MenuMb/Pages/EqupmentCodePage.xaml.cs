using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для EqupmentCodePage.xaml
    /// </summary>
    public partial class EqupmentCodePage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp)};
        ObservableCollection<EqupmentCode> codesList;
        public EqupmentCodePage()
        {
            InitializeComponent();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win = new EqupmentCodeWindow();
            if (win.ShowDialog() == true)
            {
                //Department department = new Department();
                //department.Name = win.Name;
                await AddDepartment(win.EqName,win.EqCode);
            }

        }

        private async Task AddDepartment(string Name,int Code)
        {
            var EqCode = new
            {
                Code = Code,
                Name = Name,
                ApiToken = LoginUser.User.ApiToken
            };
            string jsonString = JsonSerializer.Serialize(EqCode);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/equpmentcode/add", content);
            if (response.IsSuccessStatusCode)
            {
                EqupmentCode department = new EqupmentCode(EqCode.Code, EqCode.Name);
                codesList.Add(department);
            }
            else if (response.StatusCode == (System.Net.HttpStatusCode)500)
            {
                MessageBox.Show("Произошла ошибка на сервере");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEqupmentCodes();
        }

        async Task LoadEqupmentCodes()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            codesList = await client.GetFromJsonAsync<ObservableCollection<EqupmentCode>>("/equpmentcode/list" + param);
            if (codesList != null && codesList.Count != 0)
            {
                EqupmentCodeDataGrid.ItemsSource = codesList;
            }
            else
            {
                StatusUpdater.UpdateStatusBar("Данных нет");
            }
        }
    }
}
