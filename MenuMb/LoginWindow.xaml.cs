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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginHost : NavigationWindow
    {
        public LoginHost()
        {
            InitializeComponent();
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = this.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            var translateTransform = new TranslateTransform();
            this.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
