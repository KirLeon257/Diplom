using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MenuMb
{


    /// <summary>
    /// Логика взаимодействия для CoefWindow.xaml
    /// </summary>
    public partial class CoefWindow : Window
    {
        string[] months = { "Декабрь", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        ObservableCollection<OCType> ocTypes;
        internal ObservableCollection<NewCoef> coefficients;
        public CoefWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year - 50, 100).ToList();
            YearComboBox.SelectedValue = DateTime.Now.Year;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOCType();
            LoadCoefsMonth();
        }

        private async Task LoadOCType()
        {
            try
            {

                var param = "?ApiToken=" + LoginUser.User.ApiToken;
                ocTypes = await HttpRequestHelper.GetAsync<ObservableCollection<OCType>>("/oc_type/list", param);
                if (ocTypes == null)
                {
                    StatusUpdater.UpdateStatusBar("Нет данных");
                }

                OcTypeComboBox.ItemsSource = ocTypes;
                OcTypeComboBox.DisplayMemberPath = "Name";

            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить данные :(");
            }
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = new
            {
                coefficients,
                BaseDate = BaseDate.SelectedDate.Value,
                OcTypeCode = (OcTypeComboBox.SelectedItem as OCType).Code,
                LoginUser.User.ApiToken
            };
            Btn.IsEnabled = false;
            try
            {
                var response = await HttpRequestHelper.PostAsync("/coefficient/add", data);
                if(response == "OK")
                {
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Btn.IsEnabled = true;
            }

        }

        void LoadCoefsMonth()
        {
            coefficients = new ObservableCollection<NewCoef>();
            DateTime startDateTime = new DateTime((int)YearComboBox.SelectedItem - 1, 12, 1);
            for (var i = 0; i < months.Length; i++)
            {
                if (i == 0)
                {
                    coefficients.Add(new NewCoef(startDateTime, 0));
                    startDateTime = startDateTime.AddMonths(1);
                    continue;
                }
                coefficients.Add(new NewCoef(startDateTime, 0));
                startDateTime = startDateTime.AddMonths(1);
            }
            NewCoefDataGrid.ItemsSource = coefficients;
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCoefsMonth();
        }
    }
}
