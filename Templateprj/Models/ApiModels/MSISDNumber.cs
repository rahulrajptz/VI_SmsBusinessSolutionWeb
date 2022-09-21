using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Templateprj.Models.ApiModels
{

    public class MSISDNumber
    {
      
        [Required]
        [Display(Name = "MSISDN")]
        public string Msisdn { get; set; }

        [Required]
        [Display(Name = "VAR1")]
        public string Var1 { get; set; }

    
        [Required]
        [Display(Name = "VAR2")]
        public string Var2 { get; set; }

        
        [Required]
        [Display(Name = "VAR3")]
        public string Var3 { get; set; }

    
        [Required]
        [Display(Name = "VAR4")]
        public string Var4 { get; set; }
        
        [Required]
        [Display(Name = "VAR5")]
        public string Var5 { get; set; }

    
        [Required]
        [Display(Name = "VAR6")]
        public string Var6 { get; set; }

        [Required]
        [Display(Name = "VAR7")]
        public string Var7 { get; set; }


        [Required]
        [Display(Name = "VAR8")]
        public string Var8 { get; set; }


        [Required]
        [Display(Name = "VAR9")]
        public string Var9{ get; set; }


        [Required]
        [Display(Name = "VAR10")]
        public string Var10{ get; set; }

    }
}