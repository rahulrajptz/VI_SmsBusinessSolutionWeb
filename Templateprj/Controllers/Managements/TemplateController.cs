using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
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

        [HttpPost]
        public virtual ActionResult Templates(RegisterTemplateCommand command)
        {
            string status = _templateManagemntRepository.SaveTemplate(command, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public virtual ActionResult Templates(string id)
        {
            string status = _templateManagemntRepository.DeleteTemplate(id, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetTemplateNames()
        {
            return Json(_templateManagemntRepository.TemplateAutoFilItems(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult GetTemplates(TemplateModel model)
        {
            string json = _templateManagemntRepository.GetTemplates(model);
            return Content(json, "application/json");
        }

        [HttpPost]
        [NoCompress]

        public virtual ActionResult UploadTemplate()
        {
            try
            {
                string json = "";
                string CampignId = "TempalteTemp";
                string starttype = "2";

                if (Request.Files.Count > 0)
                {
                    ExcelExtension _xls = new ExcelExtension();
                    string filePath = GlobalValues.TempPath;
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    string path = filePath + "/" + DateTime.Now.ToString("MMddyyyyHHmmss") + "_" + (new Random()).Next();
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var uploadedFile = Request.Files[0];
                    var fileName = Path.GetFileName(uploadedFile.FileName);
                    int cnt = uploadedFile.FileName.Count(f => f == '.');
                    if (cnt <= 1)
                    {

                        string xtn = Path.GetExtension(fileName).ToUpper();
                        try
                        {
                            if (xtn == ".XLS" || xtn == ".XLSX")
                            {
                                path = Path.Combine(path, fileName);
                                uploadedFile.SaveAs(path);

                                var d = _xls.GetDataSetFromExcel(path, false, out bool isError);

                                if (d?.Tables[0]?.Rows != null && d.Tables[0].Rows.Count > 0)
                                {
                                    string data = JsonConvert.SerializeObject(d.Tables[0]);
                                    string status = "1";//_prc.insertfilepath(path, CampignId, starttype);
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
                                    json = "{\"status\":\"0\",\"response\":\"File is empty\" }";

                                }

                                System.IO.File.Delete(path);

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
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}