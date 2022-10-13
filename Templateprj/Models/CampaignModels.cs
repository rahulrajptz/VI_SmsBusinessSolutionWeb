using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Templateprj.Models
{

    public class insightModel
    {
        public string Agent { get; set; }
        public string interval { get; set; }
        public SelectList intervalList { get; set; }
        public SelectList AgentList { get; set; }
        public string averageCallLength { get; set; }
        public string callLengthPercentage { get; set; }
        public string callLenghtIncreaseFlag { get; set; }
        public string numberofCalls { get; set; }
        public string numberofCallsPercentage { get; set; }
        public string callIncreaseFlag { get; set; }
    }

    public class CampaignModels
    {
    }
    public class CampaignviewModel
    {
        public string interval { get; set; }
        public SelectList intervalList { get; set; }

        public string status { get; set; }
        public SelectList statusList { get; set; }


    }
    public class CampaignModel
    {
        public string campaignStep { get; set; }
        public string campaignId { get; set; }

        public string campaignName { get; set; }
        public string campaignDescription { get; set; }
        public string callType { get; set; }
        public SelectList callTypeList { get; set; }

        //public string allocation { get; set; }
        //public SelectList allocationList { get; set; }

        public bool numberMasking { get; set; }
        //public SelectList numberMaskingList { get; set; }
        public bool customFields { get; set; }
        public bool agentMapping { get; set; }

        public string feedbackType { get; set; }
        public SelectList feedbackTypeList { get; set; }

        public string feedbackUrl { get; set; }

        public List<dynamicFields> dynamic { get; set; }
        public List<questions> question { get; set; }

        public string deletedQuestion { get; set; }
        public string deletedField { get; set; }


    }


    public class dynamicFields
    {
        public string fieldId { get; set; }
        public string header { get; set; }
        public string contentType { get; set; }
        public SelectList contentTypeList { get; set; }

    }

    public class questions
    {
        public string questionId { get; set; }
        public string questionType { get; set; }
        [XmlIgnore]
        public SelectList questionTypeList { get; set; }

        public string questionValue { get; set; }
        public List<Choice> multipleChoices { get; set; }
        public string deletedChoice { get; set; }


    }
    public class Choice
    {
        public string choiceId { get; set; }

        [Required]
        public string choiceValue { get; set; }
    }
    public class AgentManagementModel
    {
        //[Required]
        public string type { get; set; }

        public SelectList TypeList { get; set; }
        //  [Required]
        public string firstname { get; set; }
        //  [Required]
        public string lastname { get; set; }
        //  [Required]
        public string mobilenumber { get; set; }
        //   [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$")]
        public string emailid { get; set; }
        // [Required]
        public string typeedit { get; set; }
        // [Required]
        public string firstnameedit { get; set; }
        //  [Required]
        public string lastnameedit { get; set; }
        // [Required]
        public string mobilenumberedit { get; set; }
        //  [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$")]
        public string emailidedit { get; set; }

        public string agentid { get; set; }
    }

    public class basefile
    {
        public string customerName { get; set; }
        public string customerNumber { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }

        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string field10 { get; set; }


        public string agentMapping { get; set; }


    }
    public class baseuploadModel
    {
        public string Id { get; set; }
        public string campaignName { get; set; }
        public string campaignDescription { get; set; }
        public string callType { get; set; }
        public bool numberMasking { get; set; }
        public string numberMaskingValue { get; set; }
        public bool agentMapping { get; set; }
        public string agentMappingValue { get; set; }

        public string errordata { get; set; }

    }
    public class campaignResponseModel
    {
        public string message { get; set; }
        public string campaignId { get; set; }
        public string status { get; set; }
    }

    public class bulkAgent
    {
        public string agentType { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNumber { get; set; }
        public string email { get; set; }


    }
    public class bulkagentModel
    {
        public string errordata { get; set; }
    }


    public class SMSCampaignModel
    {
        

        [Required]
        public string campaignName { get; set; }
        public string campaignId { get; set; }
        [Required]
        public int campaignType { get; set; }
        public SelectList campaignTypeList { get; set; }
        [Required]
        public string campaignStartTime { get; set; }
        public SelectList campaignStartTimeList { get; set; }
        [Required]
        public string fromDate { get; set; }
        [Required]
        public string toDate { get; set; }
        [Required]
        public string fromTime { get; set; }
        [Required]
        public string toTime { get; set; }
        [Required]
        public string priority { get; set; }
        public SelectList priorityList { get; set; }
        [Required]
        public string gateway { get; set; }
        public SelectList gatewayList { get; set; }
        [Required]
        public string tps { get; set; }
        public SelectList tpsList { get; set; }

        [Required]
        public int smsType { get; set; }
        public SelectList smsTypeList { get; set; }



        [Required]
        public int senderId { get; set; }
        public SelectList senderIdList { get; set; }

        [Required]
        public int templateId { get; set; }
        public SelectList templateIdList { get; set; }

        public string smsContent { get; set; }


        public List<variableList> SMSvariable { get; set; }

        public List<testSMS> SMSTest { get; set; }




        [JsonIgnore]
        public int listcampaignName { get; set; }
        [JsonIgnore]
        public SelectList listcampaignNameList { get; set; }
        [JsonIgnore]
        public string listCreatedDate { get; set; }
        [JsonIgnore]
        public int listCampaignStatus { get; set; }
        [JsonIgnore]
        public SelectList listCampaignStatusList { get; set; }
        [JsonIgnore]
        public int listCampaignPriority { get; set; }
        [JsonIgnore]
        public SelectList listCampaignPriorityList { get; set; }
        [JsonIgnore]
        public int listCampaignType { get; set; }
        [JsonIgnore]
        public SelectList listCampaignTypeList { get; set; }



        [JsonIgnore]
        public string uploadCampaignName { get; set; }
        [JsonIgnore]
        public SelectList uploadCampaignNameList { get; set; }

        [JsonIgnore]
        public string uploadCampaignstarttype { get; set; }
        [JsonIgnore]
        public SelectList uploadCampaignstarttypeList { get; set; }
        [JsonIgnore]
        public string scheduleDate { get; set; }
        [Display(Name = "Priority")]
        public string uploadpriority { get; set; }
        //[JsonIgnore]
        public SelectList uploadpriorityList { get; set; }







        [JsonIgnore]
        public int statuscampaignName { get; set; }
        [JsonIgnore]
        public SelectList statuscampaignNameList { get; set; }
        [JsonIgnore]
        public string statusCreatedDate { get; set; }
        [JsonIgnore]
        public int statusCampaignStatus { get; set; }
        [JsonIgnore]
        public SelectList statusCampaignStatusList { get; set; }
        [JsonIgnore]
        public int statusCampaignType { get; set; }
        [JsonIgnore]
        public SelectList statusCampaignTypeList { get; set; }
        [JsonIgnore]
        //[Required]
        [Display(Name = "Priority")]
        public int statusCampaignPriority { get; set; }
        [JsonIgnore]
        public SelectList statusPriorityList { get; set; }


        [JsonIgnore]
        public string reportcampaignName { get; set; }
        [JsonIgnore]
        public SelectList reportcampaignNameList { get; set; }
        [JsonIgnore]
        public string reportCreatedDate { get; set; }
        [JsonIgnore]
        public string reportCampaignStatus { get; set; }
        [JsonIgnore]
        public SelectList reportCampaignStatusList { get; set; }
        [JsonIgnore]
        //[Required]
        [Display(Name = "Priority")]
        public string reportCampaignPriority { get; set; }
        [JsonIgnore]
        public SelectList reportCampaignPriorityList { get; set; }


    }

    public class InstantSmsReportModel
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string MSISDN { get; set; }
        public int TemplateId { get; set; }
        public SelectList Templates { get; set; }
        public int reportStatus { get; set; }
        public SelectList reportStatusList { get; set; }
    }

    public class variableList
    {
        public string variableName { get; set; }
        public string renameVariable { get; set; }
    }

    public class testSMS
    {
        public string mobileNumber { get; set; }

        public List<data> variableData { get; set; }
        public string message { get; set; }
        public string smsId { get; set; }

    }
    public class data
    {
        public string variableLabel { get; set; }

        public string variableContent{ get; set; }

    }
}