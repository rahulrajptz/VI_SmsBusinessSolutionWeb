using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web;

namespace Templateprj.Controllers
{
    //[OutputCache(Duration = 300, VaryByParam = "none")]
    public partial class ReportsController : Controller
    {
        ReportsDb _prc = new ReportsDb();

        #region Feedback Report
        [AuthorizeUser]
        public virtual ActionResult FeedbackReports()
        {
            feedbackreportmodel model = new feedbackreportmodel();
            model.intervalList = _prc.getinterval();
            model.agentList = _prc.getagent();
            model.reportTypeList = _prc.getReportType();
            model.campaignNameList = _prc.getcampaign();
            return View(model);
        }
        [HttpPost]
        [NoCompress]
        public virtual ActionResult FeedbackReports(feedbackreportmodel model, string Download)
        {
            model.intervalList = _prc.getinterval();
            model.agentList = _prc.getagent();
            model.reportTypeList = _prc.getReportType();
            model.campaignNameList = _prc.getcampaign();
            if (string.IsNullOrEmpty(Download))
            {
                ViewBag.InboundRepData = _prc.getReports(model);


            }
            else
            {
                DataTable dt = _prc.reportDownload(model);
                if (dt == null)
                {
                    ViewBag.Status = "0";
                    ViewBag.Message = "No Data Found to Download";
                }
                else
                {
                    excel_download(dt);

                }

            }

            return View(model);
        }
        #endregion

        [AuthorizeUser]
        public virtual ActionResult viewcustomerfeedbackdetails(string customerId)
        {
            string report = _prc.getfeedback(customerId);
            return Content(report, "application/json");
        }

        public void excel_download(DataTable dt)
        {
            GridView GridView1 = new GridView();
            // DataTable ldt_asset = _prc.GetAssetprodown(Celltext, Headertext, Firstcell, l);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "SummaryReport" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GridView1.GridLines = GridLines.Both;
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = 12;
            GridView1.RenderControl(htmltextwrtter);
            string style = @"<style> td { mso-number-format:\@;} </style>";

            Response.Write(style);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        [HttpGet]
        [AuthorizeUser]
        public virtual ActionResult DetailedReport()
        {
            DeatailedReportModel model = new DeatailedReportModel();
            model.IdList = _prc.GetSenderIdList();
            return View("DetailedReport", model);
        }
        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult DetailedReport(DeatailedReportModel model)
        {
            model.IdList = _prc.GetSenderIdList();
            int status = 0;
            try
            {
                string name = "";
                name = "Detailedrpt" + "_" + DateTime.Now.ToString();
                _prc.ExportDetailedReport(model, name, out  status);
            }
            catch (Exception ex)
            {
                LogWriter.Write(DateTime.Now + "::ReportsController::DetailedReport::Exception::" + ex.Message);
            }
            if (status == 1)
            {
                return View("DetailedReport", model);
            }
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