using ClosedXML.Excel;
using MenuMb.Classes;
using MenuMb.Classes.Acts;
using MenuMb.Classes.OC;
using MenuMb.Classes.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Логика взаимодействия для AktProgressWindow.xaml
    /// </summary>
    public partial class AktProgressWindow : Window
    {
        public AktProgressWindow()
        {
            InitializeComponent();
            
        }

        public async Task PrintAktPriem(int addId)
        {
            try
            {
                var param = $"?AddId={addId}&ApiToken={LoginUser.User.ApiToken}";
                var Info = await HttpRequestHelper.GetAsync<ActPriem>("/oc_addmition/act_priem", param);
                if (Info != null)
                {
                    this.Show();
                    await GenerateAktPrixod(Info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
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
                    await SetAktPriemDates(cells, infoAct);
                    UpgradeStatusBar(28);
                    await FillOcXarakt(worksheet, infoAct);
                    string DirectoryName = "\\Акты\\Прием";
                    if (!Directory.Exists(DirectoryName))
                    {
                        Directory.CreateDirectory(DirectoryName);
                    }
                    string filename = DirectoryName + "\\Акт_Приема_" + $"{infoAct.AddInfo.NomenName}_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                    workbook.SaveAs(filename);
                    UpgradeStatusBar(50);
                    Process.Start("explorer.exe", DirectoryName);
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private async Task FillOcXarakt(IXLWorksheet worksheet, ActPriem infoAct)
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

        async Task SetAktPriemDates(IXLCells cells, ActPriem info)
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
                await UpgradeStatusBar(1);
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

        async Task UpgradeStatusBar(int value)
        {
            progressBar.Value += value;
        }

        public async Task PrintAktPerem(int PeremId)
        {
            try
            {
                var param = $"?PeremId={PeremId}&ApiToken={LoginUser.User.ApiToken}";
                var Info = await HttpRequestHelper.GetAsync<AktPerem>("/oc_moving/akt_perem", param);
                if (Info != null)
                {
                    this.Show();
                    await GenerateAktPerem(Info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        private async Task GenerateAktPerem(AktPerem info)
        {
            using (var workbook = new XLWorkbook(".\\ActsObrasec\\Akt_obrazec_perem.xlsx"))
            {
                try
                {
                    var worksheet = workbook.Worksheet(1);
                    var cells = worksheet.CellsUsed();
                    await SetAktPeremDates(cells, info);
                    UpgradeStatusBar(28);
                    await FillOcXarakt(worksheet, info);
                    string DirectoryName = "\\Акты\\Перемещение";
                    if (!Directory.Exists(DirectoryName))
                    {
                        Directory.CreateDirectory(DirectoryName);
                    }
                    string filename = DirectoryName + "\\Акт_Перемещения_" + $"{info.NomenName}_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                    workbook.SaveAs(filename);
                    UpgradeStatusBar(50);
                    Process.Start("explorer.exe", DirectoryName);
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        async Task SetAktPeremDates(IXLCells cells, AktPerem info)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                {"Date",info.Date.ToString("dd MMMM yyyy") },
                {"MoveId",info.MoveId.ToString($"D6") },
                {"MOL",info.MOL },
                {"Old_dep",info.Old_dep },
                {"New_dep",info.New_dep }
            };

            foreach (KeyValuePair<string, string> pair in keyValuePairs)
            {
                if (FillCell(cells, pair.Key, pair.Value) == false)
                {
                    throw new Exception("Не удалось заполнить данные с датами");
                }
                await UpgradeStatusBar(1);
            }

        }

        private async Task FillOcXarakt(IXLWorksheet worksheet, AktPerem infoAct)
        {
            var cellName = worksheet.CellsUsed().Where(x => x.GetString() == "<NomenName>").FirstOrDefault();
            var cellCount = worksheet.CellsUsed().Where(x => x.GetString() == "<CountType>").FirstOrDefault();
            var cellsCost = worksheet.CellsUsed().Where(x => x.GetString() == "<Cost>").ToList();
            if (cellName == null || cellCount == null || cellsCost==null)
            {
                throw new Exception("Не удалось заполнить характеристику ОС");
            }
            cellName.Value = infoAct.NomenName;
            cellCount.Value = "шт.";
            foreach(var cell in cellsCost)
            {
                cell.Value = infoAct.InitialCost;
            }
            //var NamerowStart = cellName.Address.RowNumber;
            //var NamecolumnStart = cellName.Address.ColumnNumber;
            //var CountColumnStart = cellCount.Address.ColumnNumber;

            //var row = NamerowStart;
            //var columnName = NamecolumnStart;
            //var columnCount = CountColumnStart;


            //foreach (var xar in infoAct.xaract)
            //{
            //    if ((row - NamerowStart) > 4 && infoAct.xaract.Count() > 4)
            //    {
            //        worksheet.Row(row).InsertRowsBelow(1);
            //    }
            //    FillCell(worksheet.Cell(row, columnName), xar.NameCharacteristic);
            //    FillCell(worksheet.Cell(row, columnCount), xar.Count.ToString());
            //    row++;
            //}

        }
    }
}
