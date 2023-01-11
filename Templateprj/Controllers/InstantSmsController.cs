using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models.InstantSms;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Controllers
{
    //[OutputCache(Duration = 300, VaryByParam = "none")]
    [RoutePrefix("Campaign")]
    public partial class InstantSmsController : Controller
    {
        readonly IInstantSmsRepository _instantserviceRepo;
        CampaignDb _prc = new CampaignDb();

        public InstantSmsController(IInstantSmsRepository instantserviceRepo)
        {
            _instantserviceRepo = instantserviceRepo;
        }

        [Route("InstantSms")]
        public virtual ActionResult InstantSms()
        {
            ViewBag.ItemList = "Instant SMS";
            var model = _instantserviceRepo.GetInstantSms();
            return View(model);
        }

        public static string hex(string decString)
        {
            byte[] bytes = Encoding.Default.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        [HttpPost]
        public virtual ActionResult SendInstatntSms(InstantSmsCommand model)
        {
            // model.DBMessage=GetSingleUnicodeHex(model.Messages[0].)
            if (model.Variables != null && model.Variables.Count > 0)
            {
                for (int indx = 0; indx < model.Variables.Count; indx++)
                {
                    model.Variables[indx].Value = hex(model.Variables[indx].Value);
                }
            }
            if (model.unicodeStatus == "8")
            {
                for (int index = 0; index < model.Messages.Count; index++)
                {
                    model.Messages[index].DBMessage = GetSingleUnicodeHex(model.Messages[index].Message);
                }
            }
            else
            {
                for (int index = 0; index < model.Messages.Count; index++)
                {
                    model.Messages[index].DBMessage =model.Messages[index].Message;
                }
            }
            string status = _instantserviceRepo.SendInstantSms(model, out string response);

            string responsejson = "{\"status\":\""+status+"\",\"response\":\"" + response + "\"}";
            
            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult GetSmsCount(GetSmsCountQuery query)
        {
            if (query.unicodeStatus == "8")
            {
                query.DBMessage = GetSingleUnicodeHex(query.SMSContent);
                return Json(JsonConvert.SerializeObject(_instantserviceRepo.GetSmsContent(query.DBMessage, query.TemplateId)), JsonRequestBehavior.AllowGet);
            }
            else { 
                return Json(JsonConvert.SerializeObject(_instantserviceRepo.GetSmsContent(query.SMSContent, query.TemplateId)), JsonRequestBehavior.AllowGet);
            }
        }

        
        [HttpPost]
        public virtual ActionResult Instantsmsreport(InstantSmsModel model)
        {
            int status = 1;
            string json = _prc.getinstantsmsreport(model);
            // string json = "{\"thead\": [{\"title\": \"Campaign ID\"}, {\"title\": \"Campaign Name\"}, {\"title\": \"Campaign Type\"}, {\"title\": \"Created Date\"}, {\"title\": \"Start Date & Time\"}, {\"title\": \"From Date\"}, {\"title\": \"To Date\"}, {\"title\": \"From Time\"}, {\"title\": \"To Time\"}, {\"title\": \"Status\"}, {\"title\": \"Upload Base\"}, {\"title\": \"Test  Report\"}],\"tdata\": [[\"7288806665\", \"AP\", \"IMI MOBILES\", \"404071719557642\", \"test\", \"Get\", \"Active\", \"2017-11-15 14:27:24\",\"Normal\", \"Yes\", \"0\", \"CDR Configured\"],[\"9505270111\", \"AP\", \"IMI MOBILES\", \"404071713625143\", \"asd\", \"Get\", \"Active\",\"2018-01-12 14:06:40\", \"Normal\", \"Yes\", \"1\", \"CDR Configured\"]]}";
            if (status == 1)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
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
    }
}