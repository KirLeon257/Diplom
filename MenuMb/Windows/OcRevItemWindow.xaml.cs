﻿using MenuMb.Classes;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System.Collections.ObjectModel;
using System.Windows;

namespace MenuMb.Windows
{
    /// <summary>
    /// Логика взаимодействия для OcRevItemWindow.xaml
    /// </summary>
    public partial class OcRevItemWindow : Window
    {
        ObservableCollection<OcRevaluationItem> ocRevaluationItems;
        int nomenId;
        public OcRevItemWindow(int nomenId)
        {
            InitializeComponent();
            this.nomenId = nomenId;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRevItem();
        }

        private async Task LoadRevItem()
        {
            var param = $"?ApiToken={LoginUser.User.ApiToken}&NomenId={nomenId}";
            try
            {
                ocRevaluationItems = await HttpRequestHelper.GetAsync<ObservableCollection<OcRevaluationItem>>("/oc_revaluation/item_info", param);
                if (ocRevaluationItems != null)
                {
                    OCRevItemDataGrid.ItemsSource = ocRevaluationItems;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
    }
}
