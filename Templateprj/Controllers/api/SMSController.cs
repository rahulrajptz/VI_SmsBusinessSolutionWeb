using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Templateprj.DataAccess;
using Templateprj.Helpers;
using Templateprj.Models;
using Templateprj.Models.ApiModels;
using Templateprj.Services;

namespace Templateprj.Controllers.api
{
    [RoutePrefix("campaign")]
    public class SMSController : ApiController
    {
        private readonly ISmsService _smsService=new SmsService();

       [AllowAnonymous]
        [HttpPost]
        [Route("bulksms/send/{apikey}")]
       // [ActionName("BulkSms")]
        public HttpResponseMessage PostBulkSms(string apiKey,[FromBody]BulkSms bulkSMS)
        {
            APIResponse response = _smsService.ProcessInput(GetClientIp(), apiKey, bulkSMS);
            return GetStringResponse(response, "json");
        }



        private HttpResponseMessage GetStringResponse(APIResponse responseModel, string type)
        {
            var response = "";
            if (type == "xml")
            {

                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(responseModel.GetType());
                    serializer.Serialize(stringwriter, responseModel);
                    response = HttpUtility.HtmlDecode(stringwriter.ToString());
                }
            }
            else if (type == "json") { response = new JavaScriptSerializer().Serialize(responseModel).Replace("\\", ""); }

            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(response)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue($"application/{type}");
            return resp;

        }


        private string GetClientIp(HttpRequestMessage request =null)
        {
            request = request ?? Request;
            string returnIp = "";
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                returnIp = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (HttpContext.Current != null)
            {
                returnIp = HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                returnIp = null;
            }

            return returnIp;
        }

    }
}
