using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Filters;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.Controllers
{
    public partial class IncidentController : Controller
    {
        // GET: Incident      
        private readonly IncidentDb _incidentPrc = new IncidentDb();
        //private readonly HomeDbPrcs _homePrc = new HomeDbPrcs();
        private readonly ProjectTrackerDb _prj = new ProjectTrackerDb();
        public virtual ActionResult Index()
        {
            Session["PasswordFlag"] = 0;
            if (Session["RoleID"].ToString() == "1")
            {
                return RedirectToAction(MVC.Incident.SystemAlerts());
            }
            else if (Session["RoleID"].ToString() == "2" || Session["RoleID"].ToString() == "4")
            {
                return RedirectToAction(MVC.Incident.IncidentView());
            }
            else if (Session["RoleID"].ToString() == "3")
            {
                return RedirectToAction(MVC.Incident.QrcView());
            }

            return RedirectToAction(MVC.Incident.SystemAlerts());
        }

        #region incident

        private MultiSelectList getWeekDays(int[] selectedIds, MultiSelectList multiSelectList)
        {
            var db = multiSelectList;
            var categories = db.Select(c => new
            {
                VALUE = c.Value,
                TEXT = c.Text
            }).ToList();
            MultiSelectList _weekDays = new MultiSelectList(categories, "VALUE", "TEXT", selectedIds);
            return _weekDays;
        }

        [HttpGet]
        [AuthorizeUser]
        public virtual ActionResult Incidents(string id)
        {
            var Alert = "F";
            var System = "F";
            string[] spliChars = id.Split(',');
            if (spliChars.Length > 1)
            {
                Alert = spliChars[1];
                System = spliChars[2];
            }
            int id1 = Convert.ToInt32(spliChars[0]);
            DataTable dt = new DataTable();
            string selectedSeverity = "";
            string owner = "";
            string status = "";
            int[] customerimpacted = null;
            string location = "";
            string problem = "";
            string impactdes = "";
            string resolution = "";
            string cause = "";
            string action = "";
            string incidenttime = "";
            string resolutiontime = "";
            string pending = "";
            string alertId = id1.ToString();
            string systemId = id1.ToString();

            IncidentModel model = new IncidentModel();
            SelectList lst = _incidentPrc.GetAllCustomer();
            MultiSelectList CusrImpactedList = new MultiSelectList(lst, "VALUE", "TEXT", model.CustomerImpacted);
            if (id1 > 0 && Alert == "F" && System == "F")
            {
                dt = _incidentPrc.GetIncidentDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        IncidentModel models = new IncidentModel();

                        models.IncidentId = id1.ToString();
                        models.AlertId = "0";
                        models.SystemId = "0";
                        models.SeverityList = _incidentPrc.GetAllSeverity();
                        models.OwnerList = _incidentPrc.GetAllOwner();
                        models.StatusList = _incidentPrc.GetAllStatus();
                        models.InsRaisedByList = _incidentPrc.incidentRaisedBy();
                        string customerimpacteds = dt.Rows[0][4].ToString();
                        int[] selectedIds = null;
                        if (customerimpacteds != "")
                        {
                            selectedIds = customerimpacteds.Split(',').Select(n => int.Parse(n)).ToArray();//converting to int array
                        }
                        models.CustomerImpactedList = getWeekDays(selectedIds, CusrImpactedList); //convert to multiselectlist and selected values true
                        models.LocationList = _incidentPrc.GetAllLocations();
                        models.SelectedSeverity = dt.Rows[0][1].ToString();
                        models.Owner = dt.Rows[0][2].ToString();
                        models.Staus = dt.Rows[0][3].ToString();
                        //model.CustomerImpacted = customerimpacted;
                        models.Problem = dt.Rows[0][5].ToString();
                        models.ImpactDes = dt.Rows[0][6].ToString();
                        models.Resolution = dt.Rows[0][7].ToString();
                        models.Cause = dt.Rows[0][8].ToString();
                        models.Action = dt.Rows[0][9].ToString();
                        models.IncidentTime = dt.Rows[0][10].ToString();
                        models.ResolutionTime = dt.Rows[0][11].ToString();
                        models.Location = dt.Rows[0][12].ToString();
                        models.Pending = dt.Rows[0][13].ToString();
                        models.InsRaisedBy = dt.Rows[0][14].ToString();
                        models.IncMailIds = dt.Rows[0][15].ToString();
                        ModelState.Clear();
                        return View(models);
                    }
            }
            else if (id1 > 0 && Alert == "T")
            {
                dt = _incidentPrc.GetFlipDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        //int idflip = id1;
                        IncidentModel models = new IncidentModel();
                        string cuslist = dt.Rows[0]["complaint_by"].ToString();
                        if (cuslist != null)
                        {
                            customerimpacted = cuslist.Split(',').Select(n => int.Parse(n)).ToArray();
                        }
                        models.AlertId = id1.ToString();
                        models.SystemId = "0";
                        models.IncidentId = "0";
                        models.SeverityList = _incidentPrc.GetAllSeverity();
                        models.OwnerList = _incidentPrc.GetAllOwner();
                        models.StatusList = _incidentPrc.GetAllStatus();
                        models.InsRaisedByList = _incidentPrc.incidentRaisedBy();
                        string customerimpacteds = dt.Rows[0][4].ToString();
                        int[] selectedIds = null;
                        if (customerimpacteds != "")
                        {
                            selectedIds = customerimpacteds.Split(',').Select(n => int.Parse(n)).ToArray();//converting to int array
                        }
                        models.CustomerImpactedList = getWeekDays(selectedIds, CusrImpactedList); //convert to multiselectlist and selected values true
                        models.LocationList = _incidentPrc.GetAllLocations();
                        models.SelectedSeverity = dt.Rows[0]["severity"].ToString();
                        models.Owner = "";
                        models.Staus = "";
                        //model.CustomerImpacted = customerimpacted;
                        models.Problem = dt.Rows[0]["Problem"].ToString();
                        models.ImpactDes = dt.Rows[0][6].ToString();
                        models.Resolution = "";
                        models.Cause = "";
                        models.Action = "";
                        models.IncidentTime = dt.Rows[0]["incidenttime"].ToString();
                        models.ResolutionTime = "";
                        models.Location = dt.Rows[0]["location"].ToString();
                        models.Pending = "";

                        ModelState.Clear();
                        return View(models);

                    }
            }
            else if (id1 > 0 && System == "T")
            {
                dt = _incidentPrc.GetSystemIncidentDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {


                        IncidentModel models = new IncidentModel();
                        string cuslist = dt.Rows[0]["customerimpacted"].ToString();
                        if (cuslist != null)
                        {
                            customerimpacted = cuslist.Split(',').Select(n => int.Parse(n)).ToArray();
                        }
                        models.AlertId = "";
                        models.SystemId = id1.ToString();
                        models.IncidentId = "0";
                        models.SeverityList = _incidentPrc.GetAllSeverity();
                        models.OwnerList = _incidentPrc.GetAllOwner();
                        models.StatusList = _incidentPrc.GetAllStatus();
                        models.InsRaisedByList = _incidentPrc.incidentRaisedBy();
                        string customerimpacteds = dt.Rows[0][4].ToString();
                        int[] selectedIds = null;
                        if (customerimpacteds != "")
                        {
                            selectedIds = customerimpacteds.Split(',').Select(n => int.Parse(n)).ToArray();//converting to int array
                        }
                        models.CustomerImpactedList = getWeekDays(selectedIds, CusrImpactedList); //convert to multiselectlist and selected values true
                        models.LocationList = _incidentPrc.GetAllLocations();
                        models.SelectedSeverity = "";
                        models.Owner = "";
                        models.Staus = "";
                        //model.CustomerImpacted = customerimpacted;
                        models.Problem = dt.Rows[0]["problem"].ToString();
                        models.ImpactDes = "";
                        models.Resolution = "";
                        models.Cause = "";
                        models.Action = "";
                        models.IncidentTime = dt.Rows[0]["incidenttime"].ToString();
                        models.ResolutionTime = "";
                        models.Location = dt.Rows[0]["location_id"].ToString();
                        models.Pending = "";

                        ModelState.Clear();
                        return View(models);
                    }
            }

            model.IncidentId = id1.ToString();
            model.AlertId = alertId;
            model.SystemId = systemId;
            model.SeverityList = _incidentPrc.GetAllSeverity();
            model.OwnerList = _incidentPrc.GetAllOwner();
            model.StatusList = _incidentPrc.GetAllStatus();
            model.CustomerImpactedList = CusrImpactedList;
            model.LocationList = _incidentPrc.GetAllLocations();
            model.SelectedSeverity = selectedSeverity;
            model.Owner = owner;
            model.Staus = status;
            model.CustomerImpacted = customerimpacted;
            model.Problem = problem;
            model.ImpactDes = impactdes;
            model.Resolution = resolution;
            model.Cause = cause;
            model.Action = action;
            model.IncidentTime = incidenttime;
            model.ResolutionTime = resolutiontime;
            model.Location = location;
            model.Pending = pending;
            model.InsRaisedByList = _incidentPrc.incidentRaisedBy();


            ModelState.Clear();
            return View(model);
        }


        [HttpPost]        
        public virtual ActionResult Incidents(IncidentModel incident, HttpPostedFileBase fileUpload)
        {
            int mid, status; string mailId, cc;
            incident.Attachment = fileUpload;
            DataTable ldt = new DataTable();
            ldt = _incidentPrc.RegistrationIncident(incident, fileUpload, out mid, out mailId, out cc, out status, out byte[] file);
            if (ldt != null)
                if (ldt.Rows.Count > 0)
                {
                    AccountDbPrcs _prc = new AccountDbPrcs();
                    MailSender _mailSender = new MailSender();

                    Session["Email"] = mailId;

                    string mailBody = _mailSender.ComposeIncidentMail(ldt);
                    if (mailBody.Length > 0)
                    {
                        _mailSender.SendEmailInc(mid, mailId, "INCIDENT COMMUNICATION -(" + ldt.Rows[0]["location"] + ")-(INC#" + ldt.Rows[0]["inc_id"] + ")", mailBody, cc, file, fileUpload);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Authentication Failed!";
                    }
                }
            if (status == 1)
            {
                ViewBag.Status = "1";
                ViewBag.Success = "Added";
            }
            else if (status == 2)
            {
                ViewBag.Status = "1";
                ViewBag.Update = "Updated";
            }
            else
            {
                ViewBag.Status = "1";
                ViewBag.Success = "";
            }
            IncidentModel model = new IncidentModel
            {
                SeverityList = _incidentPrc.GetAllSeverity(),
                OwnerList = _incidentPrc.GetAllOwner(),
                StatusList = _incidentPrc.GetAllStatus(),
                CustomerImpactedList = _incidentPrc.GetAllCustomer(),
                LocationList = _incidentPrc.GetAllLocations(),
                InsRaisedByList = _incidentPrc.incidentRaisedBy()
            };
            ModelState.Clear();
            return View(model);
            //return RedirectToAction(MVC.Incident.Index());

        }

        [NoCompress]        
        public virtual ActionResult IncidentView(ViewModel model, string search, string export, string open, string monthlyreport, string Weektreand, string monthtreand)
        {

            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Type = "s";
                ViewBag.Data = _incidentPrc.GetAllIncidents(model, false, null);
            }
            if (!string.IsNullOrEmpty(export))
            {
                string fileName = "";
                if (model.Type == "s" && model.DateRange != null)
                {
                    fileName = "Report[" + model.DateRange.Replace('-', '.').Trim() + "]";
                    _incidentPrc.GetAllIncidents(model, true, fileName);
                }
                else if (model.Type == "p")
                {
                    fileName = "Report_Open";
                    if (model.DateRange != null)
                        fileName = "Report_Open[" + model.DateRange.Replace('-', '.').Trim() + "]";
                    _incidentPrc.GetAllOpenIncidents(true, fileName);
                }
            }
            if (!string.IsNullOrEmpty(monthlyreport))
            {
                if (model.DateRange != null)
                {
                    DataSet ds = _incidentPrc.MonthlyRep(model);
                    ExcelExtension ex = new ExcelExtension();
                    string Tablename = "CircleWiseIncidents,CategoryWiseIncidents,DowntimeInMinutes";

                    ex.ExportToExcelBySheet(ds, model, Tablename);
                }
            }
            if (!string.IsNullOrEmpty(Weektreand))
            {
                if (model.DateRange != null)
                {
                    DataSet ds = _incidentPrc.Weektrend(model);
                    ExcelExtension ex = new ExcelExtension();
                    string Tablename = "TotalTicketTrend,Prutech,IdeaMLPS,IdeaNWS";

                    ex.ExportToExcelBySheet(ds, model, Tablename);
                }
            }
            if (!string.IsNullOrEmpty(monthtreand))
            {
                if (model.DateRange != null)
                {
                    DataSet ds = _incidentPrc.Monthtrend(model);
                    ExcelExtension ex = new ExcelExtension();
                    string Tablename = "TotalTicketTrend,Prutech,IdeaMLPS,IdeaNWS";

                    ex.ExportToExcelBySheet(ds, model, Tablename);
                }
            }
            if (!string.IsNullOrEmpty(open))
            {
                ViewBag.Type = "p";
                ViewBag.Data = _incidentPrc.GetAllOpenIncidents(false, "");
            }
            return View();
        }

        public List<SelectListItem> getIncRaisedBy()
        {
            List<SelectListItem> IncRaised = new List<SelectListItem>();
            IncRaised.Add(new SelectListItem { Text = "--select--", Value = "" });
            IncRaised.Add(new SelectListItem { Text = "IT Team", Value = "1" });
            IncRaised.Add(new SelectListItem { Text = "Support Team", Value = "2" });
            IncRaised.Add(new SelectListItem { Text = "Development Team", Value = "3" });
            IncRaised.Add(new SelectListItem { Text = "Marketing Team", Value = "4" });
            return IncRaised;
        }

        #endregion

        #region Qrc

        [HttpGet]
        [AuthorizeUser]
        public virtual ActionResult Qrc(String id)
        {
            var Alert = "F";
            var System = "F";
            string[] spliChars = id.Split(',');
            if (spliChars.Length > 1)
            {
                Alert = spliChars[1];
                System = spliChars[2];
            }

            int id1 = Convert.ToInt32(spliChars[0]);
            DataTable dt = new DataTable();
            string complainttype = "";
            string type = "";
            string location = "";
            string cusname = "";
            string status = "";
            string complaintdate = "";
            string closingdate = "";
            string problemstmt = "";
            string rdn = "";
            string rca = "";
            string solution = "";
            string alertId = id1.ToString();
            string systemId = id1.ToString();

            if (id1 > 0 && Alert == "F" && System == "F")
            {
                dt = _incidentPrc.GetQrcDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {

                        complaintdate = dt.Rows[0][1].ToString();
                        complainttype = dt.Rows[0][2].ToString();
                        cusname = dt.Rows[0][3].ToString();
                        type = dt.Rows[0][4].ToString();
                        problemstmt = dt.Rows[0][5].ToString();
                        location = dt.Rows[0][6].ToString();
                        rdn = dt.Rows[0][7].ToString();
                        status = dt.Rows[0][8].ToString();
                        rca = dt.Rows[0][9].ToString();
                        solution = dt.Rows[0][10].ToString();
                        closingdate = dt.Rows[0][11].ToString();
                    }
            }
            else if (id1 > 0 && Alert == "T")
            {
                dt = _incidentPrc.GetFlipDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        systemId = "0";
                        id1 = 0;
                        id = "0";
                        complaintdate = dt.Rows[0]["incidenttime"].ToString();
                        complainttype = dt.Rows[0]["complaintsource"].ToString();
                        cusname = dt.Rows[0]["complaint_by"].ToString();
                        type = "";
                        problemstmt = dt.Rows[0]["Problem"].ToString();
                        location = dt.Rows[0]["location"].ToString();
                        rdn = "";
                        status = "";
                        rca = "";
                        solution = "";
                        closingdate = "";
                    }
            }
            else if (id1 > 0 && System == "T")
            {
                dt = _incidentPrc.GetSystemIncidentDetails(id1);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        alertId = "0";
                        id1 = 0;
                        id = "0";
                        complaintdate = dt.Rows[0]["incidenttime"].ToString();
                        complainttype = dt.Rows[0]["complaintsource"].ToString();
                        cusname = dt.Rows[0]["customerimpacted"].ToString();
                        type = "";
                        problemstmt = dt.Rows[0]["problem"].ToString();
                        location = dt.Rows[0]["location_id"].ToString();
                        rdn = "";
                        status = "";
                        rca = "";
                        solution = "";
                        closingdate = "";

                    }
            }
            QrcModel model = new QrcModel
            {
                QrcId = id1.ToString(),
                AlertId = alertId,
                SystemId = systemId,
                ComplaintTypeList = _incidentPrc.GetAllComplaintTypeQrc(),
                TypeList = _incidentPrc.GetAllTypeQrc(),
                LocationList = _incidentPrc.GetAllLocationsQrc(),
                CusNameList = _incidentPrc.GetAllCusNameQrc(),
                StatusList = _incidentPrc.GetAllComStatusQrc(),
                ComplaintType = complainttype,
                Type = type,
                Location = location,
                CusName = cusname,
                SelectedStatus = status,
                ComplaintDate = complaintdate,
                ClosingDate = closingdate,
                ProblemStmt = problemstmt,
                Rdn = rdn,
                Rca = rca,
                Solution = solution

            };
            return View(model);

        }

        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult Qrc(QrcModel qrc)
        {
            ViewBag.Status = 0;
            ViewBag.Message = "";
            int status = _incidentPrc.RegistrationQrc(qrc);
            ViewBag.Status = status;
            if (status == 1)
                if (qrc.QrcId == "0")
                    ViewBag.Message = "Added Successfully";
                else
                    ViewBag.Message = "Updated Successfully";
            else
            { ViewBag.Message = "Some error occured."; ViewBag.Status = -1; }

            QrcModel model = new QrcModel
            {
                ComplaintTypeList = _incidentPrc.GetAllComplaintTypeQrc(),
                TypeList = _incidentPrc.GetAllTypeQrc(),
                LocationList = _incidentPrc.GetAllLocationsQrc(),
                CusNameList = _incidentPrc.GetAllCusNameQrc(),
                StatusList = _incidentPrc.GetAllComStatusQrc()
            };
            ModelState.Clear();
            //return RedirectToAction(MVC.Incident.Index());
            return View(model);

        }

        [NoCompress]
        [AuthorizeUser]
        public virtual ActionResult QrcView(ViewModel model, string search, string export, string open)
        {

            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Type = "s";
                ViewBag.Data = _incidentPrc.GetAllQrc(model, false, null);
            }
            if (!string.IsNullOrEmpty(export))
            {
                string fileName = "";
                if (model.Type == "s" && model.DateRange != null)
                {
                    fileName = "Report[" + model.DateRange.Replace('-', '.').Trim() + "]";
                    //DataTable dt = _incidentPrc.GetAllQrc(model, true, fileName);
                    _incidentPrc.GetAllQrc(model, true, fileName);
                }
                else if (model.Type == "p")
                {
                    fileName = "Report Open";
                    if (model.DateRange != null)
                    {
                        fileName = "Report_Open[" + model.DateRange.Replace('-', '.').Trim() + "]";
                    }
                    _incidentPrc.GetAllOpenQrc(true, fileName);
                }

            }
            if (!string.IsNullOrEmpty(open))
            {
                ViewBag.Type = "p";
                ViewBag.Data = _incidentPrc.GetAllOpenQrc(false, "");
            }
            return View();
        }

        #endregion

        #region alert     

        public virtual ActionResult FlipkartAlerts(string id)
        {
            int id1 = Convert.ToInt32(id);
            if (id1 > 0)
            {
                bool status = _incidentPrc.DeleteFlipAlerts(id);
                if (status == true)
                {
                    ViewBag.Status = "1";
                    ViewBag.Success = "Deleted";
                }
                else
                {

                    ViewBag.Status = "1";
                    ViewBag.Success = "";
                }
            }
            ViewBag.Data = _incidentPrc.GetAllFlipkartAlerts();
            return View();

        }

        public virtual ActionResult SystemAlerts(string id)
        {

            int id1 = Convert.ToInt32(id);
            if (id1 > 0)
            {
                bool status = _incidentPrc.DeleteSysAlerts(id);
                if (status == true)
                {
                    ViewBag.Status = "1";
                    ViewBag.Success = "Deleted";
                }
                else
                {
                    ViewBag.Status = "1";
                    ViewBag.Success = "";
                }
            }
            ViewBag.Data = _incidentPrc.GetAllSystemAlerts();
            return View();
        }

        #endregion

        #region customer
        [HttpGet]
        public virtual ActionResult Customer(string status)
        {
            if (status == "1")
            {
                ViewBag.OwnerSuccess = "Successful";
            }
            else if (status == "0")
            {
                ViewBag.OwnerError = "Errors";
            }
            CusModel model = new CusModel
            {
                LocationList = _incidentPrc.GetAllLocations()
            };
            return View(model);
        }

        [HttpPost]
        [AuthorizeUser]
        public virtual ActionResult Customer(CusModel model)
        {
            bool status = _incidentPrc.CreateCustomer(model);
            if (status == true)
            {
                ViewBag.Status = "1";
                ViewBag.Success = "Added";
            }
            else
            {
                ViewBag.Status = "1";
                ViewBag.Success = "";
            }
            CusModel modela = new CusModel
            {
                LocationList = _incidentPrc.GetAllLocations()
            };
            ModelState.Clear();//clear the model and delete all fileds except locationlist
            return View(modela);
            // return RedirectToAction(MVC.Incident.Customer());
        }
        #endregion

        #region project tracker

        //Get Details of tfn prj name on 10 digit rdn focus out
        public virtual JsonResult RdnGetDetails(string RdnNum)
        {
            if (!string.IsNullOrEmpty(RdnNum) && RdnNum.Length == 10)
            {
                DataTable dtrdn = _prj.RdnDetails(RdnNum);
                //model.ProjectName = dtrdn.Rows[0][0].ToString();
                //model.TFN = dtrdn.Rows[0][1].ToString();
                //model.RDN = dtrdn.Rows[0][2].ToString();
                //model.Id = dtrdn.Rows[0][3].ToString();
                string json = JsonConvert.SerializeObject(dtrdn, Formatting.Indented);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        [HttpGet]
        public virtual ActionResult ProjectTracker(string id, string RdnNum)
        {
            ProjectTracker model = new ProjectTracker();
            model.CircleList = _prj.Locations();
            model.StatusList = _prj.Status();
            model.RfpStatusList = _prj.RfpStatus();
            model.InternalUatList = _prj.InternalUat();
            model.TypeOfSolutionList = _prj.TypeofSolution();
            model.InternalUatresultList = _prj.InternalUatResultList();

            if (!string.IsNullOrEmpty(id))
            {
                DataTable dt = _prj.GetProjectTrackerById(id);

                model.Id = dt.Rows[0][0].ToString();
                model.RequestFrom = dt.Rows[0][1].ToString();
                model.ProjectName = dt.Rows[0][2].ToString();
                model.RDN = dt.Rows[0][3].ToString();
                model.TFN = dt.Rows[0][4].ToString();
                model.IMSI = dt.Rows[0][5].ToString();
                model.Circle = dt.Rows[0][6].ToString();
                model.ProjectRecipientDate = dt.Rows[0][7].ToString();
                model.AppConfigDate = dt.Rows[0][8].ToString();
                model.MSCConfigDate = dt.Rows[0][9].ToString();
                model.ProjectCompletionDate = dt.Rows[0][10].ToString();
                model.RerecipientofRfp = dt.Rows[0][11].ToString();
                model.EIDCommitDate = dt.Rows[0][12].ToString();
                model.InternalUatresult = dt.Rows[0][13].ToString();
                //model.EIDCommitToClient = dt.Rows[0][13].ToString();
                //model.SolutionConfirmtoIdea = dt.Rows[0][14].ToString();
                model.DateWelcomeEmail = dt.Rows[0][14].ToString();
                model.CustomerSpoc = dt.Rows[0][15].ToString();
                model.CustomerEmail = dt.Rows[0][16].ToString();
                model.CustomerNumber = dt.Rows[0][17].ToString();
                model.IdeaSpoc = dt.Rows[0][18].ToString();
                model.InternalUat = dt.Rows[0][19].ToString();
                model.TypeOfSolution = dt.Rows[0][20].ToString();
                model.RfpStatus = dt.Rows[0][21].ToString();
                model.Remarks = dt.Rows[0][22].ToString();
                model.Status = dt.Rows[0][23].ToString();
                model.OtherRemarks = dt.Rows[0][24].ToString();
                return View(model);
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        [HandleError]
        public virtual ActionResult ProjectTracker(ProjectTracker model)
        {
            model.CircleList = _prj.Locations();
            model.StatusList = _prj.Status();
            model.RfpStatusList = _prj.RfpStatus();
            model.InternalUatList = _prj.InternalUat();
            model.TypeOfSolutionList = _prj.TypeofSolution();
            model.InternalUatresultList = _prj.InternalUatResultList();
            int status = _prj.RegistrationProjectTracker(model);
            if (status == 1)
            {
                ProjectTracker models = new ProjectTracker
                {
                    CircleList = _prj.Locations(),
                    StatusList = _prj.Status(),
                    RfpStatusList = _prj.RfpStatus(),
                    InternalUatList = _prj.InternalUat(),
                    TypeOfSolutionList = _prj.TypeofSolution(),
                    InternalUatresultList = _prj.InternalUatResultList()
                };
                ViewBag.Success = "1";
                ModelState.Clear();
                return View(models);
            }
            ViewBag.Failed = "2";
            return View(model);
        }

        [HandleError]
        [NoCompress]
        public virtual ActionResult ProjectTrackerView(ProjectViewModel model, string search, string export, string open)
        {
            model.CircleListall = _prj.LocationsProject();
            string filename = "";
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Data = _prj.ProjectTrackerView(model, false, filename);
            }
            else if (!string.IsNullOrEmpty(export))
            {
                filename = "ProjectReport[" + model.Daterange + "]";
                _prj.ProjectTrackerView(model, true, filename);
            }
            return View(model);
        }

        public virtual JsonResult RdnAutofill(string Rdn, string tfn)
        {
            List<string> lt = new List<string>();
            DataTable rdnlist = _prj.GetRdnsAutoComp();
            DataTable tfnlist = _prj.GetTfnsAutoComp();
            //var searchList = new List<string> { Rdn };//give search parameter as list
            var Countries = (dynamic)null;
            if (rdnlist != null && Rdn != null)
            {
                if (rdnlist.Rows.Count > 0)
                {
                    for (int i = 0; i < rdnlist.Rows.Count; i++)
                    {
                        lt.Add(rdnlist.Rows[i]["Text"].ToString());
                    }
                    Countries = from name in lt
                                where name.StartsWith(Rdn)
                                select name;
                }
            }
            if (tfnlist != null && tfn != null)
            {
                if (tfnlist.Rows.Count > 0)
                {
                    for (int i = 0; i < tfnlist.Rows.Count; i++)
                    {
                        lt.Add(tfnlist.Rows[i]["Text"].ToString());
                    }
                    Countries = from name in lt
                                where name.StartsWith(tfn)
                                select name;
                }
            }

            return Json(Countries, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult IncHistory(string id)
        {

            ViewBag.Datas = _prj.GetHistory(id);
            return PartialView(MVC.Incident.Views._IncHistory);
        }

        [NoCompress]
        public virtual ActionResult DownloadHistory(string id)
        {
            ViewBag.Data = _prj.DownLoadHistory(id);

            //return RedirectToRoute("ProjectTracker");
            return RedirectToRoute("ProjectTrackerView");
        }

        #endregion

        #region Owner

        [HttpPost]
        public virtual ActionResult Owner(CusModel model)
        {
            int status = _incidentPrc.CreateOwner(model);
            if (status == 1)
            {
                return RedirectToAction("Customer", new { status = 1 });
            }
            return RedirectToAction("Customer", new { status = 0 });
        }

        #endregion

    }
}
