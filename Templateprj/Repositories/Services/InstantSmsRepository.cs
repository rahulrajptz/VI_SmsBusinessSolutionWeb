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
            SelectListItem selectListItem = new SelectListItem { Text = "-Select-", Value = "" };
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(selectListItem);

            SelectListItem selectListItemAll = new SelectListItem { Text = "-All-", Value = "0" };
            List<SelectListItem> listItemsAll = new List<SelectListItem>();
            listItemsAll.Add(selectListItemAll);

            SelectListItem selectListItems = new SelectListItem { Text = "-All-", Value = "10" };
            List<SelectListItem> AlllistItems = new List<SelectListItem>();
            AlllistItems.Add(selectListItems);

            DataTable dtSmstype = _prc.getsmstypelist();
            DataTable dtstatuslist = _prc.getInstantstatuslist();
            DataTable dttemplateList = _prc.getTemplateId();
            DataSet dspopupData = _prc.getTemplateSearchDetails();

            
            return new InstantSmsModel()
            {
                SmsTypes = dtSmstype.ToSelectList(listItems, "VALUE", "TEXT"),
                StatusList = dtstatuslist.ToSelectList(AlllistItems, "VALUE", "TEXT"),
                TemplateList = dttemplateList.ToSelectList(listItemsAll, "VALUE", "TEXT"),
                pouptemplateNameList = dspopupData.Tables[0].ToSelectList(listItemsAll, "VALUE", "TEXT"),
                pouptemplateTypeList = dspopupData.Tables[1].ToSelectList(listItemsAll, "VALUE", "TEXT"),
                poupContentTypeList = dspopupData.Tables[3].ToSelectList(listItemsAll, "VALUE", "TEXT"),
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