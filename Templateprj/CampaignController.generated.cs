// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Templateprj.Controllers
{
    public partial class CampaignController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CampaignController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected CampaignController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CampaignBase()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CampaignBase);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GetTemplateIdfromSenderId()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetTemplateIdfromSenderId);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GetSenderIdFromSmsType()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetSenderIdFromSmsType);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult getcampaignSearchReport()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaignSearchReport);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GetmessagecontentfromTemplate()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetmessagecontentfromTemplate);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CreatebulksmsCampaign()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreatebulksmsCampaign);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult createsms()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.createsms);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SendSMS()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SendSMS);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SaveCampaign()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveCampaign);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult getcampaigndetailsfromid()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigndetailsfromid);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult changeCampaignStatus()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.changeCampaignStatus);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult getcampaigncreatedlist()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigncreatedlist);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult getcampaignstatusReport()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaignstatusReport);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult getcampaigndetailReport()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigndetailReport);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CampaignController Actions { get { return MVC.Campaign; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Campaign";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Campaign";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string CampaignBase = "CampaignBase";
            public readonly string GetTemplateIdfromSenderId = "GetTemplateIdfromSenderId";
            public readonly string GetSenderIdFromSmsType = "GetSenderIdFromSmsType";
            public readonly string getcampaignnames = "getcampaignnames";
            public readonly string getcampaignSearchReport = "getcampaignSearchReport";
            public readonly string GetmessagecontentfromTemplate = "GetmessagecontentfromTemplate";
            public readonly string CreatebulksmsCampaign = "CreatebulksmsCampaign";
            public readonly string createsms = "createsms";
            public readonly string SendSMS = "SendSMS";
            public readonly string SaveCampaign = "SaveCampaign";
            public readonly string BulkSms = "BulkSms";
            public readonly string getcampaigndetailsfromid = "getcampaigndetailsfromid";
            public readonly string changeCampaignStatus = "changeCampaignStatus";
            public readonly string getcampaigncreatedlist = "getcampaigncreatedlist";
            public readonly string getcampaignstatusReport = "getcampaignstatusReport";
            public readonly string getcampaigndetailReport = "getcampaigndetailReport";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string CampaignBase = "CampaignBase";
            public const string GetTemplateIdfromSenderId = "GetTemplateIdfromSenderId";
            public const string GetSenderIdFromSmsType = "GetSenderIdFromSmsType";
            public const string getcampaignnames = "getcampaignnames";
            public const string getcampaignSearchReport = "getcampaignSearchReport";
            public const string GetmessagecontentfromTemplate = "GetmessagecontentfromTemplate";
            public const string CreatebulksmsCampaign = "CreatebulksmsCampaign";
            public const string createsms = "createsms";
            public const string SendSMS = "SendSMS";
            public const string SaveCampaign = "SaveCampaign";
            public const string BulkSms = "BulkSms";
            public const string getcampaigndetailsfromid = "getcampaigndetailsfromid";
            public const string changeCampaignStatus = "changeCampaignStatus";
            public const string getcampaigncreatedlist = "getcampaigncreatedlist";
            public const string getcampaignstatusReport = "getcampaignstatusReport";
            public const string getcampaigndetailReport = "getcampaigndetailReport";
        }


        static readonly ActionParamsClass_CampaignBase s_params_CampaignBase = new ActionParamsClass_CampaignBase();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CampaignBase CampaignBaseParams { get { return s_params_CampaignBase; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CampaignBase
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_GetTemplateIdfromSenderId s_params_GetTemplateIdfromSenderId = new ActionParamsClass_GetTemplateIdfromSenderId();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetTemplateIdfromSenderId GetTemplateIdfromSenderIdParams { get { return s_params_GetTemplateIdfromSenderId; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetTemplateIdfromSenderId
        {
            public readonly string senderId = "senderId";
            public readonly string smstype = "smstype";
        }
        static readonly ActionParamsClass_GetSenderIdFromSmsType s_params_GetSenderIdFromSmsType = new ActionParamsClass_GetSenderIdFromSmsType();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetSenderIdFromSmsType GetSenderIdFromSmsTypeParams { get { return s_params_GetSenderIdFromSmsType; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetSenderIdFromSmsType
        {
            public readonly string smstype = "smstype";
        }
        static readonly ActionParamsClass_getcampaignSearchReport s_params_getcampaignSearchReport = new ActionParamsClass_getcampaignSearchReport();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_getcampaignSearchReport getcampaignSearchReportParams { get { return s_params_getcampaignSearchReport; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_getcampaignSearchReport
        {
            public readonly string templateName = "templateName";
            public readonly string templateType = "templateType";
            public readonly string templateStatus = "templateStatus";
            public readonly string ContentType = "ContentType";
        }
        static readonly ActionParamsClass_GetmessagecontentfromTemplate s_params_GetmessagecontentfromTemplate = new ActionParamsClass_GetmessagecontentfromTemplate();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetmessagecontentfromTemplate GetmessagecontentfromTemplateParams { get { return s_params_GetmessagecontentfromTemplate; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetmessagecontentfromTemplate
        {
            public readonly string templateId = "templateId";
        }
        static readonly ActionParamsClass_CreatebulksmsCampaign s_params_CreatebulksmsCampaign = new ActionParamsClass_CreatebulksmsCampaign();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CreatebulksmsCampaign CreatebulksmsCampaignParams { get { return s_params_CreatebulksmsCampaign; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CreatebulksmsCampaign
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_createsms s_params_createsms = new ActionParamsClass_createsms();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_createsms createsmsParams { get { return s_params_createsms; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_createsms
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_SendSMS s_params_SendSMS = new ActionParamsClass_SendSMS();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SendSMS SendSMSParams { get { return s_params_SendSMS; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SendSMS
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_SaveCampaign s_params_SaveCampaign = new ActionParamsClass_SaveCampaign();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveCampaign SaveCampaignParams { get { return s_params_SaveCampaign; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveCampaign
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_BulkSms s_params_BulkSms = new ActionParamsClass_BulkSms();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_BulkSms BulkSmsParams { get { return s_params_BulkSms; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_BulkSms
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_getcampaigndetailsfromid s_params_getcampaigndetailsfromid = new ActionParamsClass_getcampaigndetailsfromid();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_getcampaigndetailsfromid getcampaigndetailsfromidParams { get { return s_params_getcampaigndetailsfromid; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_getcampaigndetailsfromid
        {
            public readonly string campaignid = "campaignid";
        }
        static readonly ActionParamsClass_changeCampaignStatus s_params_changeCampaignStatus = new ActionParamsClass_changeCampaignStatus();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_changeCampaignStatus changeCampaignStatusParams { get { return s_params_changeCampaignStatus; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_changeCampaignStatus
        {
            public readonly string campaignid = "campaignid";
            public readonly string cstatus = "cstatus";
        }
        static readonly ActionParamsClass_getcampaigncreatedlist s_params_getcampaigncreatedlist = new ActionParamsClass_getcampaigncreatedlist();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_getcampaigncreatedlist getcampaigncreatedlistParams { get { return s_params_getcampaigncreatedlist; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_getcampaigncreatedlist
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_getcampaignstatusReport s_params_getcampaignstatusReport = new ActionParamsClass_getcampaignstatusReport();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_getcampaignstatusReport getcampaignstatusReportParams { get { return s_params_getcampaignstatusReport; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_getcampaignstatusReport
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_getcampaigndetailReport s_params_getcampaigndetailReport = new ActionParamsClass_getcampaigndetailReport();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_getcampaigndetailReport getcampaigndetailReportParams { get { return s_params_getcampaigndetailReport; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_getcampaigndetailReport
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _dynamicFields = "_dynamicFields";
                public readonly string _MultipleChoice = "_MultipleChoice";
                public readonly string _Question = "_Question";
                public readonly string AgentManagement = "AgentManagement";
                public readonly string BulkAddAgent = "BulkAddAgent";
                public readonly string BulkSms = "BulkSms";
                public readonly string CampaignBaseUpload = "CampaignBaseUpload";
                public readonly string CampaignView = "CampaignView";
                public readonly string CreateCampaign = "CreateCampaign";
                public readonly string CreateCampaignNew = "CreateCampaignNew";
                public readonly string NewCreateCampaign = "NewCreateCampaign";
            }
            public readonly string _dynamicFields = "~/Views/Campaign/_dynamicFields.cshtml";
            public readonly string _MultipleChoice = "~/Views/Campaign/_MultipleChoice.cshtml";
            public readonly string _Question = "~/Views/Campaign/_Question.cshtml";
            public readonly string AgentManagement = "~/Views/Campaign/AgentManagement.cshtml";
            public readonly string BulkAddAgent = "~/Views/Campaign/BulkAddAgent.cshtml";
            public readonly string BulkSms = "~/Views/Campaign/BulkSms.cshtml";
            public readonly string CampaignBaseUpload = "~/Views/Campaign/CampaignBaseUpload.cshtml";
            public readonly string CampaignView = "~/Views/Campaign/CampaignView.cshtml";
            public readonly string CreateCampaign = "~/Views/Campaign/CreateCampaign.cshtml";
            public readonly string CreateCampaignNew = "~/Views/Campaign/CreateCampaignNew.cshtml";
            public readonly string NewCreateCampaign = "~/Views/Campaign/NewCreateCampaign.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_CampaignController : Templateprj.Controllers.CampaignController
    {
        public T4MVC_CampaignController() : base(Dummy.Instance) { }

        [NonAction]
        partial void CampaignBaseOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult CampaignBase(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CampaignBase);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CampaignBaseOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void GetTemplateIdfromSenderIdOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string senderId, string smstype);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetTemplateIdfromSenderId(string senderId, string smstype)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetTemplateIdfromSenderId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "senderId", senderId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "smstype", smstype);
            GetTemplateIdfromSenderIdOverride(callInfo, senderId, smstype);
            return callInfo;
        }

        [NonAction]
        partial void GetSenderIdFromSmsTypeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string smstype);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetSenderIdFromSmsType(string smstype)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetSenderIdFromSmsType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "smstype", smstype);
            GetSenderIdFromSmsTypeOverride(callInfo, smstype);
            return callInfo;
        }

        [NonAction]
        partial void getcampaignnamesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaignnames()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaignnames);
            getcampaignnamesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void getcampaignSearchReportOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string templateName, string templateType, string templateStatus, string ContentType);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaignSearchReport(string templateName, string templateType, string templateStatus, string ContentType)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaignSearchReport);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "templateName", templateName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "templateType", templateType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "templateStatus", templateStatus);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ContentType", ContentType);
            getcampaignSearchReportOverride(callInfo, templateName, templateType, templateStatus, ContentType);
            return callInfo;
        }

        [NonAction]
        partial void GetmessagecontentfromTemplateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string templateId);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetmessagecontentfromTemplate(string templateId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetmessagecontentfromTemplate);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "templateId", templateId);
            GetmessagecontentfromTemplateOverride(callInfo, templateId);
            return callInfo;
        }

        [NonAction]
        partial void CreatebulksmsCampaignOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult CreatebulksmsCampaign(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreatebulksmsCampaign);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CreatebulksmsCampaignOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void createsmsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult createsms(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.createsms);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            createsmsOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void SendSMSOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult SendSMS(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SendSMS);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SendSMSOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void SaveCampaignOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult SaveCampaign(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveCampaign);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SaveCampaignOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void BulkSmsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult BulkSms()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BulkSms);
            BulkSmsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void BulkSmsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult BulkSms(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BulkSms);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            BulkSmsOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void getcampaigndetailsfromidOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string campaignid);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaigndetailsfromid(string campaignid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigndetailsfromid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "campaignid", campaignid);
            getcampaigndetailsfromidOverride(callInfo, campaignid);
            return callInfo;
        }

        [NonAction]
        partial void changeCampaignStatusOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string campaignid, string cstatus);

        [NonAction]
        public override System.Web.Mvc.ActionResult changeCampaignStatus(string campaignid, string cstatus)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.changeCampaignStatus);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "campaignid", campaignid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "cstatus", cstatus);
            changeCampaignStatusOverride(callInfo, campaignid, cstatus);
            return callInfo;
        }

        [NonAction]
        partial void getcampaigncreatedlistOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaigncreatedlist(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigncreatedlist);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            getcampaigncreatedlistOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void getcampaignstatusReportOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaignstatusReport(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaignstatusReport);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            getcampaignstatusReportOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void getcampaigndetailReportOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Templateprj.Models.SMSCampaignModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult getcampaigndetailReport(Templateprj.Models.SMSCampaignModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.getcampaigndetailReport);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            getcampaigndetailReportOverride(callInfo, model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
