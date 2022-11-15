using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Templateprj.Models.Managements
{
    public class TemplateBulkModel
    {
        
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TeleMarketer { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public string Category { get; set; }
        public string RegisteredDlt { get; set; }
        public string RequestedOn { get; set; }
        public string StatusDate { get; set; }
        public string CreatedBy { get; set; }
        public string BlackListedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string Status { get; set; }

     

        public string TemplateMessage { get; set; }
        public int ConsentType { get; set; }
        public string Reason { get; set; }
        public int ContentType { get; set; }
        public int UnicodeStatus
        {
            get
            {
                return !string.IsNullOrEmpty(TemplateMessage)
                       && System.Text.ASCIIEncoding.GetEncoding(0).GetString(System.Text.ASCIIEncoding.GetEncoding(0).GetBytes(TemplateMessage)) != TemplateMessage ? 8 : 0;
            }
        }

        [JsonIgnore]
        public int? ExistingUnicodeStatus { get; set; }

        public string ValidityPeriod { get; set; }
        public int DeliveryStatus { get; set; }
        public string VariableCount { get; set; }


    }
}