using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Models.Managements
{
    public class ManagementModel
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountID { get; set; }
        public string BuildingNumber { get; set; }
        public string PrincipalEntityId { get; set; }
        public string DltRegisteredAccount { get; set; }
        public string AccountExpiry { get; set; }
        public string AccountExpiryDescription { get; set; }
        public DateTime? AccountExpiryDate { get; set; }
        public string DltAddressDetails { get; set; }
        public string DltEmailAddress { get; set; }
    }
}