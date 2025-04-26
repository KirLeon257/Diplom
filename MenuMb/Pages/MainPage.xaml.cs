using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class ReferenceInformationPage : Page
    {
        public ReferenceInformationPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //if (LoginUser.User.Role == "Admin")
            //{
            //    ContextMenu menu = new ContextMenu();
            //    var menuItem = new MenuItem();
            //    menuItem.Header = "Удалить";
            //    menu.Items.Add(menuItem);
                
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DepartmentPage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SuppliersPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ResponseblePersonePage());
        }
    }
}
