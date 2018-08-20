using System;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class NoCacheAttribute : ActionFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (!filterContext.RequestContext.HttpContext.Request.RawUrl.ToLower().Contains("/Content/") && !filterContext.RequestContext.HttpContext.Request.RawUrl.ToLower().Contains("/Scripts/"))
			{
				if (!filterContext.IsChildAction && !(filterContext.Result is FileResult))
				{
					filterContext.HttpContext.Response.Cache.SetExpires(DateTime.MinValue);
					filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
					filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
					filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
					filterContext.HttpContext.Response.Cache.SetNoStore();
					base.OnResultExecuting(filterContext);
				}
			}
		}
	}
}