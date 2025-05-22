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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcAddmitionPage.xaml
    /// </summary>
    public partial class OcAddmitionPage : Page
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
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                ContextMenu menu = new ContextMenu();
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
                            msg = $"Вы уверены что хотите удалить принятие ОС \"{SelectedNomen[0].Name}\"?";
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
                menu.Items.Add(deleteMenuItem);
                OcAddDataGrid.ContextMenu = menu;
            }


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
    }
}
