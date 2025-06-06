using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Windows;
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

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcRevaluationPage.xaml
    /// </summary>
    public partial class OcRevaluationPage : Page
    {
        ObservableCollection<OcRevaluation> ocRevaluations;
        public OcRevaluationPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRev();
        }

        private async Task LoadRev()
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}";
            try
            {
                ocRevaluations = await HttpRequestHelper.GetAsync<ObservableCollection<OcRevaluation>>("/oc_revaluation/list", param);
                if (ocRevaluations!=null)
                {
                    OcRevDataGrid.ItemsSource = ocRevaluations;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void OcRevDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = OcRevDataGrid.SelectedItem as OcRevaluation;
            if (selected != null)
            {
                OcRevItemWindow itemWindow = new OcRevItemWindow(selected.oc_nomenclature_Id);
                itemWindow.Show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcRevDateSelectWindow dateSelectWindow = new OcRevDateSelectWindow();
            if (dateSelectWindow.ShowDialog() == true)
            {
                this.NavigationService.Refresh();
            }
        }
    }
}
