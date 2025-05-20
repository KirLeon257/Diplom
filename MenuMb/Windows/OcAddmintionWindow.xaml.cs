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
using System.Windows.Shapes;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для OcAddmintionWindow.xaml
    /// </summary>
    public partial class OcAddmintionWindow : Window
    {
        List<FormCode> FormCodes = new List<FormCode>()
        {
            new FormCode() {Code = 0501030,Name="Форма по ОКУД"},
            new FormCode() {Code = 0,Name = "по ОКЮЛП"},
        };
        List<FormCode> RecipientCodes = new List<FormCode>()
        {
            new FormCode() {Code = 0,Name = "по ОКЮЛП"},
        };
        List<BasisInfo> BasisInfos = new List<BasisInfo>() { new BasisInfo() {Basis_date = DateTime.Now } };
        List<Supplier> Suppliers;
        NomenclaturaOCBase OCBase;
        public OcAddmintionWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FormCodesDataGrid.ItemsSource = FormCodes;
            RecipientCodeDataGrid.ItemsSource = RecipientCodes;
            BasisInfoDataGrid.ItemsSource = BasisInfos;
            await LoadSuppliers();
        }

        private async Task LoadSuppliers()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                var response = await HttpRequestHelper.GetAsync<List<Supplier>>("/supplier/list", param);
                if (response != null)
                {
                    SupplierBox.ItemsSource = response;
                    ExcepteBox.ItemsSource= response;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ChoiseOcNomenWindowDialog();
            if (dialog.ShowDialog() == true)
            {
                OCBase = dialog.SelectedItem;
               
            }
        }

        private void SupplierBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupplierBox.SelectedItem != null)
            {
                SupplierYNPTextBox.Text = ((Supplier)SupplierBox.SelectedItem).YNP;
            }
        }

        private void ExcepteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExcepteBox.SelectedItem != null)
            {
                ExcepteYNPTextBox.Text = ((Supplier)ExcepteBox.SelectedItem).YNP;
            }
        }
    }
}
