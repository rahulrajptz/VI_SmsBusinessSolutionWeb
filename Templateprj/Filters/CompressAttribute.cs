using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Filters
{
	public class CompressAttribute : ActionFilterAttribute
	{
		public const CompressionMode Compress = CompressionMode.Compress;



		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			bool dontCompress = filterContext.ActionDescriptor.IsDefined(typeof(NoCompressAttribute), true)
																																									|| filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(NoCompressAttribute), true);
			if (dontCompress)
				return;


#if !DEBUG
						HttpRequestBase request = filterContext.HttpContext.Request;
						HttpResponseBase response = filterContext.HttpContext.Response;
						string acceptEncoding = request.Headers["Accept-Encoding"];
						if (acceptEncoding == null)
								return;
						else if (acceptEncoding.ToLower().Contains("gzip"))
						{
								response.Filter = new GZipStream(response.Filter, Compress);
								response.AppendHeader("Content-Encoding", "gzip");
						}
						else if (acceptEncoding.ToLower().Contains("deflate"))
						{
								response.Filter = new DeflateStream(response.Filter, Compress);
								response.AppendHeader("Content-Encoding", "deflate");
						}
#endif
		}
	}
	public class NoCompressAttribute : ActionFilterAttribute
	{
	}
}