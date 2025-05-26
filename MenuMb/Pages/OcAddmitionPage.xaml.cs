using ClosedXML.Excel;
using MenuMb.Classes;
using MenuMb.Classes.Acts;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace MenuMb.Pages
{
    /// <summary>
    /// Логика взаимодействия для OcAddmitionPage.xaml
    /// </summary>
    public partial class OcAddmitionPage : System.Windows.Controls.Page
    {
        ObservableCollection<OCAddmitionBase> ocAdditions;
        public OcAddmitionPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OcAddmintionWindow addmintionWindow = new OcAddmintionWindow();
            if (addmintionWindow.ShowDialog() == true)
            {
                ocAdditions.Add(addmintionWindow.newAddmission);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            var PrintBtn = new MenuItem();
            PrintBtn.Header = "Печать";
            var ActPrintBtn = new MenuItem();
            ActPrintBtn.Header = "Акт приема";
            ActPrintBtn.Click += async (o, e) =>
            {
                PrintAktPriem();
            };
            PrintBtn.Items.Add(ActPrintBtn);
            if (LoginUser.User.Role == RoleEnum.Admin.ToString())
            {
                var deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Удалить";
                deleteMenuItem.Click += async (o, e) =>
                {
                    var SelectedNomen = OcAddDataGrid.SelectedItems.Cast<OCAddmitionBase>().ToList();

                    if (SelectedNomen != null)
                    {
                        string msg;
                        if (SelectedNomen.Count == 1)
                        {
                            msg = $"Вы уверены что хотите удалить принятие ОС \"{SelectedNomen[0].NomenName}\"?";
                        }
                        else
                        {
                            msg = $"Вы уверены что хотите удалить выбранные элементы?";
                        }
                        if (MessageBox.Show(msg, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string delparam = $"?ApiToken={LoginUser.User.ApiToken}";
                            foreach (var item in SelectedNomen)
                            {
                                delparam += $"&AddId={item.AddId}";
                            }

                            var response = await HttpRequestHelper.DeleteAsync("/oc_addmition/delete", delparam);
                            if (response != null && response == "OK")
                            {

                                foreach (var item in SelectedNomen)
                                {
                                    ocAdditions.Remove(item);
                                }

                            }
                        }
                    }
                    ;

                };
                menu.Items.Add(PrintBtn);
                menu.Items.Add(deleteMenuItem);
            }
            OcAddDataGrid.ContextMenu = menu;
            try
            {
                ocAdditions = await HttpRequestHelper.GetAsync<ObservableCollection<OCAddmitionBase>>("/oc_addmition/list", "?ApiToken=" + LoginUser.User.ApiToken);
                if (ocAdditions != null)
                {
                    OcAddDataGrid.ItemsSource = ocAdditions;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void PrintAktPriem()
        {
            try
            {
                var Selected = OcAddDataGrid.SelectedItem as OCAddmitionBase;
                if (Selected is null)
                {
                    MessageBox.Show("Выберите запись в таблице");
                    return;
                }
                var param = $"?AddId={Selected.AddId}&ApiToken={LoginUser.User.ApiToken}";
                var Info = await HttpRequestHelper.GetAsync<ActPriem>("/oc_addmition/act_priem", param);
                if (Info != null)
                {
                    await GenerateAktPrixod(Info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task GenerateAktPrixod(ActPriem infoAct)
        {
            using (var workbook = new XLWorkbook(".\\ActsObrasec\\blank-priem-peredacha-oc.xlsx"))
            {
                try
                {
                    var worksheet = workbook.Worksheet(1);
                    var cells = worksheet.CellsUsed();
                    SetDates(cells, infoAct);
                    FillOcXarakt(worksheet, infoAct);
                    string DirectoryName = "\\Акты\\Прием";
                    if (!Directory.Exists(DirectoryName))
                    {
                        Directory.CreateDirectory(DirectoryName);
                    }
                    string filename = DirectoryName + "\\Акт_Приема_" + $"{infoAct.AddInfo.NomenName}_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                    workbook.SaveAs(filename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void FillOcXarakt(IXLWorksheet worksheet, ActPriem infoAct)
        {
            var cellName = worksheet.CellsUsed().Where(x => x.GetString() == "<XaracteristicName>").FirstOrDefault();
            var cellCount = worksheet.CellsUsed().Where(x => x.GetString() == "<XaracteristicCount>").FirstOrDefault();
            if (cellName == null || cellCount == null)
            {
                throw new Exception("Не удалось заполнить характеристику ОС");
            }

            var NamerowStart = cellName.Address.RowNumber;
            var NamecolumnStart = cellName.Address.ColumnNumber;
            var CountColumnStart = cellCount.Address.ColumnNumber;

            var row = NamerowStart;
            var columnName = NamecolumnStart;
            var columnCount = CountColumnStart;


            foreach (var xar in infoAct.xaract)
            {
                if ((row - NamerowStart) > 4 && infoAct.xaract.Count() > 4)
                {
                    worksheet.Row(row).InsertRowsBelow(1);
                }
                FillCell(worksheet.Cell(row, columnName), xar.NameCharacteristic);
                FillCell(worksheet.Cell(row, columnCount), xar.Count.ToString());
                row++;
            }

        }

        void SetDates(IXLCells cells, ActPriem info)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                {"Date", info.AddInfo.AddmissionDate.ToString("dd MMMM yyyy")},
                {"DateTest",info.AddInfo.TestDate.ToString("dd MMMM yyyy")},
                {"DateEnter",info.AddInfo.EnterDate.ToString("dd MMMM yyyy") },
                {"AddDate", info.AddInfo.AddmissionDate.ToString("dd MMMM yyyy")},
                {"Basis_date",info.AddInfo.Basis_date.ToString("dd MMMM yyyy") },
                {"Supplier",info.supplier.Name},
                {"SupplierYNP",info.supplier.YNP},
                {"Excepter",info.excepter.Name},
                {"ExcepterYNP",info.excepter.YNP},
                {"NomenDepartment",info.AddInfo.Department},
                {"Basis",info.AddInfo.Basis},
                {"Basis_number",info.AddInfo.Basis_number },
                {"AddId",$"{info.AddInfo.AddId.ToString("D6")}" },
                {"NomenName",info.AddInfo.NomenName },
                {"IsTechnicalCorrect",info.AddInfo.IsTechnicalCorrect },
                {"Dorabotka",info.AddInfo.Dorabotka },
                {"NomenInvNum",info.AddInfo.Inventory_number },
                {"EqupmentCode",info.AddInfo.EquipmentCode.ToString() },
                {"NomenInitCoast",info.AddInfo.InitialCost.ToString() },
                {"NomenSPI",info.AddInfo.SPI },
                {"NomenAmortisationType",info.AddInfo.AmortisationType == "Liner"? "Линейный":"" },
                {"AmortisationYearNorm",info.AddInfo.AmortisationYearNorm.ToString("N2")+"% в год" }
            };

            foreach (KeyValuePair<string, string> pair in keyValuePairs)
            {
                if (FillCell(cells, pair.Key, pair.Value) == false)
                {
                    throw new Exception("Не удалось заполнить данные с датами");
                }
            }

        }



        bool FillCell(IXLCells cells, string CellName, string value)
        {
            var usedcells = cells.Where(c => c.Value.ToString() == $"<{CellName}>").ToList();
            if (usedcells != null)
            {
                foreach (var cell in usedcells)
                {
                    cell.Value = value;
                }
                return true;
            }
            return false;
        }

        bool FillCell(IXLCell cell, string value)
        {
            if (value != null)
            {
                cell.Value = value;
                return true;
            }
            return false;
        }

        private void OcAddDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
