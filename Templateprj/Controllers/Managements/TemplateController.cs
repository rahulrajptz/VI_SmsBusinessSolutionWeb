using System.Web.Mvc;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Controllers

{
    public partial class TemplateController : Controller
    {
        private readonly ITemplateManagemntRepository _templateManagemntRepository;

        public TemplateController(ITemplateManagemntRepository templateManagemntRepository)
        {
            _templateManagemntRepository = templateManagemntRepository;
        }

        public virtual ActionResult Templates()
        {
            ViewBag.ItemList = "Computer Shop Item List Page";
            TemplateModel model = _templateManagemntRepository.GetTemplateFilters();
            return View("~/Views/Management/Templates.cshtml", model);
        }

        [HttpGet]
        public virtual JsonResult GetTemplateNames()
        {
            return Json(_templateManagemntRepository.GetTemplateNames(), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public virtual ActionResult Templates(string id)
        {
            string status = _templateManagemntRepository.DeleteTemplate(id, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult GetTemplates(TemplateModel model)
        {
            string json = _templateManagemntRepository.GetTemplates(model);
            return Content(json, "application/json");
        }
    }
}