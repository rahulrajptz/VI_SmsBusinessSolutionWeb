using Newtonsoft.Json;

namespace Templateprj.Models.Managements
{
    public class RegisterTemplateCommand
    {
        public long? Id { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TeleMarketer { get; set; }
        public int? Type { get; set; }
        public int? Header { get; set; }
        public string Category { get; set; }
        public string RegisteredDlt { get; set; }
        public string RequestedOn { get; set; }
        public string StatusDate { get; set; }
        public string CreatedBy { get; set; }
        public string BlackListedBy { get; set; }
        public int ApprovalStatus { get; set; }
        public int Status { get; set; }
        public string TemplateMessage { get; set; }
        public int ConsentType { get; set; }
        public string Reason { get; set; }
        public string ContentType { get; set; }
        public int UnicodeStatus { get {
            return !string.IsNullOrEmpty(TemplateMessage) 
                   && System.Text.ASCIIEncoding.GetEncoding(0).GetString(System.Text.ASCIIEncoding.GetEncoding(0).GetBytes(TemplateMessage)) != TemplateMessage?8:0;
            }  }
        public string ValidityPeriod { get; set; }
        public int DeliveryStatus { get; set; }
        public string VariableCount { get; set; }

        [JsonIgnore]
        public TemplateModel Masters { get; set; }

    }
}