using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcWriteoffPage.xaml
    /// </summary>
    public partial class OcWriteoffPage : Page
    {
        ObservableCollection<OcWriteoff> ocWriteoffs;
        public OcWriteoffPage()
        {
            InitializeComponent();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcWriteoffChoiseWindow ocWriteoffChoiseWindow = new OcWriteoffChoiseWindow();
            if (ocWriteoffChoiseWindow.ShowDialog() == true)
            {
                try
                {
                    var data = new
                    {
                        NomenId = ocWriteoffChoiseWindow.NomenId,
                        Date = ocWriteoffChoiseWindow.Date.ToString("yyyy-MM-dd"),
                        ocWriteoffChoiseWindow.Basis,
                        LoginUser.User.ApiToken
                    };

                    var response = await HttpRequestHelper.PostAsync("/oc_writeoff/add", data);
                    if (response == "OK")
                    {
                        this.NavigationService.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Test");
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadInfo();
        }

        private async Task LoadInfo()
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}";
            try
            {
                ocWriteoffs = await HttpRequestHelper.GetAsync<ObservableCollection<OcWriteoff>>("/oc_writeoff/list", param);
                OcWriteOffDataGrid.ItemsSource = ocWriteoffs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AktProgressWindow window = new AktProgressWindow();
            try
            {
                var selected = OcWriteOffDataGrid.SelectedItem as OcWriteoff;
                window.PrintWriteOffAkt(selected.NomenId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}