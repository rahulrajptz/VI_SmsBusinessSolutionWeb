using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
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
using Templateprj.Models.ApiModels;

namespace Templateprj.Controllers.api
{
    [RoutePrefix("Values")]
    public class ValuesController : ApiController
    {

        private readonly MailSender _prcs = new MailSender();

        ApiDbprc _prc = new ApiDbprc();

        [Route("CreateCampaign/{apikey}")]
        public HttpResponseMessage PostCreateCampaign(string apikey, [FromBody]campaignmodel[] model)
        {
            var CreateCampaignResponse = new CreateCampaignResponse { };
            string token = Request.Headers.Authorization.Parameter;


            token = token.ToString();
            string otoken = "";
            otoken = ConfigurationManager.AppSettings["Token"].ToString();

            if (token.Equals(otoken))
            {

                if (model == null)
                {
                    return GetString500Response();

                }
                else
                {
                    var length = model.Length;

                    List<campaignmodel> arlist = new List<campaignmodel>();

                    for (int i = 0; i < length; i++)
                    {
                        arlist.Add(model[i]);

                    }
                    for (int k = 0; k < length; k++)
                    {

                        if ((arlist[k].Campaign_Name == null) || (arlist[k].Campaign_Name == "") || (arlist[k].Campaign_Type == null) || (arlist[k].Campaign_Type == null) || (arlist[k].From_Date == "") || (arlist[k].From_Date == null) || (arlist[k].To_Date == "") || (arlist[k].To_Date == null) || (arlist[k].From_Time == "") || (arlist[k].From_Time == null) || (arlist[k].to_Time == "") || (arlist[k].to_Time == null) || (arlist[k].Template_ID == "") || (arlist[k].Template_ID == null) || (arlist[k].pingbackurl == "") || (arlist[k].pingbackurl == null) || (arlist[k].content_type == "") || (arlist[k].content_type == null) || (arlist[k].Request_ID == "") || (arlist[k].Request_ID == null))
                        {
                            string MissingError = "{\"status\":\"406\",\"Message\": \"Please Fill All Fields\"}";

                            return GetString406ResponsewithMsg(MissingError);

                        }
                        if (string.IsNullOrWhiteSpace(arlist[k].Campaign_Name))
                        {
                            string whitespaceError = "{\"status\":\"406\",\"Message\": \"Please Enter Valid Campaign Name \"}";

                            return GetString406ResponsewithMsg(whitespaceError);



                        }
                        if (string.IsNullOrWhiteSpace(arlist[k].Campaign_Type) || string.IsNullOrWhiteSpace(arlist[k].From_Date) || string.IsNullOrWhiteSpace(arlist[k].To_Date) || string.IsNullOrWhiteSpace(arlist[k].From_Time) || string.IsNullOrWhiteSpace(arlist[k].to_Time) || string.IsNullOrWhiteSpace(arlist[k].Template_ID) || string.IsNullOrWhiteSpace(arlist[k].pingbackurl) || string.IsNullOrWhiteSpace(arlist[k].content_type))
                        {
                            string whitespaceError = "{\"status\":\"406\",\"Message\": \"Avoid WhiteSpaces \"}";

                            return GetString406ResponsewithMsg(whitespaceError);



                        }
                        var fromdate = arlist[k].From_Date;

                        if (ValidateDate(fromdate))
                        {

                            string fromdateError = "{\"status\":\"406\",\"Message\": \"From date should be in dd/M/yyyy format \"}";

                            return GetString406ResponsewithMsg(fromdateError);
                        }
                        var todate = arlist[k].To_Date;
                        if (ValidateDate(todate))
                        {
                            string todateError = "{\"status\":\"406\",\"Message\": \"To date should be in dd/M/yyyy format \"}";

                            return GetString406ResponsewithMsg(todateError);

                        }
                        var fromtime = arlist[k].From_Time;
                        if (ValidateTime(fromtime))
                        {

                            string fromtimeError = "{\"status\":\"406\",\"Message\": \" From time  should be in hr:min AM/PM format \"}";

                            return GetString406ResponsewithMsg(fromtimeError);
                        }
                        var totime = arlist[k].to_Time;
                        if (ValidateTime(totime))
                        {
                            string fromtimeError = "{\"status\":\"406\",\"Message\": \" To time  should be in hr:min AM/PM format \"}";

                            return GetString406ResponsewithMsg(fromtimeError);
                        }


                        string fromDate = arlist[k].From_Date;
                        string toDate = arlist[k].To_Date;
                        string fromTime = arlist[k].From_Time;
                        string toTime = arlist[k].to_Time;
                        int status = checkEndTimeValidity(toDate + " " + toTime, fromDate + " " + fromTime, true);
                        if (status != 1)
                        {
                            string dateError = "{\"status\":\"406\",\"Message\": \" Check Date And Time  \"}";

                            return GetString406ResponsewithMsg(dateError);


                        }


                        string contype = arlist[k].content_type;
                        contype = contype.Trim().ToLower();


                        if (contype == "dynamic")
                        {

                            string varcount = arlist[k].Number_of_variables;
                            if (varcount == "0")
                            {

                                string dynamicvar0Error = "{\"status\":\"406\",\"Message\": \"Variable Count Should Greater Than Zero For Dynamic Type \"}";

                                return GetString406ResponsewithMsg(dynamicvar0Error);


                            }
                            var countlen = varcount.Length;
                            for (int i = 0; i < varcount.Length; i++)
                            {
                                char c = varcount[i];
                                if (c < '0' || c > '9')
                                {
                                    string varcountError = "{\"status\":\"406\",\"Message\": \"Add Valid Variable Count \"}";

                                    return GetString406ResponsewithMsg(varcountError);
                                }

                            }

                        }
                        if (contype == "static")
                        {

                            string varcount = arlist[k].Number_of_variables;

                            if (varcount != "0")
                            {

                                string dynamicvar0Error = "{\"status\":\"406\",\"Message\": \"Variable Count Should Be Zero For Static Type \"}";

                                return GetString406ResponsewithMsg(dynamicvar0Error);

                            }

                        }

                        if (contype == "dynamic")
                        {
                            var len_1 = arlist[k].SMSvariable.Count.ToString();
                            var varcount1 = arlist[k].Number_of_variables;
                            if (len_1 != varcount1)
                            {
                                string contenttypeError = "{\"status\":\"406\",\"Message\": \"No of variables and Sms variable count should be same \"}";

                                return GetString406ResponsewithMsg(contenttypeError);

                            }
                        }
                        if (contype != "static" && contype != "dynamic")
                        {

                            string contenttypeError = "{\"status\":\"406\",\"Message\": \"Content Type Must Static or Dynamic \"}";

                            return GetString406ResponsewithMsg(contenttypeError);

                        }



                        //var am_pm2 = strlist4[2];
                        if (contype == "dynamic")
                        {
                            var len = arlist[k].SMSvariable.Count;
                            for (int x = 0; x < len; x++)
                            {

                                var var1length = arlist[k].SMSvariable[x].variableName.ToString();
                                if (ValidateLength(var1length))
                                {
                                    string varengthError = "{\"status\":\"406\",\"Message\": \"Add Valid variableName(maximum length should not exceed 14 character and minimum length should have atleast 2 character) \"}";

                                    return GetString406ResponsewithMsg(varengthError);

                                }

                                var var2length = arlist[k].SMSvariable[x].renameVariable.ToString();

                                if (ValidateLength(var2length))
                                {
                                    string varengthError = "{\"status\":\"406\",\"Message\": \"Add Valid variableName(maximum length should not exceed 14 character and minimum length should have atleast 2 character) \"}";

                                    return GetString406ResponsewithMsg(varengthError);
                                }
                            }//sms variable iteration...

                        }
                        var s1 = arlist[k].Campaign_Name;
                        string hexString = hex(s1);
                        arlist[k].Campaign_Name = hexString;

                    } //model iteration
                    string json = JsonConvert.SerializeObject(arlist);
                    var response = _prc.ApiCreateCampaign(apikey, json);

                    if (response == "")
                    {

                        return GetString500Response();


                    }
                    else if (response == "[{}]")
                    {

                        string emptyres = "{\"status\":\"failure\",\"Message\": \"Check Template ID\"}";


                        return GetString500ResponsewithMsg(emptyres);

                    }
                    else
                    {
                        return GetStringResponse(response);
                    }

                }

            }
            else
            {

                return GetString401Response();

            }
        }
        public int checkEndTimeValidity(string endTime, string starttime, bool isCurrentDate)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime endDate, startDate, compDate;
            endDate = DateTime.ParseExact(endTime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            startDate = DateTime.ParseExact(starttime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            if (isCurrentDate)
                compDate = DateTime.Now;
            else
                compDate = startDate;
            //    compDate = DateTime.ParseExact(comparetime, "dd/M/yyyy h:mm tt", CultureInfo.InvariantCulture);
            if (startDate >= endDate)
                return 5;
            else if (isCurrentDate && compDate.AddHours(4) > endDate)
                return 7;
            else if (startDate < compDate)
            {
                return 6;
            }
            else if (endDate < compDate)
            {

                return 4;
            }
            else
                return 1;
        }

        [Route("InsertBulksms/{apikey}")]
        public HttpResponseMessage PostInsertBulksms(string apikey, [FromBody]Insertbulksmsmodel[] model)
        {
            var CreateCampaignResponse = new CreateCampaignResponse { };
            string token = Request.Headers.Authorization.Parameter;


            token = token.ToString();
            string otoken = "";
            otoken = ConfigurationManager.AppSettings["Token"].ToString();

            if (token.Equals(otoken))
            {

                if (model == null)
                {
                    return GetString500Response();

                }
                else
                {
                    var length = model.Length;

                    List<Insertbulksmsmodel> arlist = new List<Insertbulksmsmodel>();

                    for (int i = 0; i < length; i++)
                    {
                        arlist.Add(model[i]);

                    }


                    for (int k = 0; k < length; k++)
                    {
                        var len = arlist[k].BulkData.Count;
                        if ((arlist[k].BulkData == null) || (arlist[k].ContentType == "") || (arlist[k].ContentType == null) || (arlist[k].MasterCampaignID == null) || (arlist[k].MasterCampaignID == "") || (arlist[k].RequestID == null) || (arlist[k].RequestID == "") || (arlist[k].PingbackUrl == null) || (arlist[k].PingbackUrl == ""))
                        {

                            string MissingError = "{\"status\":\"406\",\"Message\": \"Please Fill All Field\"}";


                            return GetString406ResponsewithMsg(MissingError);

                        }
                        if (string.IsNullOrWhiteSpace(arlist[k].ContentType) || string.IsNullOrWhiteSpace(arlist[k].MasterCampaignID) || string.IsNullOrWhiteSpace(arlist[k].RequestID) || string.IsNullOrWhiteSpace(arlist[k].PingbackUrl))
                        {
                            string whitespaceError = "{\"status\":\"406\",\"Message\": \"Avoid WhiteSpaces \"}";

                            return GetString406ResponsewithMsg(whitespaceError);

                        }


                        string contype = arlist[k].ContentType;
                        contype = contype.Trim().ToLower();
                        if (contype == "dynamic")
                        {

                            string varcount = arlist[k].NumberOfVariables;
                            if (varcount == "0")
                            {

                                string dynamicvar0Error = "{\"status\":\"406\",\"Message\": \"Variable Count Should Greater Than Zero For Dynamic Type \"}";

                                return GetString406ResponsewithMsg(dynamicvar0Error);


                            }

                        }
                        if (contype == "static")
                        {

                            string varcount = arlist[k].NumberOfVariables;

                            if (varcount != "0")
                            {

                                string dynamicvar0Error = "{\"status\":\"406\",\"Message\": \"Variable Count Should Be Zero For Static Type \"}";

                                return GetString406ResponsewithMsg(dynamicvar0Error);

                            }

                            var countlen = varcount.Length;
                            for (int i = 0; i < varcount.Length; i++)
                            {
                                char c = varcount[i];
                                if (c < '0' || c > '9')
                                {
                                    string varcountError = "{\"status\":\"406\",\"Message\": \"Add Valid Variable Count \"}";

                                    return GetString406ResponsewithMsg(varcountError);
                                }

                            }


                        }
                        if (contype != "static" && contype != "dynamic")
                        {

                            string contenttypeError = "{\"status\":\"406\",\"Message\": \"Content Type Must Static or Dynamic \"}";

                            return GetString406ResponsewithMsg(contenttypeError);

                        }

                        string varlengthError = "{\"status\":\"406\",\"Message\": \"Add Valid variableName(maximum length should not exceed 32 character and minimum length should have atleast 2 character) for \"}";

                        //string varvalidateError = "{\"status\":\"406\",\"Message\": \"Avoid special characters like newline, new tab, {,}, ... etc in ";
                        for (int m = 0; m < len; m++)
                        {



                            if ((arlist[k].BulkData[m].Msisdn == null) || (arlist[k].BulkData[m].Msisdn == ""))
                            {

                                string MissingError = "{\"status\":\"406\",\"Message\": \"Add Valid Msisdn\"}";

                                return GetString406ResponsewithMsg(MissingError);

                            }
                            if (string.IsNullOrWhiteSpace(arlist[k].BulkData[m].Msisdn))
                            {
                                string whitespaceError = "{\"status\":\"406\",\"Message\": \"Add Valid Msisdn \"}";

                                return GetString406ResponsewithMsg(whitespaceError);



                            }

                            var msisdn = arlist[k].BulkData[m].Msisdn.ToString();
                            var mlen = msisdn.Length;
                            if (mlen >= 14)
                            {
                                string msisdnlengthError = "{\"status\":\"406\",\"Message\": \"Add Valid Msisdn \"}";

                                return GetString406ResponsewithMsg(msisdnlengthError);

                            }
                            if (mlen < 10)
                            {
                                string msisdnlengthError = "{\"status\":\"406\",\"Message\": \"Add Valid Msisdn (min 10 digit) \"}";

                                return GetString406ResponsewithMsg(msisdnlengthError);


                            }

                            for (int i = 0; i < msisdn.Length; i++)
                            {
                                char c = msisdn[i];
                                if (c < '0' || c > '9')
                                {
                                    string msisdnlengthError = "{\"status\":\"406\",\"Message\": \"Add Valid Msisdn (msisdn must numbers)\"}";

                                    return GetString406ResponsewithMsg(msisdnlengthError);
                                }

                            }

                            List<string> varArray = new List<string>();


                            var var1 = arlist[k].BulkData[m].VAR1.ToString();
                            var var2 = arlist[k].BulkData[m].VAR2.ToString();
                            var var3 = arlist[k].BulkData[m].VAR3.ToString();
                            var var4 = arlist[k].BulkData[m].VAR4.ToString();
                            var var5 = arlist[k].BulkData[m].VAR5.ToString();
                            var var6 = arlist[k].BulkData[m].VAR6.ToString();
                            var var7 = arlist[k].BulkData[m].VAR7.ToString();
                            var var8 = arlist[k].BulkData[m].VAR8.ToString();
                            var var9 = arlist[k].BulkData[m].VAR9.ToString();
                            var var10 = arlist[k].BulkData[m].VAR10.ToString();

                            varArray.Add(var1);
                            varArray.Add(var2);
                            varArray.Add(var3);
                            varArray.Add(var4);
                            varArray.Add(var5);
                            varArray.Add(var6);
                            varArray.Add(var7);
                            varArray.Add(var8);
                            varArray.Add(var9);
                            varArray.Add(var10);


                            var leng = varArray.Count;



                            for (int n = 0; n < leng; n++)
                            {

                                if (varArray[n] != "")
                                {

                                    if (ValidateLength(varArray[n]))
                                    {
                                        string var = "VAR" + n + 1;
                                        string jsonvalidateError = varlengthError + var;
                                        return GetString406ResponsewithMsg(jsonvalidateError);

                                    }
                                }
                            }



                            for (int y = 0; y < leng; y++)
                            {
                                var s1 = varArray[y];
                                string hexString = hex(s1);

                                varArray[y] = hexString;
                            }

                            List<string> uniArray = new List<string>();
                            uniArray.Add(var1);
                            uniArray.Add(var2);
                            uniArray.Add(var3);
                            uniArray.Add(var4);
                            uniArray.Add(var5);
                            uniArray.Add(var6);
                            uniArray.Add(var7);
                            uniArray.Add(var8);
                            uniArray.Add(var9);
                            uniArray.Add(var10);
                            for (int v = 0; v < leng; v++)
                            {
                                var s1 = varArray[v];
                                //bool isUnicode = System.Text.ASCIIEncoding.GetEncoding(0).GetString(System.Text.ASCIIEncoding.GetEncoding(0).GetBytes(s1)) != s1;
                                //if (isUnicode) {
                                var unicode = GetSingleUnicodeHex(s1);

                                uniArray[v] = unicode;



                            }



                            arlist[k].BulkData[m].VAR1 = varArray[0];
                            arlist[k].BulkData[m].VAR2 = varArray[1];
                            arlist[k].BulkData[m].VAR3 = varArray[2];
                            arlist[k].BulkData[m].VAR4 = varArray[3];
                            arlist[k].BulkData[m].VAR5 = varArray[4];
                            arlist[k].BulkData[m].VAR6 = varArray[5];
                            arlist[k].BulkData[m].VAR7 = varArray[6];
                            arlist[k].BulkData[m].VAR8 = varArray[7];
                            arlist[k].BulkData[m].VAR9 = varArray[8];
                            arlist[k].BulkData[m].VAR10 = varArray[9];


                            arlist[k].BulkData[m].VARU1 = uniArray[0];
                            arlist[k].BulkData[m].VARU2 = uniArray[1];
                            arlist[k].BulkData[m].VARU3 = uniArray[2];
                            arlist[k].BulkData[m].VARU4 = uniArray[3];
                            arlist[k].BulkData[m].VARU5 = uniArray[4];
                            arlist[k].BulkData[m].VARU6 = uniArray[5];
                            arlist[k].BulkData[m].VARU7 = uniArray[6];
                            arlist[k].BulkData[m].VARU8 = uniArray[7];
                            arlist[k].BulkData[m].VARU9 = uniArray[8];
                            arlist[k].BulkData[m].VARU10 = uniArray[9];


                        }//bulk datalist iteration...


                    }//model list iteration...

                    string json = JsonConvert.SerializeObject(arlist);
                    var response = _prc.ApiInsertBulksms(apikey, json);
                    List<failure> failureRes = new List<failure>();
                    var entity = JsonConvert.DeserializeObject<failure>(response);
                    if (entity.Status == "9")
                    {
                        return GetString500Response();

                    }

                    if (response == "")
                    {

                        return GetString500Response();
                    }

                    else if (response == "[{}]")
                    {

                        string emptyres = "{\"status\":\"Failure\",\"Message\": \"Check Template ID\"}";


                        return GetString500ResponsewithMsg(emptyres);
                    }

                    else
                    {
                        return GetStringResponse(response);
                    }
                }
            }
            else
            {

                return GetString401Response();
            }
        }

        //public Boolean ValidateVAR(string str) {
        //    bool s1 = false;
        //    var specialChars = new[] { '\"', '\n', '\t', '{', '}', '\\', '\r' };

        //    foreach (var specialChar in specialChars.Where(str.Contains))
        //    {
        //        s1 = true;
        //          return s1;
        //    }

        //    return s1;
        //}
        public static string GetSingleUnicodeHex(string strTextMsg)
        {
            byte[] s1 = UTF8Encoding.Unicode.GetBytes(strTextMsg);
            string strUnicode = "";
            string strTmp1 = "";
            string strTmp2 = "";

            for (int i = 0; i < s1.Length; i += 2)
            {
                strTmp1 = int.Parse(s1[i + 1].ToString()).ToString("x");
                if (strTmp1.Length == 1)
                    strTmp1 = "0" + strTmp1;

                strTmp2 = int.Parse(s1[i].ToString()).ToString("x");
                if (strTmp2.Length == 1)
                    strTmp2 = "0" + strTmp2;

                strUnicode += strTmp1 + strTmp2;
            }
            return strUnicode;
        }
        public static string hex(string decString)
        {
            byte[] bytes = Encoding.Default.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        public Boolean ValidateDate(string date)
        {
            bool res = false;
            String[] spearator = { "/" };


            // using the method
            String[] strlist = date.Split(spearator,
                 StringSplitOptions.RemoveEmptyEntries);
            if (strlist.Length != 3)
            {
                res = true;
                return res;

            }
            else
            {


                var day1 = strlist[0];
                var month1 = strlist[1];
                var year1 = strlist[2];
                return res;

            }


        }


        public Boolean ValidateTime(string time)
        {

            bool res = false;
            String[] spearator = { ":" };


            // using the method
            String[] strlist = time.Split(spearator,
                 StringSplitOptions.RemoveEmptyEntries);
            if (strlist.Length != 2)
            {
                res = true;
                return res;

            }
            var hr1 = strlist[0];
            var min1 = strlist[1];

            string[] seperatorq = { " " };
            String[] strlistq = min1.Split(seperatorq,
                StringSplitOptions.RemoveEmptyEntries);
            if (strlistq.Length != 2)
            {
                res = true;
                return res;


            }
            else
            {

                return res;


            }


        }
        public Boolean ValidateLength(string var)
        {

            bool res = false;
            var mlen = var.Length;
            if (mlen >= 32)
            {
                res = true;
                return res;
            }
            if (mlen < 2)
            {
                res = true;
                return res;


            }
            return res;
        }



        public class failure
        {


            public string Status { get; set; }
            public string Message { get; set; }
            public string Process { get; set; }
            public string MasterCampaignID { get; set; }
            public string CampaignCreateddate { get; set; }
            public string RequestID { get; set; }
            public string CampaignName { get; set; }
            public string CampaignType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
            public string TemplateId { get; set; }
            public string TemplateName { get; set; }
            public string TemplateMessage { get; set; }
            public string NumberOfVariables { get; set; }
            public List<SMSvariable> SMSvariable { get; set; }

        }
        public class SMSvariable
        {
            public string variableName { get; set; }
            public string renameVariable { get; set; }

        }


        //    string campId = "";
        //string convertedCode = "";
        //string isUnicode;
        //APIResponse rtnmodel = new APIResponse();
        //if (IsIPValid(apikey.Trim(), ""))
        //{
        //    if (Validatecampaign(campaign, out rtnmodel))
        //    {
        //        isUnicode = _prc.GetUnicodeStatus(campaign.templateid);
        //        if (isUnicode == "1")
        //        {
        //            convertedCode = ConvertToUnicode(campaign.script);
        //        }
        //        string sts = _prc.CreateCampign(apikey, convertedCode, isUnicode, campaign, out string response, out campId);
        //        if (sts == "1")
        //        {
        //            rtnmodel.Status = "Success";
        //            rtnmodel.Message = response;
        //            rtnmodel.CampaignId = campId;


        //        }
        //        else
        //        {
        //            rtnmodel.Status = "Failed";
        //            rtnmodel.Message = response;
        //            rtnmodel.CampaignId = "";
        //        }
        //    }
        //}
        //else
        //{
        //    rtnmodel.Status = "failed";
        //    rtnmodel.Message = "Authentication failed!";
        //    rtnmodel.CampaignId = "";

        //}

        //return GetStringResponse( "json");


        //public List<campaignmodel> Campaignmodel { get; set; }


        public class CreateCampaignResponse
        {
            public CreateCampaignResponse()
            {

                this.Token = "";
                this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }

            public string Token { get; set; }
            public HttpResponseMessage responseMsg { get; set; }

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

        private HttpResponseMessage GetString406ResponsewithMsg(string errordata)
        {


            return new HttpResponseMessage(HttpStatusCode.NotAcceptable) { Content = new StringContent(errordata, System.Text.Encoding.UTF8, "application/json") };

        }
        private HttpResponseMessage GetStringOKResponsewithMsg(string errordata)
        {
            return Request.CreateResponse(HttpStatusCode.OK, errordata);
        }
        private HttpResponseMessage GetString500Response()
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
        private HttpResponseMessage GetString500ResponsewithMsg(string errordata)
        {
            //return Request.CreateResponse(HttpStatusCode.InternalServerError,errordata);
            return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errordata, System.Text.Encoding.UTF8, "application/json") };

        }

        private HttpResponseMessage GetString401Response()
        {
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
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

                return GetStringResponse(postData);
            }

        }
        [ActionName("CreateSenderID")]
        public HttpResponseMessage PostCreateSenderID(string ApiKey, Senderid Senderid)
        {
            //string status = "";
            APIResponsesenderid rtnmodel = new APIResponsesenderid();
            string sts = _prc.CreateSenderid(ApiKey, Senderid, out string mail, out string compnyname, out string msg, out string refid);
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
