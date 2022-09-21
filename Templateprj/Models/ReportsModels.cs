using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Models
{
    public class feedbackreportmodel
    {
        public string interval { get; set; }
        public SelectList intervalList { get; set; }

        public string reportType { get; set; }
        public SelectList reportTypeList { get; set; }

        public string campaignName { get; set; }
        public SelectList campaignNameList { get; set; }
        public string agent { get; set; }
        public SelectList agentList { get; set; }
        [Display(Name = "From Date")]
        [Required]
        public string fromdate { get; set; }
        [Display(Name = "To Date")]
        [Required]
        public string todate { get; set; }

    }

    public class DeatailedReportModel
    {
        [Display(Name = "Campaign Name")]
        public string SelectedId { get; set; }
        public SelectList IdList { get; set; }
        public string ReportNmae { get; set; }
        [Display(Name = "From Date")]
        public string fromdate { get; set; }
        [Display(Name = "To Date")]
        public string todate { get; set; }
    }

}