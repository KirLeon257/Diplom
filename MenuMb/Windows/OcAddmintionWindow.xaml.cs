﻿using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        List<BasisInfo> BasisInfos = new List<BasisInfo>() { new BasisInfo() { Basis_date = DateTime.Now } };
        ObservableCollection<OcCharacteristic> ocCharacteristics = new ObservableCollection<OcCharacteristic>();
        List<Supplier> Suppliers;
        List<Department> Departments;
        NomenclaturaOCBase OCBase;
        public OCAddmitionBase newAddmission;
        public OcAddmintionWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FormCodesDataGrid.ItemsSource = FormCodes;
            RecipientCodeDataGrid.ItemsSource = RecipientCodes;
            BasisInfoDataGrid.ItemsSource = BasisInfos;
            OcXaracteristicDataGrid.ItemsSource = ocCharacteristics;
            LoadSuppliers();
            LoadDepartment();

        }

        private async void LoadDepartment()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                Departments = await HttpRequestHelper.GetAsync<List<Department>>("/department/list", param);
                if (Departments != null)
                {
                    ExcepteDepartmentBox.ItemsSource = Departments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task LoadSuppliers()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                Suppliers = await HttpRequestHelper.GetAsync<List<Supplier>>("/supplier/list", param);
                if (Suppliers != null)
                {
                    SupplierBox.ItemsSource = Suppliers;
                    ExcepteBox.ItemsSource = Suppliers;
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
                if (!dialog.SelectedItem.Equals(OCBase))
                {
                    OCBase = dialog.SelectedItem;
                    ocCharacteristics.Clear();
                    ocCharacteristics.Add(new OcCharacteristic() { NameCharacteristic = OCBase.Name, Count = 1 });
                    OcNomenNameTextBox.Text = OCBase.Name;
                }

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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var basisCodes = GetBasisCodes();
                basisCodes.Basis = BasisBox.Text;
                var data = new
                {
                    SupplierId = ((Supplier)SupplierBox.SelectedItem).Id,
                    ExcepterId = ((Supplier)ExcepteBox.SelectedItem).Id,
                    DepartmentId = ((Department)ExcepteDepartmentBox.SelectedItem).Id,
                    Basis = basisCodes,
                    DateCreateAddmition = this.DateCreateAddmition.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    EnterDate = this.Date.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    OCId = OCBase.Id,
                    DateTest = this.DateTestResult.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    IsTechnicalCorrect = IsTechnicalCorrectBox.Text,
                    Dorobotka = DorobotkaBox.Text,
                    Xaracteristics = GetXaracteristic(),
                    UserId = LoginUser.User.Id,
                    ApiToken = LoginUser.User.ApiToken
                };


                var response = await HttpRequestHelper.PostAsync("/oc_addmition/add", data);
                if (response != null)
                {
                    int newId = Convert.ToInt32(response);
                    newAddmission = new OCAddmitionBase(newId, OcNomenNameTextBox.Text, OCBase.Inventory_Number, Date.SelectedDate.Value, data.Basis.Basis, data.Basis.Basis_number, data.Basis.Basis_date);
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<OcCharacteristic>? GetXaracteristic()
        {
            List<OcCharacteristic> characteristics = new List<OcCharacteristic>();
            foreach (var item in OcXaracteristicDataGrid.Items)
            {
                var row = item as OcCharacteristic;
                if (row == null)
                    continue;
                row.OcNomenId = OCBase.Id;
                characteristics.Add(row);
            }

            return characteristics.Count != 0 ? characteristics : null;
        }

        private BasisInfo? GetBasisCodes()
        {
            var row = (BasisInfo)BasisInfoDataGrid.Items[0];
            return row;
        }
    }
}
