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
    /// Логика взаимодействия для OcRevDateSelectWindow.xaml
    /// </summary>
    public partial class OcRevDateSelectWindow : Window
    {
        public OcRevDateSelectWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbMonth.ItemsSource = new[]
                { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                    "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            cmbYear.ItemsSource = Enumerable.Range(DateTime.Now.Year - 50, 100).ToList();
            cmbYear.SelectedValue = DateTime.Now.Year;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Btn.IsEnabled = false;
            DateTime selectedDate = new DateTime(int.Parse(cmbYear.Text), cmbMonth.SelectedIndex + 1, 1);
            try
            {
                var data = new
                {
                    Date = selectedDate,
                    ApiToken = LoginUser.User.ApiToken
                };
                var respons = await HttpRequestHelper.PutAsync("/oc_revaluation/exec_rev", data);
                if (respons == "OK")
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
    }
}
