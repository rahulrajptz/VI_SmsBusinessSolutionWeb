using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Web.Mvc;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.UI;
using ClosedXML.Excel;
using Templateprj.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ExcelDataReader;

namespace Templateprj.Helpers
{
    public class ExcelExtension
    {
        private const string xlsContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string csvContentType = "text/csv";
        public void ExportToSpreadsheet_neww(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name);
            context.Response.End();
        }

        public void ExportToExcel(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();

            string csv = string.Empty;
            foreach (DataColumn column in table.Columns)
            {
                csv += column.ColumnName + ",";
            }
            csv += "\r\n";
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    csv += row[i].ToString().Replace(",", string.Empty) + ",";
                }
                csv += "\r\n";
            }

            context.Response.ContentType = "application/ms-excel";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name);
            context.Response.ContentEncoding = System.Text.Encoding.Unicode;
            context.Response.Charset = "";
            context.Response.Output.Write(csv);
            context.Response.End();


        }

        #region ExportToCSV


        public void ExportTo_CSV(OracleDataReader reader, string fileName, string type)
        {
            using (reader)
            {
                fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".csv";
                string attachment = "attachment; filename=\"" + fileName + "\"";

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;// ("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = csvContentType;
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.Headers.Add("Set-Cookie", "fileDownload=true; path=/");

                string value, data = "";
                int i, rowCount = 0, colCount = reader.FieldCount, flushFreq = 1000;
                DataTable dtSchema = reader.GetSchemaTable();

                //try
                //{
                //    FieldInfo fi = reader.GetType().GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                //    int rowSize = (int)fi.GetValue(reader);
                //    reader.pre = rowSize * 1000;
                //}
                //catch (Exception ex)
                //{
                //    LogWriter.Write("ExcelExtension.ExportToCSV2 :: Unable to set FetchSize :: Exception ::" + ex.Message);
                //}

                if (dtSchema != null)
                {
                    if (type == "1")
                    {
                        for (i = 0; i < colCount - 1; i++)
                        {
                            value = dtSchema.Rows[i]["ColumnName"].ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                                value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                            data += value + ",";
                        }
                    }
                    else if (type == "2")
                    {
                        for (i = 0; i < colCount; i++)
                        {
                            value = dtSchema.Rows[i]["ColumnName"].ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                                value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                            data += value + ",";
                        }
                    }
                }
                LogWriter.Write("data:::" + data);
                try
                {
                    HttpContext.Current.Response.Write(data + Environment.NewLine);
                }
                catch (Exception ex) { LogWriter.Write("Exception:::" + ex.Message); }

                if (reader.HasRows)
                {
                    object temp;
                    while (reader.Read())
                    {
                        rowCount++;

                        data = "";

                        if (type == "1")
                        {

                            for (i = 0; i < colCount - 1; i++)
                            {
                                temp = reader[i];
                                if (temp is DateTime)
                                {
                                    value = ((DateTime)temp).ToString("dd MMM yyyy HH:mm:ss");
                                }
                                else
                                {
                                    value = temp.ToString();
                                    if (!string.IsNullOrWhiteSpace(value))
                                        value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                                }
                                data += value + ",";
                            }
                        }
                        else if (type == "2")
                        {
                            for (i = 0; i < colCount; i++)
                            {
                                temp = reader[i];
                                if (temp is DateTime)
                                {
                                    value = ((DateTime)temp).ToString("dd MMM yyyy HH:mm:ss");
                                }
                                else
                                {
                                    value = temp.ToString();
                                    if (!string.IsNullOrWhiteSpace(value))
                                        value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                                }
                                data += value + ",";
                            }
                        }
                        HttpContext.Current.Response.Write(data + Environment.NewLine);

                        if (rowCount >= flushFreq)
                        {
                            HttpContext.Current.Response.Flush();
                            rowCount = 0;
                        }
                    }
                }
                else
                {
                    data = "";
                    for (i = 0; i < colCount / 2; i++)
                    {
                        data += ",";
                    }
                    data += "No Data found";
                    for (i = colCount / 2; i < colCount; i++)
                    {
                        data += ",";
                    }
                    HttpContext.Current.Response.Write(data + Environment.NewLine);
                }
                LogWriter.Write("StatusCode:::" + HttpContext.Current.Response.StatusCode.ToString());
                LogWriter.Write("StatusDescription:::" + HttpContext.Current.Response.StatusDescription.ToString());
                LogWriter.Write("Status:::" + HttpContext.Current.Response.Status.ToString());
                LogWriter.Write("IsClientConnected:::" + HttpContext.Current.Response.IsClientConnected.ToString());
                LogWriter.Write("Status:::" + HttpContext.Current.Response.Expires.ToString() + "  Now::" + System.DateTime.Now.ToString());
                //LogWriter.Write("Status:::" + HttpContext.Current.Response..ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public void writeCSVfile(DataTable theData, string fileName)
        {
            GridView gv = new GridView();
            gv.DataSource = theData;
            gv.DataBind();

            HttpContext context = HttpContext.Current;
            context.Response.Clear();

            context.Response.Buffer = true;

            context.Response.AddHeader("content-disposition",

            "attachment;filename=GridViewExport.doc");

            context.Response.Charset = "";

            context.Response.ContentType = "application/vnd.ms-word ";

            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gv.AllowPaging = false;

            gv.DataBind();

            gv.RenderControl(hw);

            context.Response.Output.Write(sw.ToString());

            context.Response.Flush();

            context.Response.End();


            //context.Response.Clear();
            //context.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            //context.Response.AddHeader("content-disposition", "attachment;filename=ccc.xls");
            //// If you want the option to open the Excel file without saving then    // comment out the line below
            //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //context.Response.ContentType = "application/vnd.xls";
            //context.Response.Charset = "UTF-8";
            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            //gv.RenderControl(htmlWrite);
            //context.Response.Write(stringWrite.ToString());
            //context.Response.Flush();
            //context.Response.End();
        }

        public byte[] ExportToCSV(DataTable table, ref string fileName, out string contentType)
        {
            contentType = csvContentType;
            fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".csv";

            StringBuilder csvBuilder = new StringBuilder();
            string value, data = "", csvCharRegEx = "[\"',]";
            int i, colCount = table.Columns.Count, rowCount = table.Rows.Count;

            for (i = 0; i < colCount; i++)
            {
                value = table.Columns[i].ColumnName;
                if (!string.IsNullOrWhiteSpace(value))
                    value = Regex.Replace(value, csvCharRegEx, "");
                data += value + ",";
            }
            csvBuilder.AppendLine(data);

            DataRow dr; object temp;
            if (rowCount > 0)
            {
                for (i = 0; i < rowCount; i++)
                {
                    data = "";
                    dr = table.Rows[i];
                    for (int j = 0; j < colCount; j++)
                    {
                        temp = dr[j];
                        if (temp.GetType() == typeof(DateTime))
                        {
                            value = ((DateTime)temp).ToString("dd MMM yyyy HH:mm:ss");
                        }
                        else
                        {
                            value = temp.ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                                value = Regex.Replace(value, csvCharRegEx, "");
                        }
                        data += value + ",";
                    }
                    csvBuilder.AppendLine(data);
                }
            }
            else
            {
                data = "";
                for (i = 0; i < colCount / 2; i++)
                {
                    data += ",";
                }
                data += "No Data found";
                for (i = colCount / 2; i < colCount; i++)
                {
                    data += ",";
                }
                csvBuilder.AppendLine(data);
            }

            return new System.Text.UTF8Encoding().GetBytes(csvBuilder.ToString());
        }

        public void ExportToCSV(OracleDataReader reader, string fileName)
        {
            //LogWriter.Write("ExcelExtension.ExportToCSV::" + fileName);
            using (reader)
            {
                fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".csv";
                string attachment = "attachment; filename=\"" + fileName + "\"";

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = csvContentType;
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.Headers.Add("Set-Cookie", "fileDownload=true; path=/");

                //LogWriter.Write("ExcelExtension.ExportToCSV:1:");
                string value, data = "";
                int i, rowCount = 0, colCount = reader.FieldCount, flushFreq = 1000;
                DataTable dtSchema = reader.GetSchemaTable();
                //LogWriter.Write("ExcelExtension.ExportToCSV:dtSchema:" + dtSchema.Rows.Count);

                //try
                //{
                //    FieldInfo fi = reader.GetType().GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                //    int rowSize = (int)fi.GetValue(reader);
                //    reader.FetchSize = rowSize * 10;
                //}
                //catch (Exception ex)
                //{
                //    LogWriter.Write("ExcelExtension.ExportToCSV2 :: Unable to set FetchSize :: Exception ::" + ex.Message);
                //}

                if (dtSchema != null)
                {
                    for (i = 0; i < colCount; i++)
                    {
                        value = dtSchema.Rows[i]["ColumnName"].ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                            value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                        data += value + ",";
                    }
                }

                HttpContext context = HttpContext.Current;

                context.Response.Write(data + Environment.NewLine);
                try
                {
                    if (reader.HasRows)
                    {
                        object temp;
                        while (reader.Read())
                        {
                            rowCount++;
                            data = "";
                            for (i = 0; i < colCount; i++)
                            {
                                temp = reader[i];
                                if (temp is DateTime)
                                {
                                    value = ((DateTime)temp).ToString("dd MMM yyyy HH:mm:ss");
                                }
                                else
                                {
                                    value = temp.ToString();
                                    if (!string.IsNullOrWhiteSpace(value))
                                        value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                                }
                                data += value + ",";
                            }
                            context.Response.Write(data + Environment.NewLine);

                            if (rowCount >= flushFreq)
                            {
                                HttpContext.Current.Response.Flush();
                                rowCount = 0;
                            }
                        }
                    }
                    else
                    {
                        data = "";
                        for (i = 1; i < colCount / 2; i++)
                        {
                            data += ",";
                        }
                        data += "No Data found";
                        for (i = colCount / 2; i < colCount; i++)
                        {
                            data += ",";
                        }
                        context.Response.Write(data + Environment.NewLine);
                    }
                    context.Response.Flush();
                    context.Response.End();
                }
                catch (Exception ex)
                { LogWriter.Write("ExcelExtension.ExportToCSV:Exception:" + ex.Message); }
            }
        }
        public void ExportToCSV(MySqlDataReader reader, string fileName)
        {
            //LogWriter.Write("ExcelExtension.ExportToCSV::" + fileName);
            using (reader)
            {
                fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".csv";
                string attachment = "attachment; filename=\"" + fileName + "\"";

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = csvContentType;
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.Headers.Add("Set-Cookie", "fileDownload=true; path=/");

                //LogWriter.Write("ExcelExtension.ExportToCSV:1:");
                string value, data = "";
                int i, rowCount = 0, colCount = reader.FieldCount, flushFreq = 1000;
                DataTable dtSchema = reader.GetSchemaTable();
                //LogWriter.Write("ExcelExtension.ExportToCSV:dtSchema:" + dtSchema.Rows.Count);

                //try
                //{
                //    FieldInfo fi = reader.GetType().GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                //    int rowSize = (int)fi.GetValue(reader);
                //    reader.FetchSize = rowSize * 10;
                //}
                //catch (Exception ex)
                //{
                //    LogWriter.Write("ExcelExtension.ExportToCSV2 :: Unable to set FetchSize :: Exception ::" + ex.Message);
                //}

                if (dtSchema != null)
                {
                    for (i = 0; i < colCount; i++)
                    {
                        value = dtSchema.Rows[i]["ColumnName"].ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                            value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                        data += value + ",";
                    }
                }

                HttpContext context = HttpContext.Current;

                context.Response.Write(data + Environment.NewLine);
                try
                {
                    if (reader.HasRows)
                    {
                        object temp;
                        while (reader.Read())
                        {
                            rowCount++;
                            data = "";
                            for (i = 0; i < colCount; i++)
                            {
                                temp = reader[i];
                                if (temp is DateTime)
                                {
                                    value = ((DateTime)temp).ToString("dd MMM yyyy HH:mm:ss");
                                }
                                else
                                {
                                    value = temp.ToString();
                                    if (!string.IsNullOrWhiteSpace(value))
                                        value = value.Replace('\'', ' ').Replace('"', ' ').Replace(',', ' ');
                                }
                                data += value + ",";
                            }
                            context.Response.Write(data + Environment.NewLine);

                            if (rowCount >= flushFreq)
                            {
                                HttpContext.Current.Response.Flush();
                                rowCount = 0;
                            }
                        }
                    }
                    else
                    {
                        data = "";
                        for (i = 1; i < colCount / 2; i++)
                        {
                            data += ",";
                        }
                        data += "No Data found";
                        for (i = colCount / 2; i < colCount; i++)
                        {
                            data += ",";
                        }
                        context.Response.Write(data + Environment.NewLine);
                    }
                    context.Response.Flush();
                    context.Response.End();
                }
                catch (Exception ex)
                { LogWriter.Write("ExcelExtension.ExportToCSV:Exception:" + ex.Message); }
            }
        }
        #endregion

        #region ExportToExcel
        //by using nuget package EPPlus
        public byte[] ExportToExcel(DataSet report, ref string fileName, out string contentType, params string[] sheetNames)
        {
            if (report == null)
                throw new ArgumentNullException("DataSet can not be null");

            contentType = xlsContentType;
            fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                int i = 0;
                foreach (DataTable dt in report.Tables)
                {
                    string sheetname = "";
                    try
                    {
                        if (string.IsNullOrWhiteSpace(sheetNames[i]))
                            sheetname = "Sheet " + (i + 1);
                        else
                            sheetname = sheetNames[i];
                    }
                    catch
                    {
                        sheetname = "Sheet " + (i + 1);
                    }
                    i++;

                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetname);

                    //Load the datatable into the sheet, starting from cell A1. 'true' value=>Print the column names on row 1
                    ws.Cells["A1"].LoadFromDataTable(dt, true);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.IsNullOrWhiteSpace(dt.Columns[j].ToString()))
                            continue;

                        if (dt.Columns[j].DataType == typeof(DateTime))
                        {
                            bool hasTime = false;
                            int k = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    DateTime date = (DateTime)dr[j];
                                    if (date.TimeOfDay.TotalSeconds != 0)
                                    {
                                        hasTime = true;
                                        break;
                                    }
                                    else if (k > 50)
                                    {
                                        break;
                                    }
                                    k++;
                                }
                                catch { }
                            }
                            if (hasTime)
                                ws.Column(j + 1).Style.Numberformat.Format = "DD MMM YYYY HH:MM:SS AM/PM";
                            else
                                ws.Column(j + 1).Style.Numberformat.Format = "DD MMM YYYY";
                        }
                    }

                    //Format the header
                    using (ExcelRange rng = ws.Cells[1, 1, 1, dt.Columns.Count])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.White);  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.Black);
                    }

                    //Autofit All Cells
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    //border color 
                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Color.SetColor(Color.Black);
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Color.SetColor(Color.Black);
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Color.SetColor(Color.Black);
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Color.SetColor(Color.Black);

                    //Freeze Header
                    ws.View.FreezePanes(2, 1);
                }
                return pck.GetAsByteArray();
            }
        }

        public byte[] ExportToExcel(DataTable report, ref string fileName, out string contentType, string sheetName = null)
        {
            if (report == null)
                throw new ArgumentNullException("DataSet can not be null");

            DataSet ds = new DataSet();
            ds.Tables.Add(report.Copy());
            return ExportToExcel(ds, ref fileName, out contentType, sheetName);
        }

        public byte[] CustomExportToExcel(DataTable dataReader, int linkIndx, ref string fileName, out string contentType, string sheetName = null)
        {
            contentType = xlsContentType;


            if (dataReader == null)
                return null;
            if (linkIndx == -1)
                return ExportToExcel(dataReader, ref fileName, out contentType, sheetName);

            fileName = fileName.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_") + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                if (string.IsNullOrWhiteSpace(sheetName))
                    sheetName = "Sheet 1";

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. 'true' value=>Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dataReader, true);

                for (int j = 0; j < dataReader.Columns.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(dataReader.Columns[j].ToString()))
                        continue;

                    if (dataReader.Columns[j].DataType == typeof(DateTime))
                    {
                        bool hasTime = false;
                        int k = 0;
                        foreach (DataRow dr in dataReader.Rows)
                        {
                            try
                            {
                                DateTime date = (DateTime)dr[j];
                                if (date.TimeOfDay.TotalSeconds != 0)
                                {
                                    hasTime = true;
                                    break;
                                }
                                else if (k > 50)
                                {
                                    break;
                                }
                                k++;
                            }
                            catch { }
                        }
                        if (hasTime)
                            ws.Column(j + 1).Style.Numberformat.Format = "DD MMM YYYY HH:MM:SS AM/PM";
                        else
                            ws.Column(j + 1).Style.Numberformat.Format = "DD MMM YYYY";
                    }
                }

                //Format the header
                using (ExcelRange rng = ws.Cells[1, 1, 1, dataReader.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.White);
                    rng.Style.Font.Color.SetColor(Color.Black);
                }

                int rCount = dataReader.Rows.Count;
                int cCount = dataReader.Columns.Count;
                if (cCount >= linkIndx)
                {
                    for (int i = 2; i < rCount; i++)
                    {
                        var cell = ws.Cells[i, linkIndx];
                        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            cell.Hyperlink = new Uri(cell.Value.ToString());
                            cell.Value = "Download";
                            cell.Style.Font.Color.SetColor(Color.Blue);
                            cell.Style.Font.Bold = true;
                        }
                    }
                }


                //Autofit All Cells
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //border color 
                ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[ws.Dimension.Address].Style.Border.Top.Color.SetColor(Color.Black);
                ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Color.SetColor(Color.Black);
                ws.Cells[ws.Dimension.Address].Style.Border.Left.Color.SetColor(Color.Black);
                ws.Cells[ws.Dimension.Address].Style.Border.Right.Color.SetColor(Color.Black);

                //Freeze Header
                ws.View.FreezePanes(2, 1);

                return pck.GetAsByteArray();
            }
        }

        //export to xls sheet by sheet using nuget package ClosedXML
        public void ExportToExcelBySheet(DataSet ds, ViewModel model, string tablename)
        {
            string[] tnames = tablename.Split(',');
            int SheetCount = ds.Tables.Count;

            using (XLWorkbook wb = new XLWorkbook())
            {
                int idx = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    dt.TableName = tnames[idx];
                    wb.Worksheets.Add(dt);
                    idx++;
                }

                //Export the Excel file.
                var Response = HttpContext.Current.Response;
                string filename = Regex.Replace(model.DateRange, @"\s+", "");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "" + filename + ".xls"));
                //Response.AddHeader("content-disposition", "attachment;filename="+filename+".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    try
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                    }
                    catch { }
                    Response.Flush();
                    //Response.Clear();
                    //Response.ClearContent();
                    Response.End();

                }
            }
        }

        #endregion

        #region GetDataSet
        public DataSet GetDataSet(string FilePath, out bool isError)
        {
            string extension = Path.GetExtension(FilePath);

            if (extension.ToLower().Equals(".xls") || extension.ToLower().Equals(".xlsx"))
            {
                return GetDataSetFromExcel(FilePath, true, out isError);
            }
            else if (extension.ToLower().Equals(".csv"))
            {
                return GetDataSetFromCSV(FilePath, out isError);
            }
            else
            {
                isError = true;
                return null;
            }
        }
        #endregion

        #region GetDataSetFromExcel
        public DataSet GetDataSetFromExcel(string FilePath, bool multiple, out bool isError)
        {
            isError = false;

            string ConnectonString = "";
            DataSet ds = new DataSet();
            if (Path.GetExtension(FilePath) == ".xls")
            {
                ConnectonString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + FilePath + "; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (Path.GetExtension(FilePath) == ".xlsx")
            {
                ConnectonString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + "; Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;';";
            }

            try
            {
                using (OleDbConnection excelConnection = new OleDbConnection(ConnectonString))
                {
                    //reading Excel Sheet Name
                    DataTable dt;
                    excelConnection.Open();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    string[] excelSheetNames = new string[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!row["TABLE_NAME"].ToString().Contains("FilterDatabase"))
                        {
                            excelSheetNames[i++] = row["TABLE_NAME"].ToString();
                        }
                        else
                        {
                            Array.Resize<string>(ref excelSheetNames, excelSheetNames.Length - 1);
                        }
                    }
                    //Reading Sheet data
                    foreach (string sheetname in excelSheetNames)
                    {
                        string query = string.Format("Select * from [" + sheetname + "]", excelConnection);
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection);
                        dataAdapter.Fill(ds, sheetname.TrimEnd('$'));
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                isError = true;
                LogWriter.Write(DateTime.Now + " :: Procedure.GetDataSetFromExcel :: Exception :: " + ex.Message.ToString());
            }
            return null;
        }
        #endregion

        #region GetDataSetFromCSV
        public DataSet GetDataSetFromCSV(string FilePath, out bool isError)
        {
            isError = false;
            try
            {
                DataSet ds = new DataSet();
                string connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Text;HDR=Yes;FMT=Delimited;Persist Security Info = False", System.IO.Path.GetDirectoryName(FilePath));
                string constr = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};ExtendedProperties='text;HDR=Yes;FMT=CSVDelimited'", System.IO.Path.GetDirectoryName(FilePath));
                string cmdString = string.Format("SELECT * FROM [" + System.IO.Path.GetFileName(FilePath) + "]");

                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmdString, constr))
                {
                    dataAdapter.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                isError = true;
                LogWriter.Write(DateTime.Now + " :: Procedure.GetDataSetFromCSV :: Exception :: " + ex.Message.ToString());
            }
            return null;
        }

        public DataTable loadCsvFile(string filePath)
        {
            try
            {
                var reader = new StreamReader(File.OpenRead(filePath));
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("base");
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(',');
                    if (i > 0) // skip first row -- header
                    {
                        dataTable.Rows.Add(new object[] { line[0].Replace(";", "").Replace("\"", "") });//need first column only
                    }
                    i = 1;
                }
                return dataTable;
            }
            catch (Exception ex) { LogWriter.Write(DateTime.Now + " :: Procedure.loadCsvFile :: Exception :: " + ex.Message.ToString()); return null; }
        }
        #endregion

        #region Other
        public string repstr(string str)
        {
            return str.Replace("'", " ");
        }
        public string restore(string str)
        {
            return str.Replace(" ", "'");
        }
        #endregion

        //public class ViewModel
        //{

        //    [Display(Name = "Date Range")]
        //    public string DateRange { get; set; }
        //    public string Type { get; set; }
        //    public string TableNames { get; set; }
        //}


        public DataTable ConvertToDataTable(string FilePath, string extension)
        {
            try
            {
                IExcelDataReader reader = null;
                switch (extension)
                {
                    case ".XLSX":
                        reader = ExcelReaderFactory.CreateOpenXmlReader(System.IO.File.OpenRead(FilePath));
                        break;
                    case ".XLS":
                        reader = ExcelReaderFactory.CreateBinaryReader(System.IO.File.OpenRead(FilePath));
                        break;
                    case ".CSV":
                        reader = ExcelReaderFactory.CreateCsvReader(System.IO.File.OpenRead(FilePath));
                        break;
                }

                DataTable FileTable = new DataTable();
                DataRow datarow;
                if (reader != null)
                {
                    DataTable dataTab = reader.AsDataSet().Tables[0];
                    for (int index = 0; index < dataTab.Columns.Count; index++)
                    {
                        FileTable.Columns.Add(dataTab.Rows[0][index].ToString());
                    }
                    for (int index = 1; index < dataTab.Rows.Count; index++)
                    {
                        DataRow drNew = FileTable.NewRow();
                        drNew.ItemArray = dataTab.Rows[index].ItemArray;
                        FileTable.Rows.Add(drNew);
                    }
                }

                return FileTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}