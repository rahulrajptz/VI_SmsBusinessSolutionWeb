using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.Controllers
{
    //[OutputCache(Duration = 300, VaryByParam = "none")]
    public partial class HomeController : Controller
    {
        CampaignDb _prc = new CampaignDb();
        
        public virtual ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpGet]
        public virtual ActionResult Buttons()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Buttons(string txttest)
        {
            return View();
        }

        public virtual ActionResult ExtraPage()
        {
            return View();
        }

        public virtual ActionResult UnAuthorizesd()
        {
            return View();
        }
        [AuthorizeUser]
        public virtual ActionResult Insights()
        {
            _prc.getInsightDetails(out string smsallow, out string smsDeliv, out string smssub,
                out string success, out string instant, out string apibased, out string campaigns, out string apiinstant);
            ViewBag.DataPoints1 = smsallow;
            ViewBag.DataPoints2 = smsDeliv;
            ViewBag.DataPoints3 = smssub;
            ViewBag.DataPoints4 = success;
            ViewBag.DataPoints5 = instant;
            ViewBag.DataPoints6 = apibased;
            ViewBag.DataPoints7 = campaigns;
            ViewBag.DataPoints8 = apiinstant;

                return View();
        }
       

        [AllowAnonymous]
        public virtual ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}