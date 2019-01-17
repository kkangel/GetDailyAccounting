using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDailyAccounting
{
    static class  ExcelGo
    {
        public static void JsonToExcel(List<Dictionary<string, string>> json, string fileName)
          {
              using (MemoryStream ms = new MemoryStream())
              {
  
                  IWorkbook workbook = null;
  
                  if (fileName.IndexOf(".xlsx") > 0)
                      workbook = new XSSFWorkbook();
                else if (fileName.IndexOf(".xls") > 0)
                     workbook = new HSSFWorkbook();
                 ISheet sheet = workbook.CreateSheet();
                 IRow headerRow = sheet.CreateRow(0);
 
                 //// handling header.  
                 //foreach (DataColumn column in table.Columns)
                 //    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value  
 
 
                 var keys = json.FirstOrDefault().Keys.ToList();
                 for (var i = 0; i<keys.Count(); i++)
                 {
                     headerRow.CreateCell(i).SetCellValue(keys[i]);
                 }
 
                 // handling value.  
                 int rowIndex = 1;
                 
                 for (var i = 0; i<json.Count(); i++)
                 {
                     IRow dataRow = sheet.CreateRow(rowIndex);
 
                     var values = json[i].Values.ToList();
                     for (var j = 0; j<values.Count();j++)
                     {
                         if (values[j] != null)
                         {
                             dataRow.CreateCell(j).SetCellValue(values[j].ToString());
                        }
                         
                     }
 
                     rowIndex++;
                 }
 
                 workbook.Write(ms);
                 ms.Flush();
 
                 using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                 {
                     byte[] data = ms.ToArray();
 
                     fs.Write(data, 0, data.Length);
                     fs.Flush();
 
                     data = null;
                 }
             }
        }


          public static void dataTableToExcel(DataTable table, string fileName)
                     {
                         using (MemoryStream ms = new MemoryStream())
                             {
                 
                                 IWorkbook workbook = null;
                 
                                 if (fileName.IndexOf(".xlsx") > 0)
                                         workbook = new XSSFWorkbook();
                                 else if (fileName.IndexOf(".xls") > 0)
                                         workbook = new HSSFWorkbook();
                                 ISheet sheet = workbook.CreateSheet();
                                 IRow headerRow = sheet.CreateRow(0);
                 
                                // handling header.  
                                 foreach (DataColumn column in table.Columns)
                                         headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value  
                 
                                 // handling value.  
                                 int rowIndex = 1;
                 
                               foreach (DataRow row in table.Rows)
                                     {
                                         IRow dataRow = sheet.CreateRow(rowIndex);
                     
                                         foreach (DataColumn column in table.Columns)
                                             {
                                                 dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                             }
                     
                                         rowIndex++;
                                     }
                 
                                 workbook.Write(ms);
                                 ms.Flush();
                 
                                 using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                                     {
                                         byte[] data = ms.ToArray();
                     
                                         fs.Write(data, 0, data.Length);
                                         fs.Flush();
                     
                                         data = null;
                                     }
                            }
                     }



        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName, int sheetName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(sheetName);//读取第一个sheet，当然也可以循环读取每个sheet
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行
                                int cellCount = firstRow.LastCellNum;//列数

                                //构建datatable的列
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;
                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception:" + ex.ToString());
                Console.ReadLine();
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }


    }
}
