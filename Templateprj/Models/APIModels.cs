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

                            //                   IN V_Template_Name varchar(1000),
                            //                    IN N_variable_count int, 
                            //                    IN N_senderId int, 
                            //                    IN N_SMS_Type int, 
                            //                    IN V_validity int, 
                            //                    IN N_Delivery_Flag int, 
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

