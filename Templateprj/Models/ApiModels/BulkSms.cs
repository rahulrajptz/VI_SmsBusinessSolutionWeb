using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Templateprj.Models.ApiModels
{

    public class BulkSms
    {
        [Required]
        [Display(Name = "Campain Id")]
        public string CampaignId { get; set; }

        [Required]
        [Display(Name = "List of Msisdn")]
        public List<MSISDNumber> Msisdns { get; set; }

    }
}