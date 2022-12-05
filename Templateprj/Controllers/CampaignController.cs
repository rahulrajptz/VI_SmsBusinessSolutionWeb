using ExcelDataReader;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.Controllers
{
    //[OutputCache(Duration = 300, VaryByParam = "none")]
    public partial class CampaignController : Controller
    {
        CampaignDb _prc = new CampaignDb();
        MailSender _mailSender = new MailSender();
        CryptoAlg _EncDec = new CryptoAlg();

        #region Campaign Management

        [HttpPost]
        [NoCompress]
        public virtual ActionResult CampaignBase(SMSCampaignModel model)
        {
            string json = "";

            string CampaignId = Request["CampaignId"].ToString();
            if (CampaignId.TrimEnd() == "")
            {
                json = "{\"status\":\"0\",\"response\":\"Please select a campaign\" }";
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            string uploadCampaignstarttype = Request["uploadCampaignstarttype"].ToString();
            string scheduleDate = Request["scheduleDate"].ToString();
            string uploadpriority = Request["uploadpriority"].ToString();
            string ToDate = Request["ToDate"].ToString();
            string Totime = Request["Totime"].ToString();
            string FromDate = Request["FromDate"].ToString();
            string Fromtime = Request["Fromtime"].ToString();
            int variablecnt = Convert.ToInt32(Request["Variablcnt"].ToString());

            if (uploadCampaignstarttype =="2" && scheduleDate != "")
            {
                int dateStatus = checkEndTimeValidity(ToDate + " " + Totime, scheduleDate, false);
                int dateStatusStart = checkEndTimeValidity(scheduleDate, FromDate + " " + Fromtime, false);
                if (dateStatus != 1 || dateStatusStart != 1)
                {
                    json = "{\"status\":\"9\",\"response\":\"The scheduled time must be between From time and To time \"}";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                CultureInfo enUS = new CultureInfo("en-US");
                string currentTime = DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
                dateStatus = checkEndTimeValidity(scheduleDate, currentTime, false);
                if (dateStatus != 1)
                {
                    json = "{\"status\":\"9\",\"response\":\"The scheduled time must be future time \"}";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }

            int pos = 0;
            if (Request.Files.Count > 0)
            {
                ExcelExtension _xls = new ExcelExtension();
                if (!Directory.Exists(GlobalValues.BULKPath))
                    Directory.CreateDirectory(GlobalValues.BULKPath);
                string pathtoMove = GlobalValues.BULKPath + (System.DateTime.Now.ToShortDateString()).Replace("/", "") + "\\";
                if (!Directory.Exists(pathtoMove))
                    Directory.CreateDirectory(pathtoMove);

                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                int cnt = file.FileName.Count(f => f == '.');
                if (cnt <= 1)
                {
                    string xtn = Path.GetExtension(fileName).ToUpper();
                    try
                    {
                        if (xtn == ".XLSX" || xtn == ".CSV" || xtn == ".XLS")
                        {
                            string actualFilename = "upload_" + CampaignId + "_" + (System.DateTime.Now.ToLongTimeString()).Replace(":", "") + xtn.ToLower();
                            pathtoMove = Path.Combine(pathtoMove, actualFilename);
                            pos = 1;
                            file.SaveAs(pathtoMove);
                            pos = 2;
                            bulkFileuploadModel uploadStatus = bulkUpload(pathtoMove, CampaignId, xtn, variablecnt);
                            pos = 3;
                            string response;
                            if (uploadStatus.status == 1)
                            {
                                
                                string status = _prc.insertfilepath(pathtoMove, model, uploadStatus,out response);
                                if (status == "1")
                                {
                                    json = "{\"status\":\"1\",\"response\":\""+response+"\" }";
                                }
                                else
                                {
                                    uploadStatus.message = "Uploading failed";
                                    json = "{\"status\":\"0\",\"response\":\"" + uploadStatus.message + "\" }";
                                    try
                                    {
                                        System.IO.File.Delete(pathtoMove);
                                        System.IO.File.Delete(uploadStatus.SuccessFileName);
                                        System.IO.File.Delete(uploadStatus.failedFileName);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            else if (uploadStatus.status == -1)
                            {
                                json = "{\"status\":\"-1\",\"response\":\"" + uploadStatus.message + "\" }";
                                try
                                {
                                    System.IO.File.Delete(pathtoMove);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else if (uploadStatus.status == 0)
                            {
                                json = "{\"status\":\"-1\",\"response\":\"" + uploadStatus.message + "\" }";
                                string status = _prc.insertfilepath(pathtoMove, model, uploadStatus,out response);

                                try
                                {
                                    System.IO.File.Delete(pathtoMove);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                json = "{\"status\":\"0\",\"response\":\"" + uploadStatus.message + "\" }";
                                try
                                {
                                    System.IO.File.Delete(pathtoMove);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else
                        {
                            json = "{\"status\":\"0\",\"response\":\"Please upload file in .XLSX/.CSV/.XLS format \" }";
                        }
                    }
                    catch (Exception e)
                    {
                        if (pos >= 2)
                        {
                            try
                            {
                                System.IO.File.Delete(pathtoMove);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        json = "{\"status\":\"0\",\"response\":\"Unable to load file\" }";
                        LogWriter.Write(DateTime.Now + " :: Controller.CampaignBase :: Exception :: " + e.Message.ToString());
                    }
                }
                else
                {
                    json = "{\"status\":\"0\",\"response\":\"Input file contain more than one extention , Please Chcek and upload !\" }";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Input file not found !";
                json = "{\"status\":\"0\",\"response\":\"Input file not found!\" }";

            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [NoCompress]
        public virtual ActionResult Campaignupdate(SMSCampaignModel model)
        {
            string json = "";
            string currentTime = DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
            if (checkEndTimeValidity(model.updatedToDate + " " + model.updatedToTime, currentTime, false) != 1)
                return Json("{\"status\":\"0\",\"response\":\"To Time should be greater than Current Time\" }", JsonRequestBehavior.AllowGet);
            string status = _prc.CampaignUpdate(model.updatedToDate, model.updatedToTime, model.updatedId);
            if (status == "1")
            {
                json = "{\"status\":\"1\",\"response\":\"File Successfully Uploaded\" }";
            }
            else
            {
                json = "{\"status\":\"0\",\"response\":\"File not Uploaded\" }";
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public DataTable ConverttoDataTable(string FilePath, string extension)
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

        public bulkFileuploadModel bulkUpload(string filename, string CampaignId, string fileExtension, int variableCount)
        {
            DataTable dt = new DataTable();
            DataTable dtRejected = new DataTable();
            DataTable dtBaseSuccess = new DataTable();
            DataTable dtBasefailure = new DataTable();
            bulkFileuploadModel uploadStatus = new bulkFileuploadModel();
            bulkFileuploadModel bulkInsert = new bulkFileuploadModel();
            bulkInsert.newCampaignId = CampaignId;
            bulkInsert.message = "Error";
            bulkInsert.status = 0;
            string message = "";
            int firstTime = 1;
            dt = ConverttoDataTable(filename, fileExtension);
            int pos = 0, status = 0, packetcnt = 0, baseCount = 0, successCount = 0, failedCount = 0;
            try
            {
                string columnname = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Count - 1 != variableCount)
                    {
                        bulkInsert.status = -9;
                        message = "File content is not match with template";
                        return uploadStatus;
                    }
                    else if (dt.Columns[0].ColumnName != "msisdn")
                    {
                        bulkInsert.status = -9;
                        message = "File content is not match with template";
                        return uploadStatus;
                    }


                    for (int index = 1; index < dt.Columns.Count; index++)
                    {
                        columnname = "col" + index.ToString();
                        try
                        {
                            dt.Columns[index].ColumnName = columnname;
                        }
                        catch (Exception)
                        { }
                    }

                    for (int index = 1; index < dt.Columns.Count; index++)
                    {
                        columnname = "VAR" + index.ToString();
                        try
                        {
                            dt.Columns[index].ColumnName = columnname;

                        }
                        catch (Exception)
                        { }
                    }
                    dt.AcceptChanges();
                    dt.Columns.Add("ROWID");
                    dtBaseSuccess = dt.Clone();
                    dtBasefailure = dt.Clone();
                    dtBaseSuccess.Rows.Clear();
                    dtBasefailure.Rows.Clear();
                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = dtBaseSuccess.Columns["msisdn"];
                    dtBaseSuccess.PrimaryKey = keyColumns;
                    dtBasefailure.Columns.Add("Reason");
                    DataTable dtPartial = dt.Clone();
                    int rowIndex = 0;
                    int rowtoCopy = 10000, currentRow = 0;
                    while (rowIndex < dt.Rows.Count)
                    {
                        pos = 1;
                        dtPartial.Rows.Clear();
                        pos = 2;
                        currentRow = 0;
                        rowtoCopy = 10000;
                        if (dt.Rows.Count - rowIndex < rowtoCopy)
                            rowtoCopy = dt.Rows.Count - rowIndex;
                        pos = 3;
                        DataRow foundRow, dataRow;
                        for (int index = rowIndex; index < rowIndex + rowtoCopy; index++)
                        {
                            if (dt.Rows.Count <= index)
                                break;
                            if (dt.Rows[index].ItemArray[0].ToString().Trim().Length > 5)
                            {
                                foundRow = null;
                                dataRow = null;
                                //   if (d.Trim()))                              

                                if (dtBaseSuccess.Rows.Count >= 1)
                                    foundRow = dtBaseSuccess.Rows.Find(dt.Rows[index].ItemArray[0].ToString());
                                baseCount++;
                                if (foundRow == null)
                                {
                                    pos = 4;
                                    dtPartial.ImportRow(dt.Rows[index]);
                                    pos = 5;
                                    dtPartial.Rows[currentRow]["ROWID"] = index.ToString();
                                    pos = 6;
                                    currentRow++;
                                    pos = 7;
                                    dtBaseSuccess.ImportRow(dt.Rows[index]);
                                    successCount++;
                                }
                                else
                                {
                                    dtBasefailure.ImportRow(dt.Rows[index]);                                   
                                    dtBasefailure.Rows[dtBasefailure.Rows.Count - 1]["Reason"] = "Duplicate";
                                    rowtoCopy++;
                                    failedCount++;
                                }
                            }
                            else
                            {
                                dtBasefailure.ImportRow(dt.Rows[index]);                              
                                dtBasefailure.Rows[dtBasefailure.Rows.Count - 1]["Reason"] = "Too short";
                                baseCount++;
                                rowtoCopy++;
                                failedCount++;
                            }
                        }
                        rowIndex += rowtoCopy;
                        pos = 8;
                        packetcnt++;
                        dtBaseSuccess.AcceptChanges();
                        dtBasefailure.AcceptChanges();
                        bulkInsert = _prc.InsertBulk(dtPartial, bulkInsert.newCampaignId, firstTime);
                        status = bulkInsert.status;
                        if (bulkInsert.status != 1)
                        {
                            message = "Some Error happens";
                            break;
                        }
                        else
                        {
                            message = "success";
                            firstTime = 0;
                        }
                        pos = 9;
                    }
                    if (dtBasefailure.Columns.Contains("ROWID"))
                        dtBasefailure.Columns.Remove("ROWID");
                    if (dtBaseSuccess.Columns.Contains("ROWID"))
                        dtBaseSuccess.Columns.Remove("ROWID");
                    bulkInsert.SuccessFileName = "";
                    bulkInsert.failedFileName = "";
                    if (bulkInsert.status == 1 && dtBaseSuccess.Rows.Count > 0)
                    {
                        bulkInsert.SuccessFileName = GlobalValues.BULKPath + (System.DateTime.Now.ToShortDateString()).Replace("/", "") + "\\" + "SuccessExport_" + bulkInsert.newCampaignId + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".xlsx";
                        using (ExcelPackage pck = new ExcelPackage())
                        {

                            ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add("sheet1");
                            workSheet.Cells["A1"].LoadFromDataTable(dtBaseSuccess, true);
                            pck.SaveAs(new FileInfo(bulkInsert.SuccessFileName));
                        }

                    }
                    if (bulkInsert.status == 1 && dtBasefailure.Rows.Count > 0)
                    {
                        bulkInsert.failedFileName = GlobalValues.BULKPath + (System.DateTime.Now.ToShortDateString()).Replace("/", "") + "\\" + "ExportFailed_" + bulkInsert.newCampaignId + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".xlsx";
                        using (ExcelPackage pck = new ExcelPackage())
                        {

                            ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add("sheet1");
                            workSheet.Cells["A1"].LoadFromDataTable(dtBasefailure, true);
                            pck.SaveAs(new FileInfo(bulkInsert.failedFileName));
                        }
                    }

                }
                else
                {
                    message = "No data in file";
                    bulkInsert.status = -2;
                }
                message = message.Trim();

            }
            catch (Exception ex)
            {
                message = ex.Message;
                bulkInsert.status = -1;
            }
            finally
            {
                uploadStatus.status = bulkInsert.status;
                uploadStatus.message = message;
                uploadStatus.newCampaignId = bulkInsert.newCampaignId;
                uploadStatus.packetcnt = packetcnt;
                uploadStatus.baseCount = baseCount;
                uploadStatus.successCount = successCount;
                uploadStatus.FailureCount = failedCount;
                uploadStatus.failedFileName = bulkInsert.failedFileName;
                uploadStatus.SuccessFileName = bulkInsert.SuccessFileName;
            }
            return uploadStatus;
        }


        [NoCompress]
        public void DownloadsampleFile(string id)
        {

            DataTable dt = _prc.getsamplefilesms(id);
            if (dt != null)
            {
                dt.TableName = "Upload";
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export();
                objExport.ExportDetails(dt, RKLib.ExportData.Export.ExportFormat.CSV, "");
            }

        }

        [NoCompress]
        public virtual ContentResult DownloadCampaignDetailReport(string id)
        {
            DataTable dt = _prc.getCampaignwiseDetailReport(id);
            if (!Directory.Exists(GlobalValues.BULKPath))
              Directory.CreateDirectory(GlobalValues.BULKPath);
              string fileName = GlobalValues.BULKPath + "fileexport_"+ DateTime.Now.ToString("MMddyyyyHHmmss") + ".xlsx";
            using (ExcelPackage pck = new ExcelPackage())
            {

                ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add("report1");
                workSheet.Cells["A1"].LoadFromDataTable(dt, true);
                pck.SaveAs(new FileInfo(fileName));
            }
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);

            //Convert File to Base64 string and send to Client.
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch { }

            return Content(base64);

        }

        #endregion
        [HttpPost]
        public virtual ContentResult DownloadFile(string fileName)
        {
            //Set the File Folder Path.
            string path = fileName;
            //Read the File as Byte Array.
            if (System.IO.File.Exists(path))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                //Convert File to Base64 string and send to Client.
                string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

                return Content(base64);
            }
            else
                return null;
        }
        #region Create Json
        public static string CreateJson(Object ob)
        {
            string json = JsonConvert.SerializeObject(ob);
            return json;
        }


        #endregion

        public virtual ActionResult GetTemplateIdfromSenderId(string senderId, string smstype)
        {
            if (senderId == "")
            {
                senderId = "0";
            }
            if (smstype == "")
            {
                smstype = "0";
            }

            DataTable dt;
            dt = _prc.getTemplateIdFromsenderId(senderId, smstype);

            List<DropDownMaster> list = new List<DropDownMaster>();
            list = (from DataRow row in dt.Rows

                    select new DropDownMaster()
                    {
                        TEXT = row["TEXT"].ToString(),
                        VALUE = row["VALUE"].ToString()
                    }).ToList();
            //list[0].VALUE = "";
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public virtual ActionResult GetSenderIdFromSmsType(string smstype)
        {
            if (smstype == "")
            {
                smstype = "0";
            }

            DataTable dt;
            dt = _prc.getsenderidfromsmstype(smstype);

            List<DropDownMaster> list = new List<DropDownMaster>();
            list = (from DataRow row in dt.Rows

                    select new DropDownMaster()
                    {
                        TEXT = row["TEXT"].ToString(),
                        VALUE = row["VALUE"].ToString()
                    }).ToList();
            //list[0].VALUE = "";
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public virtual ActionResult getcampaignnames()
        {


            DataTable dt;
            dt = _prc.getCampaignNameList();

            List<DropDownMaster> list = new List<DropDownMaster>();
            list = (from DataRow row in dt.Rows

                    select new DropDownMaster()
                    {
                        TEXT = row["TEXT"].ToString(),
                        VALUE = row["VALUE"].ToString()
                    }).ToList();
            //list[0].VALUE = "";
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //Active campaign Names
        public virtual ActionResult getcampaignnamesList()
        {


            DataTable dt;
            dt = _prc.getCampaignName();

            List<DropDownMaster> list = new List<DropDownMaster>();
            list = (from DataRow row in dt.Rows

                    select new DropDownMaster()
                    {
                        TEXT = row["TEXT"].ToString(),
                        VALUE = row["VALUE"].ToString()
                    }).ToList();
            //list[0].VALUE = "";
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        public virtual ActionResult gettemplateNameList()
        {


            DataSet dspopupData = _prc.getTemplateSearchDetails();
            List<DropDownMaster> list = new List<DropDownMaster>();
            if (dspopupData != null && dspopupData.Tables.Count > 0)
            {
                DataRow newRow = dspopupData.Tables[0].NewRow();
                newRow[0] = 0;
                newRow[1] = "-All-";
                dspopupData.Tables[0].Rows.InsertAt(newRow, 0);
                list = (from DataRow row in dspopupData.Tables[0].Rows

                        select new DropDownMaster()
                        {
                            TEXT = row["TEXT"].ToString(),
                            VALUE = row["VALUE"].ToString()
                        }).ToList();
                //list[0].VALUE = "";
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public virtual ActionResult getcampaignSearchReport(string templateName, string templateType,  string ContentType)
        {
            string json = _prc.getcampaignSearchreport(templateName, templateType, ContentType);
            //json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (json!="")
            {
                return Content(json, "application/json");

            }
            
            else
            {
               // Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }
        }

      
        public virtual ActionResult GetmessagecontentfromTemplate(string templateId)
        {
            if (templateId == "")
            {
                templateId = "0";
            }
            string templatestring = _prc.getTemplatebytemplateId(templateId);
            //string json = "[{\"templateContent\": \"Dear  [VAR3] [VAR4] Thank you for your purchasing from MNOP stocks. Invoice No: [VAR5] Invoice Date: [VAR6] Invoice Amount:\",\"variableCount\": \"9\",\"smsLength\": \"150/2\",\"variableNames\": [{\"variable\": \"Var1\"}, {\"variable\": \"Var2\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"}]}]";
            return Json(templatestring, JsonRequestBehavior.AllowGet);

        }
    

        public int checkEndTimeValidity(string endTime, string starttime, bool isCurrentDate)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime endDate, startDate, compDate;
            endDate = DateTime.ParseExact(endTime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            startDate = DateTime.ParseExact(starttime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            if (isCurrentDate)
                compDate = DateTime.Now;
            else
                compDate = startDate;
            //    compDate = DateTime.ParseExact(comparetime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            if (startDate >= endDate)
                return 5;
            else if (isCurrentDate && compDate.AddHours(4) > endDate)
                return 7;
            else
                return 1;
        }

        [HttpPost]
        public virtual ActionResult CreatebulksmsCampaign(SMSCampaignModel model)
        {
            string jsondata = "";
            //int checkStatus = checkEndTimeValidity(model.fromDate + " " + model.fromTime, System.DateTime.Now.ToString("dd/M/yyyy h:mm tt"), false);
            //if (checkStatus != 1)
            //    return Json("{\"status\":\"3\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);
            int status = checkEndTimeValidity(model.toDate + " " + model.toTime, model.fromDate + " " + model.fromTime,true);
            if (status != 1)
                jsondata = "{\"status\":\"" + status + "\",\"response\":\"error\"}";

            else
                jsondata = CreateJson(model);
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public static string GetSingleUnicodeHex(string strTextMsg)
        {
            byte[] s1 = UTF8Encoding.Unicode.GetBytes(strTextMsg);
            string strUnicode = "";
            string strTmp1 = "";
            string strTmp2 = "";

            for (int i = 0; i < s1.Length; i += 2)
            {
                strTmp1 = int.Parse(s1[i + 1].ToString()).ToString("x");
                if (strTmp1.Length == 1)
                    strTmp1 = "0" + strTmp1;

                strTmp2 = int.Parse(s1[i].ToString()).ToString("x");
                if (strTmp2.Length == 1)
                    strTmp2 = "0" + strTmp2;

                strUnicode += strTmp1 + strTmp2;
            }
            return strUnicode;
        }
        public virtual ActionResult createsms(SMSCampaignModel model)
        {

            string messagedetailsjson = "";
            string status = "";
            string jsondata = "";
            string jsontodb = "";

            //int checkStatus = checkEndTimeValidity(model.fromDate + " " + model.fromTime, System.DateTime.Now.ToString("dd/M/yyyy h:mm tt"),  false);
            //if (checkStatus != 1)
            //    return Json("{\"status\":\"3\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);
            int checkStatus = checkEndTimeValidity(model.toDate + " " + model.toTime, model.fromDate + " " + model.fromTime, true);
            if (checkStatus != 1)
                return Json("{\"status\":\"" + checkStatus + "\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);

            if (model.templateTypeName == "STATIC")
            {
                model.SMSTest[0].smsId = "1";
                model.SMSTest[1].smsId = "2";
                model.SMSTest[0].message = model.smsContent;
                model.SMSTest[1].message = model.smsContent;
                model.SMSTest[0].DBMessage = model.smsContent;
                model.SMSTest[1].DBMessage = model.smsContent;
                if (model.unicodeStatus == "8")
                {
                   
                    model.SMSTest[0].DBMessage = GetSingleUnicodeHex(model.SMSTest[0].message);
                    model.SMSTest[1].DBMessage = GetSingleUnicodeHex(model.SMSTest[1].message);
                }
                
                jsondata = CreateJson(model);

                jsontodb = "[" + jsondata + "]";
                status = _prc.getSmscountDetails(jsontodb, out string response);
                if (status == "1")
                {
                    messagedetailsjson = "{\"status\":\"" + status + "\",\"response\":" + response + "}";
                }
                else
                {
                    messagedetailsjson = "{\"status\":\"9\",\"response\":\"error\"}";
                }

            }
            else if (model.templateTypeName == "DYNAMIC")
            {
                model.SMSTest[0].message = model.smsContent;
                model.SMSTest[1].message = model.smsContent;
              

                //string jsondata = CreateJson(model);
                for (int i = 0, j = 1; i < model.SMSTest[0].variableData.Count(); i++, j++)
                {
                    model.SMSTest[0].message = model.SMSTest[0].message.Replace("[VAR" + j + "]", "" + model.SMSTest[0].variableData[i].variableContent + "");
                    model.SMSTest[0].DBMessage = model.SMSTest[0].message;

                  // SMSTestUnicode
                    if (model.unicodeStatus == "8")
                    {
                       //message1 = model.SMSTest[0].message;
                        model.SMSTest[0].DBMessage = GetSingleUnicodeHex(model.SMSTest[0].message);
                    }
                    model.SMSTest[0].smsId = "1";
                }
                for (int i = 0, j = 1; i < model.SMSTest[1].variableData.Count(); i++, j++)
                {
                    model.SMSTest[1].message = model.SMSTest[1].message.Replace("[VAR" + j + "]", "" + model.SMSTest[1].variableData[i].variableContent + "");
                    model.SMSTest[1].DBMessage = model.SMSTest[1].message;
                    if (model.unicodeStatus == "8")
                    {
                       // message2 = model.SMSTest[1].message;
                        model.SMSTest[1].DBMessage = GetSingleUnicodeHex(model.SMSTest[1].message);
                    }
                    model.SMSTest[1].smsId = "2";
                }
                //string str11 = GetSingleUnicodeHex(model.SMSTest[1].message);

                jsondata = CreateJson(model);
                jsontodb = "[" + jsondata + "]";


                status = _prc.getSmscountDetails(jsontodb, out string response);
                
                    if (status == "1")
                { messagedetailsjson = "{\"status\":\"" + status + "\",\"response\":" + response + "}"; }
                else
                {
                    messagedetailsjson = "{\"status\":\"9\",\"response\":\"error\"}";
                }
            }

            return Json(messagedetailsjson, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public virtual ActionResult SendSMS(SMSCampaignModel model)
        {
            //int checkStatus = checkEndTimeValidity(model.fromDate + " " + model.fromTime, System.DateTime.Now.ToString("dd/M/yyyy h:mm tt"),  false);
            //if (checkStatus != 1)
            //    return Json("{\"status\":\"3\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);
             int checkStatus = checkEndTimeValidity(model.toDate + " " + model.toTime, model.fromDate + " " + model.fromTime,true);
            if (checkStatus != 1)
                return Json("{\"status\":\"" + checkStatus + "\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);
            string responsejson = "";
            model.SMSTest[0].DBMessage = model.SMSTest[0].message;
            model.SMSTest[1].DBMessage = model.SMSTest[1].message;
            if (model.unicodeStatus == "8")
           {

               model.SMSTest[0].DBMessage = GetSingleUnicodeHex(model.SMSTest[0].message);
               model.SMSTest[1].DBMessage = GetSingleUnicodeHex(model.SMSTest[1].message);
            }
            string jsondata = CreateJson(model);
            string jsontodb = "[" + jsondata + "]";
            string status = _prc.testsms(jsontodb, out string response, out string campaignid, out string smspushed);
            //model.campaignId = campaignid;
            if (status == "1")
            {
                responsejson = "{\"status\":\"1\",\"response\":\"" + response + "\",\"smsPushed\":\"" + smspushed + "\",\"campaignId\":\"" + campaignid + "\"}";
            }
            else
            {
                responsejson = "{\"status\":\"0\",\"response\":\"" + response + "\",\"smsPushed\":" + smspushed + ",\"campaignId\":\"" + campaignid + "\"}";

            }
            
            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }
        public string ConvertToUnicode(string CodeforConversion)
        {
            try
            {
                byte[] unibyte = Encoding.Unicode.GetBytes(CodeforConversion.Trim());
                string uniString = string.Empty;
                string tmp = string.Empty;
                int i = 0;
                foreach (byte b in unibyte)
                {
                    if (i == 0)
                    {
                        tmp = string.Format("{0}{1}", @"", b.ToString("X2"));
                        i = 1;
                    }
                    else
                    {
                        uniString += string.Format("{0}{1}", @"", b.ToString("X2")) + tmp;
                        i = 0;
                    }
                }
                return uniString;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"CampaignController.ConvertToUnicode::Exception ::{ ex.Message}");
                return "";
            }
        }

        [HttpPost]
        public virtual ActionResult SaveCampaign(SMSCampaignModel model)
        {
            string isUnicode = "";
            string convertedCode = "";
            string responsejson = "";
            //int checkStatus = checkEndTimeValidity(model.fromDate + " " + model.fromTime, System.DateTime.Now.ToString("dd/M/yyyy h:mm tt"),false);
            //if (checkStatus != 1)
            //    return Json("{\"status\":\"3\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);
            int checkStatus = checkEndTimeValidity(model.toDate + " " + model.toTime, model.fromDate + " " + model.fromTime, true);
            if (checkStatus != 1)
                return Json("{\"status\":\"" + checkStatus + "\",\"response\":\"error\"}", JsonRequestBehavior.AllowGet);

            isUnicode = _prc.GetUnicodeStatus(model.templateId);
            if (isUnicode == "1")
            {
                convertedCode = ConvertToUnicode(model.smsContent);
                model.smsContent = convertedCode;
            }

            string jsondata = CreateJson(model);
            string jsontodb = "[" + jsondata + "]";
            string status = _prc.saveCampaign(jsontodb, out string response);
            responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser]
        public virtual ActionResult BulkSms()
        {

            SMSCampaignModel model = new SMSCampaignModel();


            SelectListItem selectListItem = new SelectListItem { Text = "-Select-", Value = "" };
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(selectListItem);


            SelectListItem selectListItemAll = new SelectListItem { Text = "-All-", Value = "0" };
            List<SelectListItem> listItemsAll = new List<SelectListItem>();
            listItemsAll.Add(selectListItemAll);


            DataTable dt = new DataTable();
            DataRow newRow = dt.NewRow();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            newRow[0] = "";
            newRow[1] = "-Select-";
            dt.Rows.InsertAt(newRow, 0);

            DataTable dtSmstype = _prc.getsmstypelist();
            DataTable dtCampaignType = _prc.getCampaigntypelist();
            //DataTable dttemplateIdList = _prc.getTemplateIdFromsenderId();
            //DataTable dtSenderIdList = _prc.getsenderIdlist();
            DataTable dtcmpaignamelist = _prc.getCampaignNameList();
            DataTable dtcampaignstarttype = _prc.getCampaignStarttypelist();
            DataTable dtstatuslist = _prc.getstatuslist();
            DataTable dtstatus = _prc.getstatus();
            DataTable dtprioritylist = _prc.getprioritylist();
            DataSet dspopupData = _prc.getTemplateSearchDetails();



            model.smsTypeList = dtSmstype.ToSelectList(listItems, "VALUE", "TEXT");
            model.campaignTypeList = dtCampaignType.ToSelectList(listItems, "VALUE", "TEXT");
            model.senderIdList = dt.ToSelectList(); //dtSenderIdList.ToSelectList(listItems, "VALUE", "TEXT");
            //model.templateIdList = dttemplateIdList.ToSelectList(listItems, "VALUE", "TEXT");
            model.templateIdList = dt.ToSelectList();

            model.listcampaignNameList = dtcmpaignamelist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.listCampaignStatusList = dtstatus.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.listCampaignPriorityList = dtprioritylist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.listCampaignTypeList = dtCampaignType.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.pouptemplateNameList = dspopupData.Tables[0].ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.pouptemplateTypeList = dspopupData.Tables[1].ToSelectList(listItemsAll, "VALUE", "TEXT");
          //  model.pouptemplateStatusList = dspopupData.Tables[2].ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.poupContentTypeList = dspopupData.Tables[3].ToSelectList(listItemsAll, "VALUE", "TEXT");


            model.uploadCampaignNameList = dtcmpaignamelist.ToSelectList(listItems, "VALUE", "TEXT");
            model.uploadCampaignstarttypeList = dtcampaignstarttype.ToSelectList(listItems, "VALUE", "TEXT");
            model.uploadpriorityList = dtprioritylist.ToSelectList(listItems, "VALUE", "TEXT");


            model.statuscampaignNameList = dtcmpaignamelist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusCampaignStatusList = dtstatus.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusCampaignTypeList = dtCampaignType.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusPriorityList = dtprioritylist.ToSelectList(listItemsAll, "VALUE", "TEXT");

            model.reportcampaignNameList = dtcmpaignamelist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.reportCampaignStatusList = dtstatus.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.reportCampaignPriorityList = dtprioritylist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult BulkSms(SMSCampaignModel model)
        {
            //MSCampaignModel model = new SMSCampaignModel();
            model.campaignTypeList = _prc.getcalltypelist();
            return View(model);
        }

        public virtual ActionResult getcampaigndetailsfromid(string campaignid)
        {
            string jsondata = _prc.getcmapigndetailsfromcampid(campaignid);
            return Json(jsondata, JsonRequestBehavior.AllowGet);

        }

        //changeCampaignStatus
        public virtual ActionResult changeCampaignStatus(string campaignid, string cstatus)
        {


            _prc.changestatuscampign(campaignid, cstatus);
            string jsondata = "";
            return Json(jsondata, JsonRequestBehavior.AllowGet);

        }

        class DropDownMaster
        {
            public string TEXT { get; set; }
            public string VALUE { get; set; }

        }
        
        [HttpPost]
        public virtual ActionResult getcampaigncreatedlist(SMSCampaignModel model)
        {

            //int status = 1;
            string json = _prc.getcampaigncreatedlist(model);
            //string json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (json!="")
            {
                return Content(json, "application/json");
            }
            
            else
            {
               // Response.StatusCode = 503;
                return Content("{\"Error\": \"No data\"}", "application/json");
            }
            
        }

        public void CampaignLisTestReport(string id)
        {

            DataTable dt = _prc.getcampaigntestreportlist(id);
           
            if (dt != null && dt.Rows.Count>0)
            {
                dt.TableName = "TestReport";
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export();
                objExport.ExportDetails(dt, RKLib.ExportData.Export.ExportFormat.Excel, "");
            }
        }
        [HttpPost]
        [AuthorizeUser]

        public virtual ActionResult CampaignLisTestReportNew(string id)
        {
             //int status = 1;
            string json =  _prc.getcampaigntestreportlistNew(id);
            //json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (json != "")
            {
                return Content(json, "application/json");
            }
            
            else
            {
                // Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }
        }
        [HttpPost]
        [AuthorizeUser]

        public virtual ActionResult getcampaignstatusReport(SMSCampaignModel model)
        {


            //int status = 1;
            string json = _prc.getcampaignstatusreport(model);
            //json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (json!="")
            {
                return Content(json, "application/json");
            }
            
            else
            {
               // Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }
        }

        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult getcampaigndetailReport(SMSCampaignModel model)
        {
            //int status = 1;
            string json = _prc.getcampaigndetailreport(model);
            // string json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (json!="")
            {
                return Content(json, "application/json");
            }
            
            else
            {
               // Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }
            
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            char csvDelimiter = ';';
            DataTable dtCSV = new DataTable();
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = null;
                if (path.EndsWith(".csv"))
                {
                    ws = pck.Workbook.Worksheets.Add("Sheet1");
                    ExcelTextFormat format = new ExcelTextFormat()
                    {
                        Delimiter = csvDelimiter
                    };
                    ws.Cells[1, 1].LoadFromText(System.IO.File.ReadAllText(path), format);
                }
                else
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    ws = pck.Workbook.Worksheets.First();
                }


                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }

                string csvRow = "", csvCol = "";
                DataRow dr;
                if (tbl.Rows.Count > 1)
                {
                    csvRow = tbl.Columns[0].ColumnName;
                    string[] csvarray = csvRow.Split(',');
                    for (int indx = 0; indx < csvarray.Length; indx++)
                    {
                        csvCol = csvarray[indx];
                        csvCol = csvCol.Replace("\"", "");
                        dtCSV.Columns.Add(csvCol);
                    }
                    for (int rowIndex = 0; rowIndex < tbl.Rows.Count; rowIndex++)
                    {
                        try
                        {
                            csvRow = tbl.Rows[rowIndex].ItemArray[0].ToString();
                            if (csvRow != "")
                            {
                                dr = dtCSV.NewRow();
                                csvarray = csvRow.Split(',');
                                for (int indx = 0; indx < csvarray.Length; indx++)
                                {
                                    csvCol = csvarray[indx];
                                    csvCol = csvCol.Replace("\"", "");
                                    dr[indx] = csvCol;
                                }

                                dtCSV.Rows.Add(dr);
                            }
                        }
                        catch (Exception)
                        {

                        }


                    }
                }
            }
            return dtCSV;
        }

        public DataTable ExcelToDataTable(string path)
        {
            var pck = new OfficeOpenXml.ExcelPackage();
            pck.Load(System.IO.File.OpenRead(path));
            var ws = pck.Workbook.Worksheets.First();
            DataTable tbl = new DataTable();
            bool hasHeader = true;
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }
            var startRow = hasHeader ? 2 : 1;
            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                var row = tbl.NewRow();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }
                tbl.Rows.Add(row);
            }
            pck.Dispose();
            return tbl;
        }
        
    }
}