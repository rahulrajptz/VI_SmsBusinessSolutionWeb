using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.Controllers
{
    public partial class CampaignController : Controller
    {
        CampaignDb _prc = new CampaignDb();
        MailSender _mailSender = new MailSender();
        CryptoAlg _EncDec = new CryptoAlg();

        #region Campaign Management






        [HttpPost]
        [NoCompress]

        public virtual ActionResult CampaignBase()
        {
            string json = "";

            string CampignId = Request["CampaignId"].ToString();
            string starttype = Request["starttype"].ToString();


            if (Request.Files.Count > 0)
            {
                ExcelExtension _xls = new ExcelExtension();
                if (!Directory.Exists(GlobalValues.BULKPath))
                    Directory.CreateDirectory(GlobalValues.BULKPath);

                string path = GlobalValues.BULKPath + "/" + DateTime.Now.ToString("MMddyyyyHHmmss") + "_" + CampignId;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                int cnt = file.FileName.Count(f => f == '.');
                if (cnt <= 1)
                {

                    string xtn = Path.GetExtension(fileName).ToUpper();
                    try
                    {
                        if (xtn == ".XLS" || xtn == ".XLSX")
                        {
                            path = Path.Combine(path, fileName);
                            file.SaveAs(path);

                            string status = _prc.insertfilepath(path, CampignId, starttype);
                            if (status == "1")
                            {
                                json = "{\"status\":\"1\",\"response\":\"File Successfully Uploaded\" }";
                            }
                            else
                            {
                                json = "{\"status\":\"0\",\"response\":\"File not Uploaded\" }";

                            }
                        }
                        else
                        {
                            json = "{\"status\":\"0\",\"response\":\"Please upload file in .XLS or .XLSX format \" }";
                        }




                    }
                    catch (Exception e)
                    {


                        json = "{\"status\":\"0\",\"response\":\"Unable to load file\" }";
                        LogWriter.Write(DateTime.Now + " :: Controller.CampaignBase :: Exception :: " + e.Message.ToString());
                    }
                }
                else
                {
                    //TempData["UploadBase"] = "2";

                    //TempData["UploadBaseMessage"] = "Input file contain more than one extention , Please Chcek and upload !";
                    json = "{\"status\":\"0\",\"response\":\"Input file contain more than one extention , Please Chcek and upload !\" }";

                    // ViewBag.ErrorMsg = "Input file contain more than one extention , Please Chcek and upload !";
                }


            }
            else
            {

                //TempData["UploadBase"] = "2";

                //TempData["UploadBaseMessage"] = "Input file not found !";
                ViewBag.ErrorMsg = "Input file not found !";
                json = "{\"status\":\"0\",\"response\":\"Input file not found!\" }";

            }
            //return RedirectToAction("CampaignView", "Campaign", new { area = "" });

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [NoCompress]
        public void DownloadsampleFile(string id)
        {

            DataTable dt = _prc.getsamplefilesms(id);


            dt.TableName = "Upload";


            if (dt != null)
            {

                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    wb.Worksheets.Add(dt);

                //    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //    wb.Style.Font.Bold = true;

                //    Response.Clear();
                //    Response.Buffer = true;
                //    Response.Charset = "";
                //    Response.ContentType = "Application/excel";
                //    Response.AddHeader("content-disposition", "attachment;filename= SampleFile.xlsx");

                //    using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //        wb.SaveAs(MyMemoryStream);
                //        MyMemoryStream.WriteTo(Response.OutputStream);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export();
                objExport.ExportDetails(dt, RKLib.ExportData.Export.ExportFormat.Excel, "");
            }

        }







        #endregion




















        //
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
        // 
        public virtual ActionResult GetmessagecontentfromTemplate(string template)
        {
            if (template == "")
            {
                template = "0";
            }
            string templatestring = _prc.getTemplatebytemplateId(template);
            //string json = "[{\"templateContent\": \"Dear  [VAR3] [VAR4] Thank you for your purchasing from MNOP stocks. Invoice No: [VAR5] Invoice Date: [VAR6] Invoice Amount:\",\"variableCount\": \"9\",\"smsLength\": \"150/2\",\"variableNames\": [{\"variable\": \"Var1\"}, {\"variable\": \"Var2\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"},{\"variable\": \"Var3\"}]}]";
            return Json(templatestring, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public virtual ActionResult CreatebulksmsCampaign(SMSCampaignModel model)
        {

            string jsondata = CreateJson(model);

            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public virtual ActionResult createsms(SMSCampaignModel model)
        {


            string messagedetailsjson = "";
            string jsondata = "";
            string jsontodb = "";

            if (model.smsType == 1)
            {
                model.SMSTest[0].smsId = "1";
                model.SMSTest[1].smsId = "2";
                model.SMSTest[0].message = model.smsContent;
                model.SMSTest[1].message = model.smsContent;
                jsondata = CreateJson(model);

                jsontodb = "[" + jsondata + "]";
                messagedetailsjson = _prc.getSmscountDetails(jsontodb);


            }
            else if (model.smsType == 2)
            {
                model.SMSTest[0].message = model.smsContent;
                model.SMSTest[1].message = model.smsContent;

                //string jsondata = CreateJson(model);
                for (int i = 0, j = 1; i < model.SMSTest[0].variableData.Count(); i++, j++)
                {
                    model.SMSTest[0].message = model.SMSTest[0].message.Replace("[VAR" + j + "]", "" + model.SMSTest[0].variableData[i].variableContent + "");
                    model.SMSTest[0].smsId = "1";
                }
                for (int i = 0, j = 1; i < model.SMSTest[1].variableData.Count(); i++, j++)
                {
                    model.SMSTest[1].message = model.SMSTest[1].message.Replace("[VAR" + j + "]", "" + model.SMSTest[1].variableData[i].variableContent + "");
                    model.SMSTest[1].smsId = "2";
                }
                jsondata = CreateJson(model);
                jsontodb = "[" + jsondata + "]";

                messagedetailsjson = _prc.getSmscountDetails(jsontodb);
            }


            // messagedetailsjson ="{ }";



            return Json(messagedetailsjson, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]

        public virtual ActionResult SendSMS(SMSCampaignModel model)
        {
            string responsejson = "";
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

            //      6666
        }

        [HttpPost]

        public virtual ActionResult SaveCampaign(SMSCampaignModel model)
        {
            string isUnicode = "";
            string convertedCode = "";

            string responsejson = "";
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
            DataTable dtprioritylist = _prc.getprioritylist();




            model.smsTypeList = dtSmstype.ToSelectList(listItems, "VALUE", "TEXT");
            model.campaignTypeList = dtCampaignType.ToSelectList(listItems, "VALUE", "TEXT");
            model.senderIdList = dt.ToSelectList(); //dtSenderIdList.ToSelectList(listItems, "VALUE", "TEXT");
            //model.templateIdList = dttemplateIdList.ToSelectList(listItems, "VALUE", "TEXT");
            model.templateIdList = dt.ToSelectList();

            model.listcampaignNameList = dtcmpaignamelist.ToSelectList(listItems, "VALUE", "TEXT");
            model.listCampaignStatusList = dtstatuslist.ToSelectList(listItems, "VALUE", "TEXT");
            model.listCampaignTypeList = dtCampaignType.ToSelectList(listItems, "VALUE", "TEXT");

            model.uploadCampaignNameList = dtcmpaignamelist.ToSelectList(listItems, "VALUE", "TEXT");
            model.uploadCampaignstarttypeList = dtcampaignstarttype.ToSelectList(listItems, "VALUE", "TEXT");
            model.uploadpriorityList = dtprioritylist.ToSelectList(listItems, "VALUE", "TEXT");


            model.statuscampaignNameList = dtcmpaignamelist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusCampaignStatusList = dtstatuslist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusCampaignTypeList = dtCampaignType.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.statusPriorityList = dtprioritylist.ToSelectList(listItems, "VALUE", "TEXT");

            model.reportcampaignNameList = dtcmpaignamelist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.reportCampaignStatusList = dtstatuslist.ToSelectList(listItemsAll, "VALUE", "TEXT");
            model.reportCampaignPriorityList = dtprioritylist.ToSelectList(listItems, "VALUE", "TEXT");


            return View(model);
        }
        [HttpPost]
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
        [AuthorizeUser]

        public virtual ActionResult getcampaigncreatedlist(SMSCampaignModel model)
        {


            int status = 1;
            // string json = _prc.getCountViewFilterString(model);
            string json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (status == 1)
            {
                return Content(json, "application/json");
            }
            else if (status == -2)
            {
                Response.StatusCode = 507;
                return Content("Out of Memory", "text/plain");
            }
            else
            {
                Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }



        }
        [HttpPost]
        [AuthorizeUser]

        public virtual ActionResult getcampaignstatusReport(SMSCampaignModel model)
        {


            int status = 1;
            string json = _prc.getcampaignstatusreport(model);
            //json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (status == 1)
            {
                return Content(json, "application/json");
            }
            else if (status == -2)
            {
                Response.StatusCode = 507;
                return Content("Out of Memory", "text/plain");
            }
            else
            {
                Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }



        }

        [HttpPost]
        [AuthorizeUser]

        public virtual ActionResult getcampaigndetailReport(SMSCampaignModel model)
        {


            int status = 1;
            string json = _prc.getcampaigndetailreport(model);
            // string json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (status == 1)
            {
                return Content(json, "application/json");
            }
            else if (status == -2)
            {
                Response.StatusCode = 507;
                return Content("Out of Memory", "text/plain");
            }
            else
            {
                Response.StatusCode = 503;
                return Content("{\"Error\": \"Service Unavailable\"}", "application/json");
            }
            
        }
        public virtual ActionResult getcampaignReportDownload(SMSCampaignModel model)
        {
            int status = 0;
            try
            {
                string fileName = "";
                fileName = "Detailedrpt" + "_" + DateTime.Now.ToString();
                _prc.getcampaignreportDownload(model, fileName);
            }
            catch (Exception ex)
            {
                LogWriter.Write(DateTime.Now + "::ReportsController::DetailedReport::Exception::" + ex.Message);
            }
            if (status == 1) { return View("DetailedReport", model); }
            else if (status == -2)
            {
                Response.StatusCode = 507;
                return Content("Out of Memory", "text/xlsx");
            }
            else if (status == 9)
            {
                return Content("No data found", "text/xlsx");
            }
            else
            {
                Response.StatusCode = 503;
                return Content("Something went wrong", "text/xlsx");
            }
        }
    }
}