using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MenuMb
{
    enum Month
    {

    }

    /// <summary>
    /// Логика взаимодействия для CoefWindow.xaml
    /// </summary>
    public partial class CoefWindow : Window
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<OCType> ocTypes;
        internal ObservableCollection<NewCoef> coefficients = new ObservableCollection<NewCoef>();
        public CoefWindow()
        {
            InitializeComponent();
            MonthComboBox.ItemsSource = new[]
                { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year - 50, 100).ToList();
            YearComboBox.SelectedValue = DateTime.Now.Year;

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            await LoadOCType();
            NewCoefDataGrid.ItemsSource = coefficients;
            var comboBoxColumn = NewCoefDataGrid.Columns.OfType<DataGridComboBoxColumn>().FirstOrDefault();
        }

        private async Task LoadOCType()
        {
            try
            {
                var param = "?ApiToken=" + LoginUser.User.ApiToken;
                coefficients = await client.GetFromJsonAsync<ObservableCollection<NewCoef>>("/oc_type/list" + param);
                if (coefficients == null)
                {
                    StatusUpdater.UpdateStatusBar("Нет данных");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить данные :(");
            }
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //List<(int code, double value)> values = new List<(int code, double value)>();
            Dictionary<int, double> values = new Dictionary<int, double>();
            foreach (var item in NewCoefDataGrid.Items)
            {
                var row = item as NewCoef;
                if (row == null || row.NewValue==null)
                {
                    MessageBox.Show("В таблице есть незаполненые ячейки!");
                    return;
                }
                values.Add(row.Code, (double)row.NewValue);
            }

            var data = new
            {
                period = DateTime.Parse($"{YearComboBox.Text}-{MonthComboBox.SelectedIndex + 1}-{DateTime.Today.Day}"),
                coefs = values,
                LoginUser.User.ApiToken
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/coefficient/add", content);
            if (response.IsSuccessStatusCode)
            {
                if (await response.Content.ReadAsStringAsync() == "OK")
                {
                    DialogResult = true;
                }
            }
            else
            {
                DialogResult = null;
            }
        }
    }
}
