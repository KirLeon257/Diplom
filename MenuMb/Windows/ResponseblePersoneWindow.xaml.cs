using MenuMb.Classes;
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
    /// Логика взаимодействия для ResponseblePersoneWindow.xaml
    /// </summary>
    public partial class ResponseblePersoneWindow : Window
    {
        public ResponseblePerson Person { get; set; }
        public ResponseblePersoneWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int Code = 0;
            if (string.IsNullOrEmpty(NameTxt.Text) || string.IsNullOrEmpty(SurnameTxt.Text) || string.IsNullOrEmpty(PatronymicTxt.Text) || !int.TryParse(CodeTxt.Text,out Code))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            Person = new ResponseblePerson(Code, NameTxt.Text, SurnameTxt.Text, PatronymicTxt.Text);
            DialogResult = true;
        }
    }
}
