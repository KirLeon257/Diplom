using ClosedXML.Excel;
using MenuMb.Classes;
using MenuMb.Classes.Acts;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using MenuMb.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcAddmitionPage.xaml
    /// </summary>
    public partial class OcAddmitionPage : System.Windows.Controls.Page
    {
        ObservableCollection<OCAddmitionBase> ocAdditions;
        public OcAddmitionPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcAddmintionWindow addmintionWindow = new OcAddmintionWindow();
            if (addmintionWindow.ShowDialog() == true)
            {
                ocAdditions.Add(addmintionWindow.newAddmission);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            var PrintBtn = new MenuItem();
            PrintBtn.Header = "Печать";
            var ActPrintBtn = new MenuItem();
            ActPrintBtn.Header = "Акт приема";
            ActPrintBtn.Click += async (o, e) =>
            {
                PrintAktPriem();
            };
            PrintBtn.Items.Add(ActPrintBtn);
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                var deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Удалить";
                deleteMenuItem.Click += async (o, e) =>
                {
                    var SelectedNomen = OcAddDataGrid.SelectedItems.Cast<OCAddmitionBase>().ToList();

                    if (SelectedNomen != null)
                    {
                        string msg;
                        if (SelectedNomen.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить принятие ОС \"{SelectedNomen[0].NomenName}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedNomen)
                            {
                                delparam += $"&AddId={item.AddId}";
                            }

                            var response = await HttpRequestHelper.DeleteAsync("/oc_addmition/delete", delparam);
                            if (response != null && response == "OK")
                            {

                                foreach (var item in SelectedNomen)
                                {
                                    ocAdditions.Remove(item);
                                }

                            }
                        }
                    }
                    ;

                };
                menu.Items.Add(PrintBtn);
                menu.Items.Add(deleteMenuItem);
            }
            OcAddDataGrid.ContextMenu = menu;
            try
            {
                ocAdditions = await HttpRequestHelper.GetAsync<ObservableCollection<OCAddmitionBase>>("/oc_addmition/list", "?ApiToken=" + LoginUser.User.ApiToken);
                if (ocAdditions != null)
                {
                    OcAddDataGrid.ItemsSource = ocAdditions;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void PrintAktPriem()
        {
            try
            {
                var Selected = OcAddDataGrid.SelectedItem as OCAddmitionBase;
                AktProgressWindow aktProgress = new AktProgressWindow();
                await aktProgress.PrintAktPriem(Selected.AddId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var SearchList = ocAdditions.Where(x => x.Inventory_number.StartsWith(SearchTextBox.Text)).ToList();
            if (SearchList.Count > 0)
            {
                OcAddDataGrid.ItemsSource = SearchList;
            }
        }
    }
}
