using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Controllers
{
    public partial class TestController : Controller
    {
        // GET: Test
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}