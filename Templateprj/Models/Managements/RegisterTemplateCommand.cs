using Newtonsoft.Json;
using System.Web.Mvc;

namespace Templateprj.Models.Managements
{
    public class RegisterTemplateCommand
    {
        public long? Id { get; set; }
        public string TemplateName { get; set; }
        public string TeleMarketer { get; set; }
        public string TemplateId { get; set; }
        public int? Type { get; set; }
        public string Header { get; set; }
        public int? Category { get; set; }
        public int? RegisteredDlt { get; set; }
        public string RequestedOn { get; set; }
        public string StatusDate { get; set; }
        public string CreatedBy { get; set; }
        public string BlackListedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string Status { get; set; }
        public string TemaplteMessage { get; set; }
        public string ConsentType { get; set; }
        public string Reason { get; set; }
      
    }
}