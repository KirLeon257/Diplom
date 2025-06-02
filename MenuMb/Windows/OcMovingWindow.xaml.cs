using MenuMb.Classes;
using MenuMb.Classes.OC;
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
using System.Windows.Shapes;

namespace MenuMb.Windows
{
    /// <summary>
    /// Логика взаимодействия для OcMovingWindow.xaml
    /// </summary>
    public partial class OcMovingWindow : Window
    {
        internal OcMoving SelectedMoving;
        List<Department> departments;

        public OcMovingWindow(OcMoving selected)
        {
            InitializeComponent();
            SelectedMoving = selected;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = NewDepartmentBox.SelectedItem as Department;
            if (selected != null)
            {
                var data = new
                {
                    old_dep = SelectedMoving.DepartmentId,
                    new_dep = selected.Id,
                    oc_nomen_id = SelectedMoving.NomenId,
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    LoginUser.User.ApiToken
                };
                try
                {
                    var response = await HttpRequestHelper.PutAsync("/oc_moving/move", data);
                    if (response == "OK")
                    {
                        DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDepartments();
            OldDepartmentText.Text = SelectedMoving.Current_Department;
            NomenNameText.Text = SelectedMoving.NomenName;
            var dep = departments.Where(x => x.Name == SelectedMoving.Current_Department).FirstOrDefault();
            if (dep != null)
            {
                OldDepartmentText.Text = dep.Name;
            }
        }

        private async Task LoadDepartments()
        {
            var param = "?ApiToken=" + LoginUser.User.ApiToken;
            try
            {
                departments = await HttpRequestHelper.GetAsync<List<Department>>("/department/list", param);
                NewDepartmentBox.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
