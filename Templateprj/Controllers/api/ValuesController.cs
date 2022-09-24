using System;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Templateprj.DataAccess;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.Controllers.api
{
    [RoutePrefix("Values")]
    public class ValuesController : ApiController
    {
        //private readonly MailSender _prcs = new MailSender1();
        private readonly MailSender _prcs = new MailSender();

        ApiDbprc _prc = new ApiDbprc();
        //AccountDbPrcs _prcs = new AccountDbPrcs();
        //[ActionName("CreateCampaign")]
        [Route("CreateCampaign/{apikey}/{type}")]
        public HttpResponseMessage PostCreateCampaign(string apikey, string type, Campaign campaign)
        {
            //Campaign campaign = new Campaign();
            string campId = "";
            string convertedCode = "";
            string isUnicode;
            APIResponse rtnmodel = new APIResponse();
            if (IsIPValid(apikey.Trim(), ""))
            {
                if (Validatecampaign(campaign, out rtnmodel))
                {
                    isUnicode = _prc.GetUnicodeStatus(campaign.templateid);
                    if (isUnicode == "1")
                    {
                        convertedCode = ConvertToUnicode(campaign.script);
                    }
                    string sts = _prc.CreateCampign(apikey, convertedCode, isUnicode, campaign, out string response, out campId);
                    if (sts == "1")
                    {
                        rtnmodel.Status = "Success";
                        rtnmodel.Message = response;
                        rtnmodel.CampaignId = campId;


                    }
                    else
                    {
                        rtnmodel.Status = "Failed";
                        rtnmodel.Message = response;
                        rtnmodel.CampaignId = "";
                    }
                }
            }
            else
            {
                rtnmodel.Status = "failed";
                rtnmodel.Message = "Authentication failed!";
                rtnmodel.CampaignId = "";

            }
            // return GetStringResponse(rtnmodel, type.ToLower().Trim());
            return GetStringResponse(rtnmodel, "json");
        }


        private bool Validatecampaign(Campaign campaign, out APIResponse aPIResponse)
        {
            bool rtnSts = true;
            aPIResponse = new APIResponse();
            //string formatString = "yyyy-MM-dd";
            string formatString = "dd/mm/yyyy";
            DateTime resultdt;
            if (string.IsNullOrEmpty(campaign.campname) || string.IsNullOrEmpty(campaign.fromdt)
                || string.IsNullOrEmpty(campaign.todt) || string.IsNullOrEmpty(campaign.fromtime)
                || string.IsNullOrEmpty(campaign.totime) || string.IsNullOrEmpty(campaign.senderid)
                || string.IsNullOrEmpty(campaign.templateid)
                || string.IsNullOrEmpty(campaign.type) || string.IsNullOrEmpty(campaign.script))

            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Some required field values missing";
                aPIResponse.CampaignId = "";
            }
            else if (!DateTime.TryParseExact(campaign.fromdt, formatString, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out resultdt))
            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Invalid date";
                aPIResponse.CampaignId = "";
            }
            else if (!DateTime.TryParseExact(campaign.todt, formatString, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out resultdt))
            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Invalid date";
                aPIResponse.CampaignId = "";
            }
            return rtnSts;
        }
        public string ConvertToUnicode(string CodeforConversion)
        {
            try
            {
                byte[] unibyte = Encoding.Unicode.GetBytes(CodeforConversion.Trim());
                string uniString = string.Empty;
                string tmp = string.Empty;
                int i = 0;
                foreach (byte b in unibyte)
                {
                    if (i == 0)
                    {
                        tmp = string.Format("{0}{1}", @"", b.ToString("X2"));
                        i = 1;
                    }
                    else
                    {
                        uniString += string.Format("{0}{1}", @"", b.ToString("X2")) + tmp;
                        i = 0;
                    }
                }
                return uniString;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"ApiController.ConvertToUnicode::Exception ::{ ex.Message}");
                return "";
            }

            //      6666
        }

        [ActionName("InstantSMS")]
        public HttpResponseMessage PostInstantSMS(string apikey, string type, SMSCreate sms)
        {

            string time = DateTime.Now.ToString("yyyyMMddHHmmss.fff");
            LogWriter.Write("start :: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.ffff tt"));
            string isUnicode;
            string convertedCode = "";
            string message = "Error";
            //string res = "";
            //string errorMsisdn = "";
            APIRespnsesms rtnmodel1 = new APIRespnsesms();
            //if (IsIPValid(apikey.Trim(), ""))
            //{
            string IPs = GetClientIp();

            if (ValidateSMS(sms, out rtnmodel1))
            {

                isUnicode = _prc.GetUnicodeStatus(sms.templateid);
                if (isUnicode == "1")
                {
                    convertedCode = ConvertToUnicode(sms.script);
                }
                string sts = _prc.CreateInstantSMS(apikey, isUnicode, convertedCode, out string uniqueid, out message, sms, time, IPs);
                if (sts == "1")
                {
                    rtnmodel1.Status = "Success";
                    rtnmodel1.Message = message;
                    rtnmodel1.uniqueid = uniqueid;

                    // rtnmodel1.invalidMsisdn = errorMsisdn;

                }
                else
                {
                    rtnmodel1.Status = "Failed";
                    rtnmodel1.Message = message;
                    rtnmodel1.uniqueid = "";


                    //rtnmodel1.invalidMsisdn = errorMsisdn;
                }
            }

            //}
            //else
            //{
            //    rtnmodel1.Status = "failed";
            //    rtnmodel1.Message = "Authentication failed!";
            //    rtnmodel1.uniqueid = "";

            //}
            return GetStringResponse(rtnmodel1, type.ToLower().Trim());
        }
        [HttpGet]
        public HttpResponseMessage GetData(string Msisdn, string script, string senderid, string templateid, string smstype, string key)
        {
            string isUnicode;
            string convertedCode = "";
            // string errorMsisdn = "";
            //var queryString = HttpUtility.UrlDecode(ActionContext.Request.RequestUri.Query);
            //string data = queryString.Replace("?", "");
            string time = DateTime.Now.ToString("yyyyMMddHHmmss.fff");

            SMSCreate sms = new SMSCreate();
            sms.msisdn = Msisdn;
            sms.script = script;
            sms.senderid = senderid;
            sms.templateid = templateid;
            sms.smstype = smstype;
            APIRespnsesms rtnmodel1 = new APIRespnsesms();
            string IPs = GetClientIp();

            //if (IsIPValid(key.Trim(), ""))
            //{
            if (ValidateSMS(sms, out rtnmodel1))
            {
                isUnicode = _prc.GetUnicodeStatus(sms.templateid);
                if (isUnicode == "1")
                {
                    convertedCode = ConvertToUnicode(sms.script);
                }
                string sts = _prc.CreateSMS(key, isUnicode, convertedCode, out string uniqueid, out string msg, sms, time, IPs);
                if (sts == "1")
                {
                    rtnmodel1.Status = "Success";
                    rtnmodel1.Message = msg;
                    rtnmodel1.uniqueid = uniqueid;
                    //rtnmodel1.invalidMsisdn = errorMsisdn;


                }
                else
                {
                    rtnmodel1.Status = "Failed";
                    rtnmodel1.Message = msg;
                    rtnmodel1.uniqueid = "";
                    // rtnmodel1.invalidMsisdn = errorMsisdn;

                }
            }
            //}
            //else
            //{
            //    rtnmodel1.Status = "failed";
            //    rtnmodel1.Message = "Authentication failed!";
            //    rtnmodel1.uniqueid = "";

            //}
            return GetStringResponse(rtnmodel1, "json");
        }

        //string[] split(string from, string to, string data)
        //{
        //    string[] ff = new string[2];
        //    while (data != "")
        //    {
        //        int pos = data.IndexOf(to);
        //        string Invite = data.Substring(0, pos);
        //        data = data.Remove(0, pos);
        //        if (Invite.StartsWith(from))
        //        {
        //            // string b = Invite;
        //            ff[0] = Invite;
        //            break;
        //        }
        //        else
        //            ff[0] = "";
        //    }
        //    ff[1] = data;
        //    return ff;
        //}

        private bool ValidateSMS(SMSCreate sms, out APIRespnsesms aPIResponse)
        {
            bool rtnSts = true;
            aPIResponse = new APIRespnsesms();

            if (string.IsNullOrEmpty(sms.msisdn) || string.IsNullOrEmpty(sms.script) || string.IsNullOrEmpty(sms.senderid)
                || string.IsNullOrEmpty(sms.templateid) || string.IsNullOrEmpty(sms.smstype))
            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Some required field values missing";
                aPIResponse.uniqueid = "";

                //aPIResponse.invalidMsisdn = "";


            }
            return rtnSts;
        }
        private bool ValidateSMS12(SMSCreate sms, out APIRespnse aPIResponse)
        {
            bool rtnSts = true;
            aPIResponse = new APIRespnse();

            if (string.IsNullOrEmpty(sms.msisdn) || string.IsNullOrEmpty(sms.script) || string.IsNullOrEmpty(sms.senderid) || string.IsNullOrEmpty(sms.templateid))
            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Some required field values missing";



            }
            return rtnSts;
        }

        [ActionName("UploadMSISDNBase")]
        public HttpResponseMessage PostUploadMSISDNBase(string apikey, string type, MSISDNBase msisdnbase)
        {
            APIResponse rtnmodel = new APIResponse();
            if (CheckIPValidBase(apikey.Trim(), msisdnbase.campaignid))
            {
                int unicode = _prc.UnicodeStatus(msisdnbase.campaignid);
                if (unicode == 1 || unicode == 0)
                {
                    string sts = _prc.UploadBase(apikey, msisdnbase, unicode);
                    if (sts == "1")
                    {
                        rtnmodel.Status = "Success";
                        rtnmodel.Message = "Base Uploaded!";
                        rtnmodel.CampaignId = msisdnbase.campaignid;
                        int sts1 = _prc.UploadedFile(msisdnbase);

                    }
                    else if (sts == "9")
                    {
                        rtnmodel.Status = "Failed";
                        rtnmodel.Message = "Server Unavailable!";
                        rtnmodel.CampaignId = "";
                    }
                    else
                    {
                        rtnmodel.Status = "Failed";
                        rtnmodel.Message = "Some error occurred!";
                        rtnmodel.CampaignId = "";
                    }
                }
                else
                {
                    rtnmodel.Status = "Failed";
                    rtnmodel.Message = "Campaign Not Found to upload Base!";
                    rtnmodel.CampaignId = "";
                }
            }
            else
            {
                rtnmodel.Status = "failed";
                rtnmodel.Message = "Authentication failed!";
                rtnmodel.CampaignId = "";

            }
            return GetStringResponse(rtnmodel, type.ToLower().Trim());
        }



        private bool IsIPValid(string key, string camPId)
        {
            string IPs = GetClientIp();
            // LogWriter.Write("IPs :: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.ffff tt"));
            string sts = _prc.CheckIPValid(key, IPs.Trim());
            // LogWriter.Write("prcexe :: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.ffff tt"));
            return sts == "1" ? true : false;

        }
        private bool CheckIPValidBase(string key, string camPId)
        {

            string sts = _prc.CheckIPValidBase(key, camPId);
            return sts == "1" ? true : false;
        }
        private string GetClientIp(HttpRequestMessage request = null)
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
            //  LogWritter.Write("AccountController : ApiController.GetClientIp :: " + returnIp);

            return returnIp;
        }

        private HttpResponseMessage GetStringResponse(APIRespnseBala responseModel, string type)
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
        private HttpResponseMessage GetStringResponse(APIRespnse responseModel, string type)
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
        private HttpResponseMessage GetStringResponse(APIResponse1 responseModel, string type)
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
        private HttpResponseMessage GetStringResponse(APIRespnsesms responseModel, string type)
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

        private HttpResponseMessage GetStringResponse(string json)
        {
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }


        [HttpGet]
        [ActionName("PostDeliveryStatus")]
        public HttpResponseMessage DeliveryStatus(string mobile, string status, string mid, string deliv_time, string refid)
        {
            string sts = "";
            APIRespnse rtnmodel = new APIRespnse();
            string stsout = _prc.SMSdelivery(mobile, status, mid, deliv_time, refid, out sts);
            if (stsout == "1")
            {
                rtnmodel.Status = "Success";
                rtnmodel.Message = status;
            }
            else
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = "Some error occurred!";
            }
            return GetStringResponse(rtnmodel, "json");
        }
        [ActionName("PostDeliveryFetch")]
        public HttpResponseMessage DeliveryFetch(string ApiKey, DeliveryFetch deliveryfetch)
        {
            //string status = "";
            APIRespnse rtnmodel = new APIRespnse();
            DataSet s = _prc.SMSdelivery1(ApiKey, deliveryfetch);
            string postData = s.Tables[0].Rows[0].ItemArray[0].ToString();
            if (postData == "")
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = "enter valid UniqueID!";
                return GetStringResponse(rtnmodel, "json");
            }
            else if (postData == "-1")
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = "Some error occurred!";
                return GetStringResponse(rtnmodel, "json");
            }
            else
            {
                //rtnmodel.Status = "Failed";
                //rtnmodel.Message = "Some error occurred!";
                return GetStringResponse(postData);
            }

        }
        [ActionName("Smstemplatefetch")]
        public HttpResponseMessage Templatefetch(string ApiKey, SMSTemplateFetch sMSTemplateFetch)
        {
            //string status = "";
            APIRespnse rtnmodel = new APIRespnse();
            DataSet s = _prc.TemplateFetch(ApiKey, sMSTemplateFetch);
            string postData = s.Tables[0].Rows[0].ItemArray[0].ToString();
            if (postData == "")
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = "enter valid templateid!";
                return GetStringResponse(rtnmodel, "json");
            }
            else if (postData == "-1")
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = "Some error occurred!";
                return GetStringResponse(rtnmodel, "json");
            }
            else
            {
                //rtnmodel.Status = "Failed";
                //rtnmodel.Message = "Some error occurred!";
                return GetStringResponse(postData);
            }

        }
        [ActionName("CreateSenderID")]
        public HttpResponseMessage PostCreateSenderID(string ApiKey, Senderid Senderid)
        {
            //string status = "";
            APIResponsesenderid rtnmodel = new APIResponsesenderid();
            string sts = _prc.CreateSenderid(ApiKey, Senderid, out string mail, out string compnyname, out string msg,out string refid);
            string[] mailsplits = mail.Split(',');
            if (sts == "1")
            {
                //sendmailSenderId(mailsplits, compnyname);
                rtnmodel.Status = "Success";
                rtnmodel.Message = msg;
                rtnmodel.RefId = refid;
                return GetStringResponse(rtnmodel, "json");
            }
            else
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Message = msg;
                rtnmodel.RefId = refid;

                return GetStringResponse(rtnmodel, "json");
            }

        }
        private HttpResponseMessage GetStringResponse(APIResponsesenderid responseModel, string type)
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

        [ActionName("TemplateAPI")]
        public HttpResponseMessage PostInstantSMS1(string ApiKey, script1 sms)
        {
            string isUnicode = "";
            string convertedCode = "";
            string sts = "";
            string Mailid = "";
            //string stsmsg = "";
            APIRespnse rtnmodel1 = new APIRespnse();
            APIResponse1 retnmodel = new APIResponse1();
            //  if (IsIPValid(apikey.Trim(), ""))
            //{
            if (ValidateScript(sms, out rtnmodel1))
            {

                if (sms.Unicodestatus == "1")
                {
                    convertedCode = ConvertToUnicode(sms.Script);

                }
                else if (sms.Unicodestatus == "0")
                {
                    convertedCode = sms.Script;

                }
                sts = _prc.Createscript(ApiKey, isUnicode, convertedCode, out Mailid, out string compname, out string statuus, out string tempid1, sms);
                //string[] mailsplits = Mailid.Split(',');


                //sendmail(mailsplits, compname);
                if (sts != null)
                {

                    retnmodel.Status = sts;
                    retnmodel.Message = statuus;
                    retnmodel.Templateid = tempid1;


                }
                else
                {
                    retnmodel.Status = sts;
                    retnmodel.Message = statuus;
                    retnmodel.Templateid = tempid1;

                }
            }
            return GetStringResponse(retnmodel, "json");
        }
        [ActionName("GetCreditBalanceDetails")]
        public HttpResponseMessage GetCreditBalanceDetails(string ApiKey)
        {
            //string status = "";
            APIRespnseBala rtnmodel = new APIRespnseBala();
            string sts = _prc.GetCreditBalanceDetails(ApiKey, out string msg);
            if (sts == "1")
            {
                rtnmodel.Status = "Success";
                rtnmodel.Balance = msg;
                return GetStringResponse(rtnmodel, "json");
            }
            else
            {
                rtnmodel.Status = "Failed";
                rtnmodel.Balance = msg;
                return GetStringResponse(rtnmodel, "json");
            }

        }
        public void sendmail(string[] emilid, string statsmsg)
        {
            MailServerModel mailserverdtlst = new MailServerModel();
            mailserverdtlst = _prc.GetMailServerDetails();
            try
            {
                for (int i = 0; i < emilid.Length; i++)
                {
                    string mailcontent = "";
                    string mm = emilid[i].ToString();
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(mailserverdtlst.MailServerIP.ToString(), 587);

                    mail.From = new MailAddress(mailserverdtlst.FromAddress.ToString());
                    mail.To.Add(mm);
                    mail.Subject = "Template Approval";
                    string url = "https://cts.myvi.in:8443/smsbuisnesssolution";

                    mailcontent = "Hi Admin," + " <br />" +
                                                "<p>" + statsmsg + "  " + "has created a Template.</ p>" + "<br />" +
                                                "<p>Please check and do the needfull.</ p>" + "<br />" +
                                                "<p> <strong><em><a href='" + url + "'>" + " SMSCampaign" + "</a></strong></em>" +
                                                " .</p>" +
                                                "<p>Sincerly,<br />" +
                                                //"Prudent Technologies Private Ltd.<br />" +
                                                  "<img src =cid:Logo /></p>" +
                                                "<img id='logo' alt='Prudent' src=cid:Logo width='auto' height='auto' />";
                    mail.Body = mailcontent;
                    mail.IsBodyHtml = true;

                    //"This is for testing SMTP mail from GMAIL";
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailcontent, null, "text/html");
                    LinkedResource IdeaLogo = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "/Content/images/prutech.png", "image/png");
                    IdeaLogo.ContentId = "Logo";
                    htmlView.LinkedResources.Add(IdeaLogo);

                    mail.AlternateViews.Add(htmlView);                                                                                      /*"<img id='logo' alt='Prudent' src=cid:Logo width='auto' height='auto' /></p>"*/;



                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(mailserverdtlst.UserName.ToString(), mailserverdtlst.Password.ToString());
                    //SmtpServer.EnableSsl = true;
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                    Console.Write("mail Send");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
        public void sendmailSenderId(string[] emilid, string statsmsg)
        {
            MailServerModel mailserverdtlst = new MailServerModel();
            mailserverdtlst = _prc.GetMailServerDetails();
            try
            {
                for (int i = 0; i < emilid.Length; i++)
                {
                    string mailcontent = "";
                    string mm = emilid[i].ToString();
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(mailserverdtlst.MailServerIP.ToString(), 587);

                    mail.From = new MailAddress(mailserverdtlst.FromAddress.ToString());
                    mail.To.Add(mm);
                    mail.Subject = "SenderID Creation";
                    string url = "https://cts.myvi.in:8443/smsbuisnesssolution";

                    mailcontent = "Hi Admin," + " <br />" +
                                                "<p>&nbsp;&nbsp;&nbsp;" + statsmsg + "  " + "has created a SenderID.Please check and do the needfull.<strong><em><a href='" + url + "'>" + " SMSCampaign" + "</a></strong></em></p>" +
                                                "<p>Sincerly,<br />" +
                                                //"Prudent Technologies Private Ltd.<br /></p>" +
                                                "<img id='logo' alt='Prudent' src=cid:Logo width='auto' height='auto' />";
                    mail.Body = mailcontent;
                    mail.IsBodyHtml = true;

                    //"This is for testing SMTP mail from GMAIL";
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailcontent, null, "text/html");
                    LinkedResource IdeaLogo = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "/Content/images/prutech.png", "image/png");
                    IdeaLogo.ContentId = "Logo";
                    htmlView.LinkedResources.Add(IdeaLogo);

                    mail.AlternateViews.Add(htmlView);                                                                                      /*"<img id='logo' alt='Prudent' src=cid:Logo width='auto' height='auto' /></p>"*/;



                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(mailserverdtlst.UserName.ToString(), mailserverdtlst.Password.ToString());
                    //SmtpServer.EnableSsl = true;
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                    Console.Write("mail Send");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
        private bool ValidateScript(script1 sms, out APIRespnse aPIResponse)
        {
            bool rtnSts = true;
            aPIResponse = new APIRespnse();
            long rslt;
            if (string.IsNullOrEmpty(sms.Script) || string.IsNullOrEmpty(sms.Unicodestatus) || string.IsNullOrEmpty(sms.Pingbackurl))
            {
                rtnSts = false;

                aPIResponse.Status = "Failed";
                aPIResponse.Message = "Some required field values missing";

            }
            else if (!long.TryParse(sms.DLTtemplateid, out rslt))
            {
                rtnSts = false;
            }
            return rtnSts;
        }
    }
}
