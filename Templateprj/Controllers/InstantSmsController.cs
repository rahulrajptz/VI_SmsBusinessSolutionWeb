﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            ViewBag.ItemList = "Computer Shop Item List Page";
            var model = _instantserviceRepo.GetInstantSms();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SendInstatntSms(InstantSmsCommand model)
        {
            string status = _instantserviceRepo.SendInstantSms(model, out string response);

            string responsejson = "{\"status\":\""+status+"\",\"response\":\"" + response + "\"}";
            
            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult GetSmsCount(GetSmsCountQuery query)
        {
            return Json(JsonConvert.SerializeObject(_instantserviceRepo.GetSmsContent(query.SMSContent, query.TemplateId)), JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public virtual ActionResult Instantsmsreport(InstantSmsModel model)
        {


            int status = 1;
            string json = _prc.getinstantsmsreport(model);
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
    }
}