using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Models.InstantSms
{
    public class InstantSmsModel
    {
        [Required]
        public int SmsTypeId { get; set; }
        public SelectList SmsTypes { get; set; }
        [Required]
        public int SenderId { get; set; }
        public SelectList Senders{ get; set; }
        [Required]
        public int TemplateId { get; set; }
        public SelectList Templates { get; set; }
        public string SmsContent { get; set; }

        [Required]
        public string dateFrom { get; set; }
        //public string dateTo { get; set; }
        public string MSISDN { get; set; }
        [Required]
        public string template { get; set; }
        public SelectList TemplateList { get; set; }
        [Required]
        public int reportStatus { get; set; }
        public SelectList StatusList { get; set; }
    }
}