using System.Web.Mvc;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Controllers

{
    public partial class ManagementController : Controller
    {
        private readonly IAccountManagemntRepository _accountManagemntRepository;

        public ManagementController(IAccountManagemntRepository accountManagemntRepository)
        {
            _accountManagemntRepository = accountManagemntRepository;
        }

        public virtual ActionResult Account()
        {
            ViewBag.ItemList = "Computer Shop Item List Page";
            ManagementModel model= _accountManagemntRepository.GetAccount();
       
            return View(model);
        }

        public virtual ActionResult SenderIds()
        {
            ViewBag.ItemList = "Computer Shop Item List Page";
            ManagementModel model = _accountManagemntRepository.GetAccount();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SaveAccount(ManagementModel model)
        {
            string status = _accountManagemntRepository.SaveAccount(model, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

    }
}