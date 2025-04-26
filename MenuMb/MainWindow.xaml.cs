using MenuMb.Classes.Users;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenuMb.Pages;
using MenuMb.Classes;

namespace MenuMb;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    bool isMenuClose = true;
    private object _nextPage;

    public MainWindow()
    {
        InitializeComponent();
        StatusUpdater.StatusTextBlock = this.LoadStatusText;
        Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (new ExitWindowDialog().ShowDialog() == true) 
        { 
            Application.Current.Shutdown();
        }
        else
        {
            // Очищаем данные пользователя
            LoginUser.User = null;

            // Отображаем NavigationWindow
            var navigationWindow = Application.Current.MainWindow as NavigationWindow;
            if (navigationWindow != null)
            {
                navigationWindow.Show();
            }
        }
    }

    private void MenuOpenBtn_Click(object sender, RoutedEventArgs e)
    {
        MenuAnim();
    }

    private void MenuCloseBtn_Click(object sender, RoutedEventArgs e)
    {
        MenuAnim();
    }

    private void MenuItemBtn1_Click(object sender, RoutedEventArgs e)
    {
        ShowBlock(new ReferenceInformationPage());
    }

    private void ShowBlock(Page page)
    {
        //Grid1.Visibility = Visibility.Collapsed;
        //Grid2.Visibility = Visibility.Collapsed;
        //Grid3.Visibility = Visibility.Collapsed;
        //grid.Visibility = Visibility.Visible;
        MainConteiner.NavigationService.Navigate(page);
        MenuAnim();
    }

    void MenuAnim()
    {
        //DoubleAnimation anim = new DoubleAnimation();
        //if (isMenuClose)
        //{
        //    anim.From = MenuContainer.ActualWidth;
        //    anim.To = 200;
        //    anim.Duration = TimeSpan.FromSeconds(0.25);
        //    isMenuClose = !isMenuClose;
        //}
        //else
        //{
        //    anim.From = MenuContainer.ActualWidth;
        //    anim.To = 0;
        //    anim.Duration = TimeSpan.FromSeconds(0.25);
        //    isMenuClose = !isMenuClose;
        //}

        //MenuContainer.BeginAnimation(WidthProperty, anim);
        TranslateTransform translateTransform = new TranslateTransform();
        MenuContainer.RenderTransform = translateTransform;
        DoubleAnimation animation = new DoubleAnimation();
        if (isMenuClose)
        {
            animation.From = 0;
            animation.To = 210;
            isMenuClose = !isMenuClose;
        }
        else
        {
            animation.From = 210;
            animation.To = 0;
            isMenuClose = !isMenuClose;
        }
        animation.Duration = TimeSpan.FromSeconds(0.25);
        translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);

    }

    private void MenuItemBtn2_Click(object sender, RoutedEventArgs e)
    {
        //ShowBlock(new Page2());
    }

    private void MenuItemBtn3_Click(object sender, RoutedEventArgs e)
    {
        //ShowBlock(Grid3);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        GetMenu(LoginUser.User.Role); 
    }

    private void GetMenu(string Role)
    {
        if(Role == "Admin")
        {
            AdminMenu menu = new AdminMenu();
            menu.UserMenu.MenuItemBtn1.Click += MenuItemBtn1_Click;
            menu.UserMenu.MenuItemBtn2.Click += MenuItemBtn2_Click;
            menu.UserMenu.MenuItemBtn3.Click += MenuItemBtn3_Click;
            menu.AdminMenuItemBtn1.Click += AdminMenuItemBtn1_Click;
            MenuButtonsContainer.Children.Add(menu);
        }
        else if(Role == "User")
        {
            UserMenu menu = new UserMenu();
            menu.MenuItemBtn1.Click += MenuItemBtn1_Click;
            menu.MenuItemBtn2.Click += MenuItemBtn2_Click;
            menu.MenuItemBtn3.Click += MenuItemBtn3_Click;
            MenuButtonsContainer.Children.Add(menu);
        }
       
    }

    private void AdminMenuItemBtn1_Click(object sender, RoutedEventArgs e)
    {
        ShowBlock(new UsersForAdminPage());
    }

    private void ExitBtn_Click_1(object sender, RoutedEventArgs e)
    {
        // Очищаем данные пользователя
        LoginUser.User = null;

        // Закрываем MainWindow
        this.Close();

        // Отображаем NavigationWindow
        var navigationWindow = Application.Current.MainWindow as NavigationWindow;
        if (navigationWindow != null)
        {
            navigationWindow.Show();
        }
    }
}