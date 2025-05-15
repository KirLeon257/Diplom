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
    SocketService _socketService;
    public MainWindow()
    {
        InitializeComponent();
        StatusUpdater.TextBlock = this.LoadStatusText;
        Closing += MainWindow_Closing;
    }

    

    private async void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (new ExitWindowDialog().ShowDialog() == true) 
        { 
            await _socketService.CloseAsync();
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
        MainConteiner.NavigationService.Navigate(page);
        MenuAnim();
    }

    void MenuAnim()
    {
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
       ShowBlock(new OCOperationPage());
    }

    private void MenuItemBtn3_Click(object sender, RoutedEventArgs e)
    {
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        GetMenu(LoginUser.User.Role);
        await ConnectToSocket();
    }

    private async Task ConnectToSocket()
    {
        // Запуск фонового WebSocket-клиента
        try
        {
            
            _socketService = new(this.MainConteiner.NavigationService);
            await _socketService.StartListeningAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void GetMenu(string Role)
    {
        if(Role == "Admin")
        {
            AdminMenu menu = new AdminMenu();
            menu.UserMenu.MenuItemBtn1.Click += MenuItemBtn1_Click;
            menu.UserMenu.MenuItemBtn2.Click += MenuItemBtn2_Click;
            //menu.UserMenu.MenuItemBtn3.Click += MenuItemBtn3_Click;
            menu.AdminMenuItemBtn1.Click += AdminMenuItemBtn1_Click;
            MenuButtonsContainer.Children.Add(menu);
        }
        else if(Role == "User")
        {
            UserMenu menu = new UserMenu();
            menu.MenuItemBtn1.Click += MenuItemBtn1_Click;
            menu.MenuItemBtn2.Click += MenuItemBtn2_Click;
            //menu.MenuItemBtn3.Click += MenuItemBtn3_Click;
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