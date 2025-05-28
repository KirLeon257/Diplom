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
    /// Логика взаимодействия для OcAdmissionItemWindow.xaml
    /// </summary>
    public partial class OcAmortisationItemWindow : Window
    {
        int NomenId;
        ObservableCollection<OcAmortisationItem> ocAmortisationItems;
        public OcAmortisationItemWindow(int NomenId)
        {
            InitializeComponent();
            this.NomenId = NomenId;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAmortsationItem();
        }

        private async Task LoadAmortsationItem()
        {
            ContextMenu menu = new ContextMenu();
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Click += async (o, e) =>
                {
                    var SelectedAmorts = AmortDataGrid.SelectedItems.Cast<OcAmortisationItem>().ToList();

                    if (SelectedAmorts != null)
                    {
                        string msg;
                        if (SelectedAmorts.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить запись \"{SelectedAmorts[0].Name}\" от \"{SelectedAmorts[0].Date.ToString("dd MMMM yyyy")}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?&ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedAmorts)
                            {
                                delparam += $"&AmortId={item.Id}";
                            }

                            var response = await HttpRequestHelper.DeleteAsync("/oc_amortisation/delete", delparam);


                            if (response == "OK")
                            {
                                foreach (var item in SelectedAmorts)
                                {
                                    ocAmortisationItems.Remove(item);
                                }

                            }

                        }

                    }
                    menu.Items.Add(menuItem);
                };
            }
            try
            {
                var param = $"?ApiToken={LoginUser.User.ApiToken}&NomenId={NomenId}";
                ocAmortisationItems = await HttpRequestHelper.GetAsync<ObservableCollection<OcAmortisationItem>>("/oc_amortisation/item_amortisation", param);
                if (ocAmortisationItems != null)
                {
                    AmortDataGrid.ItemsSource = ocAmortisationItems;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
