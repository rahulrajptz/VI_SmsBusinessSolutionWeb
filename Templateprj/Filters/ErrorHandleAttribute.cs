using Templateprj.Helpers;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Filters
{
	public class MyErrorHandleAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			LogWriter.Write("Filters.MyErrorHandleAttribute.OnException :: Exception :: " + filterContext.Exception.Message + "\nSource:: " + filterContext.Exception.Source + "\nTargetSite:: " + filterContext.Exception.TargetSite + "\nStackTrace:: " + filterContext.Exception.StackTrace);

			try
			{
				var statusCode = ((HttpException)filterContext.Exception).GetHttpCode();
				if (statusCode == 500)
					filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml", MasterName = "", };

				filterContext.HttpContext.Request.Filter = null;
			}
			catch { }
		}
	}
}