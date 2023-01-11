using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using ServiceStack;
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
using System.Text.RegularExpressions;
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

            if (ValidateToken(token))
            //if(token.Equals(otoken))
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

                        if ((arlist[k].CampaignName == null) || (arlist[k].CampaignName == "") || (arlist[k].FromDate == "") || (arlist[k].FromDate == null) || (arlist[k].ToDate == "") || (arlist[k].ToDate == null) || (arlist[k].FromTime == "") || (arlist[k].FromTime == null) || (arlist[k].ToTime == "") || (arlist[k].ToTime == null) || (arlist[k].TemplateID == "") || (arlist[k].TemplateID == null) || (arlist[k].pingbackurl == "") || (arlist[k].pingbackurl == null) || (arlist[k].RequestID == "") || (arlist[k].RequestID == null))
                        {
                            string MissingError = "{\"Message\": \"Please Fill All Fields \"}";

                            return GetString406ResponsewithMsg(MissingError);

                        }
                        if (string.IsNullOrWhiteSpace(arlist[k].CampaignName) || string.IsNullOrWhiteSpace(arlist[k].FromDate) || string.IsNullOrWhiteSpace(arlist[k].TemplateID) || string.IsNullOrWhiteSpace(arlist[k].pingbackurl) || string.IsNullOrWhiteSpace(arlist[k].RequestID) || string.IsNullOrWhiteSpace(arlist[k].ToTime) || string.IsNullOrWhiteSpace(arlist[k].FromTime) || string.IsNullOrWhiteSpace(arlist[k].FromTime))
                        {
                            string whitespaceError = "{\"Message\": \"Please Fill All Fields \"}";

                            return GetString406ResponsewithMsg(whitespaceError);

                        }
                        //if (string.IsNullOrWhiteSpace((arlist[k].FromDate) || (arlist[k].ToDate) || (arlist[k].FromTime) ||(arlist[k].ToTime) || (arlist[k].TemplateID) ||(arlist[k].pingbackurl)))
                        //{
                        //    string whitespaceError = "{\"status\":\"406\",\"Message\": \"Avoid WhiteSpaces \"}";

                        //    return GetString406ResponsewithMsg(whitespaceError);

                        //}

                        var fromdate = arlist[k].FromDate;

                        if (ValidateDate(fromdate))
                        {

                            string fromdateError = "{\"Message\": \"From date should be in dd/M/yyyy format \"}";

                            return GetString406ResponsewithMsg(fromdateError);
                        }
                        var todate = arlist[k].ToDate;
                        if (ValidateDate(todate))
                        {
                            string todateError = "{\"Message\": \"To date should be in dd/M/yyyy format \"}";

                            return GetString406ResponsewithMsg(todateError);

                        }
                        var fromtime = arlist[k].FromTime;
                        if (ValidateTime(fromtime))
                        {

                            string fromtimeError = "{\"Message\": \" From time  should be in hr:min AM/PM format \"}";

                            return GetString406ResponsewithMsg(fromtimeError);
                        }
                        var totime = arlist[k].ToTime;
                        if (ValidateTime(totime))
                        {
                            string fromtimeError = "{\"Message\": \" To time  should be in hr:min AM/PM format \"}";

                            return GetString406ResponsewithMsg(fromtimeError);
                        }
                        if (ValidateTime(fromtime, totime))
                        {

                            string fromtimeError = "{\"Message\": \" To time  should be in hr:min AM/PM format \"}";

                            return GetString406ResponsewithMsg(fromtimeError);

                        }

                        string fromDate = arlist[k].FromDate;
                        string toDate = arlist[k].ToDate;
                        string fromTime = arlist[k].FromTime;
                        string toTime = arlist[k].ToTime;
                        int status = checkEnddateValidity(toDate, fromDate);

                        if (status != 1)
                        {
                            string dateError = "{\"Message\": \" Check Date And Time  \"}";

                            return GetString406ResponsewithMsg(dateError);
                        }

                        var s1 = arlist[k].CampaignName;
                        string hexString = hex(s1);
                        arlist[k].CampaignName = hexString;

                    } //model iteration
                    string json = JsonConvert.SerializeObject(arlist);


                    var response = _prc.ApiCreateCampaign(apikey, json);

                    List<Create_Response> entity = new List<Create_Response>();


                    entity = JsonConvert.DeserializeObject<List<Create_Response>>(response);
                    string successStatus = "";
                    //string res = "";
                    for (int x = 0; x < entity.Count; x++)
                    {
                        string templatename = ""; string campaignname = ""; string unicodestatus = ""; string convertedCampname = ""; string convertedTempname = "";
                        unicodestatus = entity[x].Unicode;
                        templatename = entity[x].TemplateName;
                        campaignname = entity[x].CampaignName;
                        successStatus = entity[x].Status;
                        if (templatename != null)
                        {
                            convertedCampname = HextoString(campaignname);
                        }
                        if (campaignname != null)
                        {

                            convertedTempname = HextoString(templatename);
                        }


                        entity[x].TemplateName = convertedTempname;
                        entity[x].CampaignName = convertedCampname;

                        if (unicodestatus == "8")
                        {
                            string msg = "";
                            msg = GetUnicodeMessage(entity[x].TemplateMessage);
                            entity[x].TemplateMessage = msg;

                        }

                    }//entity iteration...
                    if (successStatus == "1")
                    {


                        string res = JsonConvert.SerializeObject(entity);

                        var obj = JsonConvert.DeserializeObject<dynamic>(res);
                        foreach (var item in obj)
                        {
                            item.Property("Unicode").Remove();
                            item.Property("Status").Remove();
                            item.Property("Message").Remove();

                        }

                        string Res = JsonConvert.SerializeObject(obj);

                        return GetStringResponse(Res);
                    }

                    if (response == "")
                    {
                        return GetString500Response();

                    }
                    else if (response == "[{}]")
                    {
                        string emptyres = "{\"Message\": \"Failure \"}";
                        return GetString500ResponsewithMsg(emptyres);
                    }
                    else
                    {
                        //string resp = entity.ToString();
                        return GetStringResponse(response);
                    }
                }
            }
            else
            {
                return GetString401Response();
            }
        }
        public static string HextoString(string InputText)
        {

            byte[] bb = Enumerable.Range(0, InputText.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(InputText.Substring(x, 2), 16))
                             .ToArray();
            return System.Text.Encoding.ASCII.GetString(bb);
            // or System.Text.Encoding.UTF7.GetString
            // or System.Text.Encoding.UTF8.GetString
            // or System.Text.Encoding.Unicode.GetString
            // or etc.
        }
        string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder strinbuilder = new StringBuilder();
            foreach (string value in array)
            {
                strinbuilder.Append(value);
                strinbuilder.Append(' ');
            }
            return strinbuilder.ToString();
        }
        public string GetUnicodeMessage(string message)
        {
            string str = message.ToString();
            string data = "";
            if (str.Trim() != "")
            {

                string tempMessage = "\\u" + Regex.Replace(message, ".{4}", "$0\\u");
                data = Regex.Unescape(tempMessage.Substring(0, tempMessage.Length - 2));

                data = data.Replace("\n", "");
                data = data.Replace("\r\n", "");
                return data;
            }
            else
            {

                data = data.Replace("\n", "");
                data = data.Replace("\r\n", "");
                return data;

            }

        }
        public Boolean ValidateToken(string token)
        {

            string otoken = "";
            otoken = ConfigurationManager.AppSettings["Token"].ToString();
            Boolean res = true;
            if (token != otoken)
            {
                res = false;
                return res;
            }
            return res;
        }


        public int checkEnddateValidity(string toDate, string fromDate)
        {

            string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime currentdate = DateTime.ParseExact(currentDate, "dd/MM/yyyy", null);
            DateTime fromdate, todate;
            try
            {
                fromdate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                todate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
            }
            catch
            {
                string edata = "{\"message\":\"Enter Valid Date\"}";
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable) { Content = new StringContent(edata, System.Text.Encoding.UTF8, "application/json") };
                throw new HttpResponseException(response);
                //var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                //string errordata = "Enter Valid Date ";
                // return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent( System.Text.Encoding.UTF8, "application/json") };

                //throw new HttpResponseException(resp);

            }
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            //DateTime fromdate = DateTime.Parse(fromDate); //uses the current Thread's culture
            //DateTime todate = DateTime.Parse(toDate); //uses the current Thread's culture
            if (todate < currentdate)
            {
                return 5;

            }
            if (fromdate < currentdate)
            {
                return 5;

            }
            return 1;

        }
        public Boolean ValidateTime(string fromtime, string totime)
        {
            bool res = false;
            try
            {
                DateTime dt1 = DateTime.Parse(fromtime);
                DateTime dt2 = DateTime.Parse(totime);

                dt1.ToString("HH:mm"); // 07:00 // 24 hour clock // hour is always 2 digits
                dt1.ToString("hh:mm tt"); // 07:00 AM // 12 hour clock // hour is always 2 digits

            }
            catch
            {
                string edata = "{\"message\":\"Enter Valid Time\"}";
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable) { Content = new StringContent(edata, System.Text.Encoding.UTF8, "application/json") };
                throw new HttpResponseException(response);

            }



            return res;


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
            //else if (isCurrentDate && compDate.AddHours(4) > endDate)
            //    return 7;
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
                        if (arlist[k].BulkData == null)
                        {
                            string MissingError = "{\"Message\": \"Please Add Bulk Data\"}";


                            return GetString406ResponsewithMsg(MissingError);
                        }
                        int len = 0;
                        if (arlist[k].BulkData.Count != 0)
                        {
                            len = arlist[k].BulkData.Count;
                        }

                        if ((arlist[k].BulkData == null) || (arlist[k].MasterCampaignID == null) || (arlist[k].MasterCampaignID == "") || (arlist[k].RequestID == null) || (arlist[k].RequestID == ""))
                        {

                            string MissingError = "{\"Message\": \"Please Fill All Field\"}";


                            return GetString406ResponsewithMsg(MissingError);

                        }
                        if (string.IsNullOrWhiteSpace(arlist[k].MasterCampaignID) || string.IsNullOrWhiteSpace(arlist[k].RequestID))
                        {
                            string whitespaceError = "{\"Message\": \"Please Fill All Fields \"}";

                            return GetString406ResponsewithMsg(whitespaceError);

                        }

                        // string varlengthError = "{\"status\":\"406\",\"Message\": \"Add Valid variableName(maximum length should not exceed 32 character and minimum length should have atleast 2 character) for \"}";

                        //string varvalidateError = "{\"status\":\"406\",\"Message\": \"Avoid special characters like newline, new tab, {,}, ... etc in ";
                        for (int m = 0; m < len; m++)
                        {
                            if ((arlist[k].BulkData[m].Msisdn == null) || (arlist[k].BulkData[m].Msisdn == ""))
                            {

                                string MissingError = "{\"Message\": \"Add Valid Msisdn\"}";

                                return GetString406ResponsewithMsg(MissingError);

                            }
                            if (string.IsNullOrWhiteSpace(arlist[k].BulkData[m].Msisdn))
                            {
                                string whitespaceError = "{\"Message\": \"Add Valid Msisdn \"}";

                                return GetString406ResponsewithMsg(whitespaceError);

                            }

                            var msisdn = arlist[k].BulkData[m].Msisdn.ToString();
                            var mlen = msisdn.Length;
                            if (mlen >= 14)
                            {
                                string msisdnlengthError = "{\"Message\": \"Add Valid Msisdn \"}";

                                return GetString406ResponsewithMsg(msisdnlengthError);

                            }
                            if (mlen < 10)
                            {
                                string msisdnlengthError = "{\"Message\": \"Add Valid Msisdn (min 10 digit) \"}";

                                return GetString406ResponsewithMsg(msisdnlengthError);


                            }

                            for (int i = 0; i < msisdn.Length; i++)
                            {
                                char c = msisdn[i];
                                if (c < '0' || c > '9')
                                {
                                    string msisdnlengthError = "{\"Message\": \"Add Valid Msisdn (msisdn must numbers)\"}";

                                    return GetString406ResponsewithMsg(msisdnlengthError);
                                }

                            }

                            List<string> msisdnArray = new List<string>();
                            msisdnArray.Add(arlist[k].BulkData[m].Msisdn);
                            //int n_status = ValidatemsisdnRep(msisdnArray);
                            List<string> varArray = new List<string>();
                            var var1 = ""; var var2 = ""; var var3 = ""; var var4 = ""; var var5 = ""; var var6 = ""; var var7 = ""; var var8 = ""; var var9 = ""; var var10 = "";
                            if (arlist[k].BulkData[m].VAR1 != null)
                            {
                                var1 = arlist[k].BulkData[m].VAR1.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR2 != null)
                            {
                                var2 = arlist[k].BulkData[m].VAR2.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR3 != null)
                            {
                                var3 = arlist[k].BulkData[m].VAR3.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR4 != null)
                            {
                                var4 = arlist[k].BulkData[m].VAR4.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR5 != null)
                            {
                                var5 = arlist[k].BulkData[m].VAR5.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR6 != null)
                            {
                                var6 = arlist[k].BulkData[m].VAR6.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR7 != null)
                            {
                                var7 = arlist[k].BulkData[m].VAR7.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR8 != null)
                            {
                                var8 = arlist[k].BulkData[m].VAR8.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR9 != null)
                            {
                                var9 = arlist[k].BulkData[m].VAR9.ToString();
                            }
                            if (arlist[k].BulkData[m].VAR10 != null)
                            {
                                var10 = arlist[k].BulkData[m].VAR10.ToString();
                            }


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
                                var s1 = uniArray[v];
                                //bool isUnicode = System.Text.ASCIIEncoding.GetEncoding(0).GetString(System.Text.ASCIIEncoding.GetEncoding(0).GetBytes(s1)) != s1;
                                //if (isUnicode) {
                                var unicode = GetSingleUnicodeHex(s1);

                                uniArray[v] = unicode;

                                int e_status = ValidateVAR(unicode);


                                if (e_status != 1)
                                {
                                    string varlengthError = "{\"Message\": \"Add Valid VAR Field\"}";

                                    return GetString406ResponsewithMsg(varlengthError);

                                }

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
                    List<Res_Insert> entity = new List<Res_Insert>();


                    entity = JsonConvert.DeserializeObject<List<Res_Insert>>(response);
                    string successStatus = "";
                    for (int x = 0; x < entity.Count; x++)
                    {
                        successStatus = entity[x].Status;

                    }//entity iteration...
                    if (successStatus == "9")
                    {

                        string res = JsonConvert.SerializeObject(entity);

                        var obj = JsonConvert.DeserializeObject<dynamic>(res);
                        foreach (var item in obj)
                        {

                            item.Property("Status").Remove();


                        }

                        string Res = JsonConvert.SerializeObject(obj);

                        //return GetStringResponse(Res);

                        return GetString500ResponsewithMsg(Res);
                    }
                    //if (entity.status == "9")
                    //{

                    //    return GetString500Responsewithnull(response);


                    //}

                    if (response == "")
                    {

                        return GetString500Response();
                    }

                    else if (response == "[{}]")
                    {

                        string emptyres = "{\"Message\": \"Failure\"}";


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
        public int ValidateVAR(string str)
        {
            int length = 0;
            length = str.Length / 4;
            if (length > 32)
            {
                return 5;
            }
            return 1;

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
        //public int ValidatemsisdnRep(List<string>msisdnArray) {



        //}
        public int ValidateVARempty(List<string> varArray, string length)
        {
            int Length = Int16.Parse(length);
            for (int i = 0; i < Length; i++)
            {

                if (varArray[i] == "")
                {
                    return 5;
                }
                //for (int m = i+1; m < varArray.Count; m++) {
                //    if (varArray[m] != "") {
                //        return 5;
                //    }
                //}
            }

            for (int n = Length; n < varArray.Count; n++)
            {
                if (varArray[n] != "")
                {
                    return 5;
                }

            }
            return 1;
        }
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



        public class Res_Insert
        {

            public string message { get; set; }
            public string Status { get; set; }

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
        private HttpResponseMessage GetString415ResponsewithMsg(string errordata)
        {
            return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, errordata);
        }
        private HttpResponseMessage GetString500ResponsewithMsg(string errordata)
        {


            return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errordata, System.Text.Encoding.UTF8, "application/json") };

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
        private HttpResponseMessage GetString500Responsewithnull(string errordata)
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
