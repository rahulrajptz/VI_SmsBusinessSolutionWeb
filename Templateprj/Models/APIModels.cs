using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Templateprj.Models
{
    public class APIResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string CampaignId { get; set; }

        public APIResponse()
        {
            Status = "Failed";
            Message = "Please try after some time";
            CampaignId = "";
        }


    }
    public class APIResponse1
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Templateid { get; set; }

        public APIResponse1()
        {
            Status = "Failed";
            Message = "Please try after some time";
            Templateid = "";
        }


    }
    public class APIRespnse
    {
        public string Status { get; set; }
        public string Message { get; set; }


        public APIRespnse()
        {
            Status = "Failed";
            Message = "Please try after some time";

        }
    }

    public class APIResponsesenderid
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string RefId { get; set; }


        public APIResponsesenderid()
        {
            Status = "Failed";
            Message = "Please try after some time";
            RefId = "";

        }
    }

    public class APIRespnseBala
    {
        public string Status { get; set; }
        public string Balance { get; set; }


        public APIRespnseBala()
        {
            Status = "Failed";
            Balance = "";

        }
    }
    public class APIRespnsesms
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string uniqueid { get; set; }
        // public string invalidMsisdn { get; set; }


        public APIRespnsesms()
        {
            Status = "Failed";
            Message = "Please try after some time";
            uniqueid = " ";
            //invalidMsisdn = " ";


        }
    }
    //    [{
    //"process" : "Create_Campaign", "Request_ID" : "UniqueID",
    //"Campaign_Name" : "Buzibee_Promotion2",
    //"Campaign_Type" : "service implicit",
    //"From_Date" : "28/12/2022",
    //"To_Date" : "31/12/2022",
    //"From_Time" : "09:00 AM",
    //"to_Time" : "09:00 PM",
    //"Template_ID" : "1007489473751090213",
    //"pingbackurl" : "http://cts.myvi.in?",
    //"content_type": "Dyamic",
    //"Number_of_variables": "5",
    //"Variable1_header": "VarA",
    //"Variable2_header": "VarB",
    //"Variable3_header": "VarC",
    //"Variable4_header": "VarD",
    //"Variable5_header": "VarE",
    //"Variable6_header": "",
    //"Variable7_header": "",
    //"Variable8_header": "",
    //"Variable9_header": "",
    //"Variable10_header": ""
    //}]

    //    [{
    //"process" : "Create_Campaign", "Request_ID" : "UniqueID",
    //"Campaign_Name" : "Buzibee_Promotion15",
    //"Campaign_Type" : "service implicit",
    //"From_Date" : "28/12/2022",
    //"To_Date" : "31/12/2022",
    //"From_Time" : "09:00 AM",
    //"to_Time" : "09:00 PM",
    //"Template_ID" : "1007489473751090213",
    //"pingbackurl" : "http://cts.myvi.in?",
    //"content_type": "Dyamic",
    //"Number_of_variables": "5",
    //"SMSvariable": [
    //            {
    //                "variableName": "VAR1",
    //                "renameVariable": "VARA"
    //            },
    //            {
    //                "variableName": "VAR2",
    //                "renameVariable": "VARB"
    //            },
    //            {
    //                "variableName": "VAR3",
    //                "renameVariable": "VARC"
    //            },
    //            {
    //                "variableName": "VAR4",
    //                "renameVariable": "VARD"
    //            },
    //            {
    //                "variableName": "VAR5",
    //                "renameVariable": "VARE"
    //            }
    //        ]
    //}]
    public class Create_Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
        // public string Process { get; set; }
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
        public string Header { get; set; }
        public string Unicode { get; set; }



    }
    public class campaignmodel
    {

        //  public string process { get; set; }
        public string RequestID { get; set; }
        public string CampaignName { get; set; }
        //public string Campaign_Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string TemplateID { get; set; }
        public string pingbackurl { get; set; }
        //public string content_type { get; set; }
        //public string Number_of_variables { get; set; }
        //public List<SMSvariable> SMSvariable { get; set; }

        //public string Variable1_header { get; set; }
        //public string Variable2_header { get; set; }
        //public string Variable3_header { get; set; }
        //public string Variable4_header { get; set; }
        //public string Variable5_header { get; set; }
        //public string Variable6_header { get; set; }
        //public string Variable7_header { get; set; }
        //public string Variable8_header { get; set; }
        //public string Variable9_header { get; set; }
        //public string Variable10_header { get; set; }

    }
    public class SMSvariable
    {
        public string variableName { get; set; }
        public string renameVariable { get; set; }

    }
    //    [{
    //	"Process": "upload_base_data",
    //	"RequestID": "234",
    //	"MasterCampaignID": "256",
    //	"ContentType": "Dyamic",
    //	"NumberOfVariables": "5",
    //	"PingbackUrl": "https://cts.myvi.in",
    //	"BulkData": [{
    //			"Msisdn": "7029173043",
    //			"VAR1": "1234567890",
    //			"VAR2": "CustID",
    //			"VAR3": "8888888888",
    //			"VAR4": "test",
    //			"VAR5": "test1",
    //			"VAR6": "",
    //			"VAR7": "",
    //			"VAR8": "",
    //			"VAR9": "",
    //			"VAR10": ""

    //        },
    //		{
    //			"Msisdn": "7029173043",
    //			"VAR1": "1234567891",
    //			"VAR2": "CustID1",
    //			"VAR3": "88888888881",
    //			"VAR4": "test",
    //			"VAR5": "test1",
    //			"VAR6": "",
    //			"VAR7": "",
    //			"VAR8": "",
    //			"VAR9": "",
    //			"VAR10": ""
    //		},
    //		{
    //			"Msisdn": "7029173043",
    //			"VAR1": "1234567892",
    //			"VAR2": "CustID2",
    //			"VAR3": "88888888882",
    //			"VAR4": "test",
    //			"VAR5": "test1",
    //			"VAR6": "",
    //			"VAR7": "",
    //			"VAR8": "",
    //			"VAR9": "",
    //			"VAR10": ""
    //		}
    //	]
    //}]
    public class Insertbulksmsmodel
    {
        //public string Process { get; set; }
        public string RequestID { get; set; }
        public string MasterCampaignID { get; set; }
        // public string ContentType { get; set; }
        //public string NumberOfVariables { get; set; }
        //public string PingbackUrl { get; set; }
        //public string BulkData { get; set; }
        public List<BulkData> BulkData { get; set; }

    }

    public class BulkData
    {
        public string Msisdn { get; set; }
        public string VAR1 { get; set; }
        public string VAR2 { get; set; }
        public string VAR3 { get; set; }
        public string VAR4 { get; set; }
        public string VAR5 { get; set; }
        public string VAR6 { get; set; }

        public string VAR7 { get; set; }
        public string VAR8 { get; set; }

        public string VAR9 { get; set; }

        public string VAR10 { get; set; }

        public string VARU1 { get; set; }
        public string VARU2 { get; set; }
        public string VARU3 { get; set; }
        public string VARU4 { get; set; }
        public string VARU5 { get; set; }
        public string VARU6 { get; set; }
        public string VARU7 { get; set; }
        public string VARU8 { get; set; }
        public string VARU9 { get; set; }
        public string VARU10 { get; set; }

    }
    public class Campaign
    {




        public string otp { get; set; }
        [Required]
        [Display(Name = "Campaign Name")]
        public string campname { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public string fromdt { get; set; }
        [Required]
        [Display(Name = "To Date")]
        public string todt { get; set; }

        [Required]
        [Display(Name = "From Time")]
        public string fromtime { get; set; }

        [Required]
        [Display(Name = "To Time")]
        public string totime { get; set; }

        [Required]
        [Display(Name = "Campaign Type")]
        public string type { get; set; }

        [Required]
        [Display(Name = "SenderID")]
        public string senderid { get; set; }

        [Required]
        [Display(Name = "TemplateID")]
        public string templateid { get; set; }
        [Required]

        public string script { get; set; }
        public string pingbackurl { get; set; }
    }


    public class MSISDNBase
    {
        public string campaignid { get; set; }

        public string MSISDN { get; set; }
        public List<Data> Data { get; set; }
    }
    public class SMSDelivery
    {
        public string Msisdn { get; set; }
        public string Deliverystatus { get; set; }
        public string MessageID { get; set; }
        public string DeliveryTime { get; set; }
    }
    public class Data
    {
        public string msisdn { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }
        public string param6 { get; set; }
        public string param7 { get; set; }
        public string param8 { get; set; }
        public string param9 { get; set; }
        public string param10 { get; set; }
    }
    public class SMSCreate
    {
        public string msisdn { get; set; }
        public string smstype { get; set; }
        public string script { get; set; }
        public string senderid { get; set; }
        public string templateid { get; set; }
        public string unicode { get; set; }
        public string pingbackurl { get; set; }
    }
    public class DeliveryFetch
    {
        public string UniqueID { get; set; }
    }
    public class script1
    {
        public string Script { get; set; }
        public string Unicodestatus { get; set; }
        public string Pingbackurl { get; set; }
        public string DLTtemplateid { get; set; }
        public string TemplateName { get; set; }
        public string VariableCount { get; set; }
        public string SenderId { get; set; }
        public string SMSType { get; set; }
        public string Validity { get; set; }
        public string DeliveryFlag { get; set; }
    }
    public class SMSTemplateFetch
    {
        public string templateid { get; set; }
    }

    public class Senderid
    {
        public string PrincipleId { get; set; }
        public string SenderId { get; set; }
    }
}

