using GemBox.Spreadsheet;
using SSK.Online.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSK.Online.Helpers
{
    public class ExportData
    {
        public static string ExportToExcel(DataTable elementData, DataTable recommendationData)
        {
            var fPath = Path.GetTempFileName() + ".xlsx";
            try
            {
                SpreadsheetInfo.SetLicense(AppConstants.GemLic);

                var workbook = new ExcelFile();
                var worksheet = workbook.Worksheets.Add("Exported Data - SSK Online");
                var styleHeader2 = new CellStyle();
                styleHeader2.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                styleHeader2.VerticalAlignment = VerticalAlignmentStyle.Center;
                styleHeader2.Font.Weight = ExcelFont.BoldWeight;
                styleHeader2.FillPattern.SetSolid(System.Drawing.Color.LightGreen);

                var styleHeader = new CellStyle();
                styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
                styleHeader.Font.Weight = ExcelFont.BoldWeight;
                styleHeader.FillPattern.SetSolid(System.Drawing.Color.AliceBlue);
                int Rows = 0;
                int Cols = 0;
                foreach (DataColumn dc in elementData.Columns)
                {
                    worksheet.Cells[Rows, Cols].Value = dc.ColumnName;
                    worksheet.Cells[Rows, Cols].Style = styleHeader;
                    Cols++;
                }
                Rows = 1;
                foreach (DataRow dr in elementData.Rows)
                {

                    Cols = 0;
                    foreach (DataColumn dc in elementData.Columns)
                    {
                        worksheet.Cells[Rows, Cols].Value = dr[dc.ColumnName].ToString();
                        Cols++;
                    }
                    Rows++;
                }
                Cols = 0;
                foreach (DataColumn dc in recommendationData.Columns)
                {
                    worksheet.Cells[Rows, Cols].Value = dc.ColumnName;
                    worksheet.Cells[Rows, Cols].Style = styleHeader2;
                    Cols++;
                }
                Rows++;
                foreach (DataRow dr in recommendationData.Rows)
                {

                    Cols = 0;
                    foreach (DataColumn dc in recommendationData.Columns)
                    {
                        worksheet.Cells[Rows, Cols].Value = dr[dc.ColumnName].ToString();
                        Cols++;
                    }
                    Rows++;
                }
                workbook.Save(fPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error - save to file : {ex.Message}");
                return null;
            }
            return fPath;
        }
    }
}
