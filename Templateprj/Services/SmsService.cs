using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Templateprj.DataAccess;
using Templateprj.Models;
using Templateprj.Models.ApiModels;

namespace Templateprj.Services
{
    public class SmsService : ISmsService
    {
        ApiDbprc _prc = new ApiDbprc();

        public APIResponse ProcessInput(string clientIpAddress, string apikey, BulkSms bulkSms)
        {

            //string isUnicode;
            APIResponse rtnmodel = new APIResponse();
            if (IsIPValid(clientIpAddress,apikey.Trim(), bulkSms.CampaignId))
            {
                var input = new JavaScriptSerializer().Serialize(bulkSms.Msisdns).Replace("\\", "");
                string sts = _prc.UpdateBulkSms(input, bulkSms.CampaignId, apikey,out string response);
                if (sts == "1")
                {
                    rtnmodel.Status = "Success";
                    rtnmodel.Message = response;
                    rtnmodel.CampaignId = bulkSms.CampaignId;
                }
                else
                {
                    rtnmodel.Status = "Failed";
                    rtnmodel.Message = response;
                    rtnmodel.CampaignId = "";
                }
            }

            else
            {
                rtnmodel.Status = "failed";
                rtnmodel.Message = "Authentication failed!";
                rtnmodel.CampaignId = "";

            }
            return rtnmodel;
        }

        private bool IsIPValid(string clientIpAddress, string key, string camPId)
        {
         
            // LogWriter.Write("IPs :: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.ffff tt"));
            string sts = _prc.CheckIPValid(key, clientIpAddress.Trim());
            // LogWriter.Write("prcexe :: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.ffff tt"));
            return sts == "1" ? true : false;

        }

        private bool CheckIPValidBase(string key, string camPId)
        {

            string sts = _prc.CheckIPValidBase(key, camPId);
            return sts == "1" ? true : false;
        }

    }
}