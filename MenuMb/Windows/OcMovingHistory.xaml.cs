using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
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
using System.Windows.Shapes;

namespace MenuMb.Windows
{
    /// <summary>
    /// Логика взаимодействия для OcMovingHistory.xaml
    /// </summary>
    public partial class OcMovingHistory : Window
    {
        ObservableCollection<OcMovingItem> ocMovingItems;
        int NomenId;
        public OcMovingHistory(int selectedId)
        {
            InitializeComponent();
            NomenId = selectedId;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadHistory();
        }

        private async Task LoadHistory()
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}&NomenId={NomenId}";
            try
            {
                ocMovingItems = await HttpRequestHelper.GetAsync<ObservableCollection<OcMovingItem>>("/oc_moving/moving_history", param);
                if (ocMovingItems != null)
                {
                    OcMovingHistoryDataGrid.ItemsSource = ocMovingItems;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = OcMovingHistoryDataGrid.SelectedItem as OcMovingItem;
            if (selected != null)
            {
                AktProgressWindow peremAkt = new AktProgressWindow();
                peremAkt.PrintAktPerem(selected.MoveId);
            }
        }
    }
}
