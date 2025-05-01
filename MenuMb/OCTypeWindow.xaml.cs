using MenuMb.Classes.OC;
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
using System.Windows.Shapes;

namespace MenuMb
{
    /// <summary>
    /// Логика взаимодействия для OCTypeWindow.xaml
    /// </summary>
    public partial class OCTypeWindow : Window
    {
        public int Code {  get; private set; }
        public string Name {  get; private set; }
        public int SPI { get; private set; }

        public OCTypeWindow()
        {
            InitializeComponent();
        }

        public OCTypeWindow(int code, string name, int SPI)
        {
            InitializeComponent();
            CodeTxt.Text = code.ToString();
            NameTxt.Text = name;
            SPITxt.Text = SPI.ToString();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int _Code = -1,_SPI = -1;
            if (string.IsNullOrEmpty(NameTxt.Text) || !int.TryParse(CodeTxt.Text, out _Code) || !int.TryParse(SPITxt.Text, out _SPI))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            Code = _Code;
            Name = NameTxt.Text;
            SPI = _SPI;
            DialogResult = true;
        }

        private void CodeTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9 || CodeTxt.Text.Length>5) && e.Key != Key.Back ) { e.Handled = true; }
        }
    }
}
