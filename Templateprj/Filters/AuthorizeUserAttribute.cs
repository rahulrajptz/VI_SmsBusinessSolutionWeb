using Templateprj.DataAccess;
using Templateprj.Helpers;
using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Templateprj.Filters
{
	public class AuthorizeUserAttribute : AuthorizeAttribute
	{
		public string RoleID { get; set; }

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			//Skip authorization for 'AllowAnonymous' attribute
			bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
																						|| filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
			if (skipAuthorization)
				return;


			var request = filterContext.HttpContext.Request;
			var response = filterContext.HttpContext.Response;
			var routeData = filterContext.RouteData;
			string currentAction = routeData.GetRequiredString("action").ToLower();
			string currentController = routeData.GetRequiredString("controller").ToLower();

			if (filterContext.HttpContext.Session != null)
			{
				//Not loged in
				if (filterContext.HttpContext.Session.Count == 0 || filterContext.HttpContext.Session["UserID"] == null || filterContext.HttpContext.Session["RoleID"] == null)
				{
					SetLoginRequired(filterContext, request.IsAjaxRequest());
					return;
				}

				//Checking password expired or already loged in
				if (filterContext.HttpContext.Session["PasswordFlag"] != null)
				{
					string passwordFlag = filterContext.HttpContext.Session["PasswordFlag"].ToString();

					if (passwordFlag == "1" && currentAction != "firsttimelogin" && currentController != "account")
					{
						RouteValueDictionary rd = new RouteValueDictionary();
						rd.Add("controller", "Account");
						rd.Add("action", "FirstTimeLogin");
						filterContext.Result = new RedirectToRouteResult("Default", rd);
						return;
					}

					if (passwordFlag == "3" && currentAction != "resetpassword" && currentController != "account")
					{
						RouteValueDictionary rd = new RouteValueDictionary();
						rd.Add("controller", "Account");
						rd.Add("action", "ResetPassword");
						filterContext.Result = new RedirectToRouteResult("Default", rd);
						return;
					}

					if (passwordFlag == "7" && currentAction != "securityquestion" && currentController != "account")
					{
						RouteValueDictionary rd = new RouteValueDictionary();
						rd.Add("controller", "Account");
						rd.Add("action", "SecurityQuestion");
						filterContext.Result = new RedirectToRouteResult("Default", rd);
						return;
					}
				}

				// Checking user has permission to access current page
				if (!string.IsNullOrWhiteSpace(RoleID) && filterContext.HttpContext.Session["RoleID"].ToString() != RoleID)
				{
					string[] roleList = RoleID.Split(',');
					if (Array.IndexOf(roleList, filterContext.HttpContext.Session["RoleID"].ToString()) < 0)
					{
						SetAuthorized(filterContext, request.IsAjaxRequest());
					}
					return;
				}

				if (filterContext.HttpContext.Session["RoleID"].ToString() == "-1" && currentAction != "changepassword" && currentController != "account")
				{
					if (filterContext.HttpContext.Session["LoginID"] != null)
					{
						(new AccountDbPrcs()).Logout(0);
						filterContext.HttpContext.Session.Clear();
					}
					SetLoginRequired(filterContext, request.IsAjaxRequest());

					return;
				}
			}
			else
			{
				LogWriter.Write("Filters.AuthorizeUserAttribute.OnAuthorization :: Error :: Session is null");
				filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml", MasterName = "" };
				return;
			}
		}

		private void SetAuthorized(AuthorizationContext filterContext, bool isAjax)
		{
			filterContext.HttpContext.Response.StatusCode = 403;

			if (isAjax)
			{
				filterContext.Result = new JsonResult
				{
					Data = new
					{
						Error = "Unauthorized",
						LogOnUrl = string.Empty
					},
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			else
				filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Unauthorized.cshtml", MasterName = "" };
		}

		private void SetLoginRequired(AuthorizationContext filterContext, bool isAjax)
		{
			var urlHelper = new UrlHelper(filterContext.RequestContext);
			filterContext.HttpContext.Response.StatusCode = 401;

			if (isAjax)
			{
				filterContext.Result = new JsonResult
				{
					Data = new
					{
						Error = "NotAuthorized",
						LogOnUrl = urlHelper.Action("Login", "Account")
					},
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			else
				filterContext.Result = new ViewResult { ViewName = "~/Views/Account/Login.cshtml", MasterName = "" };
		}


	}
}