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

namespace MenuMb;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    bool isMenuClose = true;

    public MainWindow()
    {
        InitializeComponent();
        Closed += MainWindow_Closed;
        MainConteiner.Navigated += MainConteiner_Navigated;
        
    }

    private void MainConteiner_Navigated(object sender, NavigationEventArgs e)
    {
        
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
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
        ShowBlock(new MainPage());
    }

    private void ShowBlock(/*Grid grid*/Page page)
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
        DoubleAnimation anim = new DoubleAnimation();
        if (isMenuClose)
        {
            anim.From = MenuContainer.ActualWidth;
            anim.To = 200;
            anim.Duration = TimeSpan.FromSeconds(0.25);
            isMenuClose = !isMenuClose;
        }
        else
        {
            anim.From = MenuContainer.ActualWidth;
            anim.To = 0;
            anim.Duration = TimeSpan.FromSeconds(0.25);
            isMenuClose = !isMenuClose;
        }

        MenuContainer.BeginAnimation(WidthProperty, anim);
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
        GetUserMenu();
    }

    private void GetUserMenu()
    {
        UserMenu menu = new UserMenu();
        menu.MenuItemBtn1.Click += MenuItemBtn1_Click;
        menu.MenuItemBtn2.Click += MenuItemBtn2_Click;
        menu.MenuItemBtn3.Click += MenuItemBtn3_Click;
        MenuButtonsContainer.Children.Add(menu);
    }

    
}