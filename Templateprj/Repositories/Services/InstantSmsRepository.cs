using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Helpers;
using Templateprj.Models.InstantSms;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Repositories.Services
{
    public class InstantSmsRepository : IInstantSmsRepository
    {
        public InstantSmsModel GetInstantSms()
        {
            CampaignDb _prc = new CampaignDb();
            DataTable dtSmstype = _prc.getsmstypelist();
            DataTable dtstatuslist = _prc.getstatuslist();
            DataTable dttemplateList = _prc.getTemplateId();

            SelectListItem selectListItem = new SelectListItem { Text = "-Select-", Value = "" };
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(selectListItem);

            return new InstantSmsModel()
            {
                SmsTypes = dtSmstype.ToSelectList(listItems, "VALUE", "TEXT"),
                StatusList = dtstatuslist.ToSelectList(listItems, "VALUE", "TEXT"),
                TemplateList = dttemplateList.ToSelectList(listItems, "VALUE", "TEXT"),
                Senders = GetSelectListDefault(),
                Templates = GetSelectListDefault()
            };
        }

        private SelectList GetSelectListDefault()
        {
            List<SelectListItem> senders = new List<SelectListItem>()
            {
                new SelectListItem { Text = "-- Select --", Value = "0" }
            };
            return new SelectList(senders, "VALUE", "TEXT");
        }

        public SmsCountModel GetSmsContent(string smsContent, int templateId)
        {
            long smsCount, length;
            CampaignDb _prc = new CampaignDb();
            _prc.GetSmsCount(smsContent, templateId, out smsCount, out length);

            return new SmsCountModel() { SmsLeg = smsCount, SmsLength = length };
        }


        public string SendInstantSms(InstantSmsCommand command, out string response)
        {
            response = "";
          
            var requestJson = JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            CampaignDb _prc = new CampaignDb();
            return _prc.SendInstantSms(requestJson, out response);
        }

    }
}