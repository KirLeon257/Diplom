using DotNetEnv;
using MenuMb.Classes;
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

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для ConnSettingPage.xaml
    /// </summary>
    public partial class ConnSettingPage : Page
    {
        string DefaultAddress;
        public ConnSettingPage() 
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            ServerIpTxt.Text = HttpRequestHelper.GetBaseAddress();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (ConnectionServerSetings.ServerIp != ServerIpTxt.Text)
            //{
            //    ConnectionServerSetings.ServerIp = ServerIpTxt.Text;
            //}
            //ConnectionServerSetings.ServerIp = ServerIpTxt.Text;
            HttpRequestHelper.ChangeBaseAddress(ServerIpTxt.Text);
            this.NavigationService.GoBack();
        }
    }
}
