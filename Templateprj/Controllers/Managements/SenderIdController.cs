using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Controllers

{
    [AuthorizeUser]
    public partial class SenderIdController : Controller
    {
        private readonly ISenderRepository _senderRepository;
        private readonly ITemplateManagemntRepository _templateManagemntRepository;

        public SenderIdController(ISenderRepository senderRepository, ITemplateManagemntRepository templateManagemntRepository)
        {
            _senderRepository = senderRepository;
            _templateManagemntRepository = templateManagemntRepository;
        }

        public virtual ActionResult SenderIds()
        {
            return View("~/Views/Management/SenderIds.cshtml");
        }

       
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
            model.ApprovalStatus = _templateManagemntRepository.GetTemplateFilters(true).Status;
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

        [HttpPut]
        public virtual ActionResult SenderIds(UpdateSenderIdCommand command)
        {
            string status = _senderRepository.UpdateSenderId(command, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public virtual ActionResult SenderIds(int id)
        {
            string status = _senderRepository.DeleteSenderId(id, out string response);

            string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";

            return Json(responsejson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NoCompress]

        public virtual ActionResult UploadSenderIds()
        {
            try
            {
                string json = "";
                string CampignId = "SenderIdTemp";
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

                                var d = _xls.ConvertToDataTable(path, xtn);

                                if (d?.Rows != null && d.Rows.Count > 0)
                                {
                                    string data = d.DataTableToJSONWithStringBuilder();
                                    var commands = JsonConvert.DeserializeObject<List<AddSenderModel>>(data);
                                    string status = _senderRepository.SaveSenderId(commands, out string response, out string dataduplicate);
                                    //string responsejson = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\",\"data\":" + dataduplicate + "}";
                                    //TempData["Message"] = response;

                                    if (string.IsNullOrEmpty(dataduplicate) || dataduplicate.Length <= 3)
                                    {
                                        json = "{\"status\":\"" + status + "\",\"response\":\"" + response + "\"}";
                                        TempData["Message"] = response;
                                    }
                                    else
                                    {
                                        json = "{\"status\":\"" + 2 + "\",\"response\":\"" + response + "\",\"data\":" + dataduplicate + "}";
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
                        json = "{\"status\":\"0\",\"response\":\"Input file contain more than one extention , Please Chcek and upload !\" }";

                    }


                }
                else
                {
                    ViewBag.ErrorMsg = "Input file not found !";
                    json = "{\"status\":\"0\",\"response\":\"Input file not found!\" }";
                }

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }


    }
}