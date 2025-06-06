using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OCNomenclaturaPage.xaml
    /// </summary>
    public partial class OCNomenclaturaPage : Page
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };
        ObservableCollection<NomenclaturaOCBase> nomenclaturaOCs;
        public OCNomenclaturaPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcNomenclaturaWindow window = new OcNomenclaturaWindow();
            if (window.ShowDialog() == true)
            {
                nomenclaturaOCs.Add(window.NewNomen);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                ContextMenu menu = new ContextMenu();
                var deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Удалить";
                deleteMenuItem.Click += async (o, e) =>
                {
                    var SelectedNomen = OcNomenDataGrid.SelectedItems.Cast<NomenclaturaOCBase>().ToList();

                    if (SelectedNomen != null)
                    {
                        string msg;
                        if (SelectedNomen.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить ОС \"{SelectedNomen[0].Name}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?&ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedNomen)
                            {
                                delparam += $"&OcId={item.Id}";
                            }

                            var response = await HttpRequestHelper.DeleteAsync("/oc_nomenclatura/delete", delparam);
                            if (response == "OK")
                            {
                                foreach (var item in SelectedNomen)
                                {
                                    nomenclaturaOCs.Remove(item);
                                }
                            }

                        }
                    }
                    ;

                };
                menu.Items.Add(deleteMenuItem);
                OcNomenDataGrid.ContextMenu = menu;
            }
            await LoadOcNomen();
        }

        private async Task LoadOcNomen()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                nomenclaturaOCs = await HttpRequestHelper.GetAsync<ObservableCollection<NomenclaturaOCBase>>("/oc_nomenclatura/list" , param);
                if (nomenclaturaOCs == null)
                {
                    StatusUpdater.UpdateStatusBar("Данных нет");
                }
                else
                {
                    OcNomenDataGrid.ItemsSource = nomenclaturaOCs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OcNomenDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OcNomenDataGrid.SelectedItem != null)
            {
                var selectedItem = (NomenclaturaOCBase)OcNomenDataGrid.SelectedItem;
                var detailsWindow = new NomenclaturaOCInfoWindow(selectedItem, this.NavigationService);
                detailsWindow.Show();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var SearchList = nomenclaturaOCs.Where(x => x.Inventory_Number.StartsWith(SearchTextBox.Text)).ToList();
            if (SearchList.Count > 0)
            {
                OcNomenDataGrid.ItemsSource = SearchList;
            }
        }
    }
}