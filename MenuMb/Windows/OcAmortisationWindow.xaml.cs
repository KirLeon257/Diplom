using MenuMb.Classes;
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
using System.Windows.Shapes;

namespace MenuMb.Windows
{
    /// <summary>
    /// Логика взаимодействия для OcAmortisationWindow.xaml
    /// </summary>
    public partial class OcAmortisationWindow : Window
    {
        public OcAmortisationWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = new DateTime(int.Parse(YearComboBox.Text),MonthComboBox.SelectedIndex+1,1);
            try
            {
                var data = new
                {
                    Date = selectedDate,
                    ApiToken = LoginUser.User.ApiToken
                };
                var respons = await HttpRequestHelper.PutAsync("/oc_amortisation/execute_amortisation",data);
                if (respons == "OK")
                {
                    DialogResult = true;
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MonthComboBox.ItemsSource = new[]
                { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                    "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year - 50, 100).ToList();
            YearComboBox.SelectedValue = DateTime.Now.Year;
        }
    }
}
