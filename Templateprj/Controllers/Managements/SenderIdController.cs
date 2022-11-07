using System.Collections.Generic;
using System.Web.Mvc;
using Templateprj.Filters;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Controllers

{
  
    public partial class SenderIdController : Controller
    {
        private readonly ISenderRepository _senderRepository;

        public SenderIdController(ISenderRepository senderRepository)
        {
            _senderRepository = senderRepository;
        }

        public virtual ActionResult SenderIds()
        {
            return View("~/Views/Management/SenderIds.cshtml");
        }

        [AuthorizeUser]
        public virtual ActionResult GetSenders()
        {
            var json = _senderRepository.GetSenderIds();
            return Content(json, "application/json");
        }

        [Route("~/AddSenderId/{id:int?}")]
        public virtual ActionResult AddSenderId(int? id)
        {
            AddSenderModel model = new AddSenderModel();
            if (id.HasValue) { model = _senderRepository.GetSenderIdById(id.Value); }
            ViewBag.IsEdit = id.HasValue;

            return View("~/Views/Management/AddSenderId.cshtml", model);
        }

        [HttpPost]
        public virtual ActionResult SenderIds(AddSenderModel command)
        {
            string status = _senderRepository.SaveSenderId(new List<AddSenderModel>() { command }, out string response, out string data);
            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\",\"data\":" + data + "}";
            TempData["Message"] = response;
            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPut]
        //public virtual ActionResult Templates(UpdateTemplateCommand command)
        //{
        //    string status = _templateManagemntRepository.UpdateTemplate(command, out string response);

        //    string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

        //    return Json(responsejson, JsonRequestBehavior.AllowGet);
        //}

        [HttpDelete]
        public virtual ActionResult SenderIds(int id)
        {
            string status = _senderRepository.DeleteSenderId(id, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public virtual JsonResult GetTemplateNames()
        //{
        //    return Json(_templateManagemntRepository.TemplateAutoFilItems(), JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //[NoCompress]

        //public virtual ActionResult UploadTemplate()
        //{
        //    try
        //    {
        //        string json = "";
        //        string CampignId = "TempalteTemp";
        //        string starttype = "2";

        //        if (Request.Files.Count > 0)
        //        {
        //            ExcelExtension _xls = new ExcelExtension();
        //            string filePath = GlobalValues.TempPath;
        //            if (!Directory.Exists(filePath))
        //                Directory.CreateDirectory(filePath);

        //            string path = filePath + "/" + DateTime.Now.ToString("MMddyyyyHHmmss") + "_" + (new Random()).Next();
        //            if (!Directory.Exists(path))
        //                Directory.CreateDirectory(path);

        //            var uploadedFile = Request.Files[0];
        //            var fileName = Path.GetFileName(uploadedFile.FileName);
        //            int cnt = uploadedFile.FileName.Count(f => f == '.');
        //            if (cnt <= 1)
        //            {

        //                string xtn = Path.GetExtension(fileName).ToUpper();
        //                try
        //                {
        //                    if (xtn == ".XLS" || xtn == ".XLSX")
        //                    {
        //                        path = Path.Combine(path, fileName);
        //                        uploadedFile.SaveAs(path);

        //                        var d = _xls.GetDataSetFromExcel(path, false, out bool isError);

        //                        if (d?.Tables[0]?.Rows != null && d.Tables[0].Rows.Count > 0)
        //                        {
        //                            string data = JsonConvert.SerializeObject(d.Tables[0]);
        //                            var commands = JsonConvert.DeserializeObject<List<RegisterTemplateCommand>>(data);
        //                            string status = _templateManagemntRepository.SaveTemplate(commands, out string response, out string dataduplicate);
        //                            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\",\"data\":" + dataduplicate + "}";
        //                            TempData["Message"] = response;
        //                        }
        //                        else
        //                        {
        //                            json = "{\"status\":\"0\",\"response\":\"File is empty\" }";

        //                        }

        //                        System.IO.File.Delete(path);

        //                    }
        //                    else
        //                    {
        //                        json = "{\"status\":\"0\",\"response\":\"Please upload file in .XLS or .XLSX format \" }";
        //                    }




        //                }
        //                catch (Exception e)
        //                {


        //                    json = "{\"status\":\"0\",\"response\":\"Unable to load file\" }";
        //                    LogWriter.Write(DateTime.Now + " :: Controller.CampaignBase :: Exception :: " + e.Message.ToString());
        //                }
        //            }
        //            else
        //            {
        //                //TempData["UploadBase"] = "2";

        //                //TempData["UploadBaseMessage"] = "Input file contain more than one extention , Please Chcek and upload !";
        //                json = "{\"status\":\"0\",\"response\":\"Input file contain more than one extention , Please Chcek and upload !\" }";

        //                // ViewBag.ErrorMsg = "Input file contain more than one extention , Please Chcek and upload !";
        //            }


        //        }
        //        else
        //        {

        //            //TempData["UploadBase"] = "2";

        //            //TempData["UploadBaseMessage"] = "Input file not found !";
        //            ViewBag.ErrorMsg = "Input file not found !";
        //            json = "{\"status\":\"0\",\"response\":\"Input file not found!\" }";

        //        }
        //        //return RedirectToAction("CampaignView", "Campaign", new { area = "" });

        //        return Json(json, JsonRequestBehavior.AllowGet);
        //    }
        //    catch(Exception ex)
        //    {
        //        ViewBag.Message = "File upload failed!!";
        //        return View();
        //    }
        //}
    }
}