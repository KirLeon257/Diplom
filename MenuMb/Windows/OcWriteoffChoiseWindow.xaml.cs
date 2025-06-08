using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для OcWriteoffChoiseWindow.xaml
    /// </summary>
    public partial class OcWriteoffChoiseWindow : Window
    {
        ObservableCollection<OcNomenToWrite> ocNomenToWrites;
        public int NomenId;
        public DateTime Date;

        public string Basis { get; private set; }

        public OcWriteoffChoiseWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadNomensToWrite();
        }

        private async Task LoadNomensToWrite()
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}";
            try
            {
                ocNomenToWrites = await HttpRequestHelper.GetAsync<ObservableCollection<OcNomenToWrite>>("/oc_writeoff/list_to_writeoff", param);
                if(ocNomenToWrites != null)
                {
                    OcNomenToWriteOffDataGrid.ItemsSource = ocNomenToWrites;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var serach = ocNomenToWrites.Where(x => x.Inventory_number.StartsWith(SearchTextBox.Text)).ToList();
            if (serach != null)
            {
                OcNomenToWriteOffDataGrid.ItemsSource = serach;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = OcNomenToWriteOffDataGrid.SelectedItem as OcNomenToWrite;
            if (selected != null && WriteOffDate.SelectedDate!=null)
            {
                NomenId = selected.NomenId;
                Date = WriteOffDate.SelectedDate.Value;
                Basis = BasisTextBox.Text;
                DialogResult = true;
            }
        }
    }
}
