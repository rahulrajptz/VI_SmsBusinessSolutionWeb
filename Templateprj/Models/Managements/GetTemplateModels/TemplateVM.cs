using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Templateprj.Models.Managements.GetTemplateModels
{
    public class TemplateVM
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public string StatusDate { get; set; }
        public string TemplateId { get; set; }
        public string ConsentType { get; set; }
        public string ContentType { get; set; }
        public string RequestedOn { get; set; }
        public string TeleMarketer { get; set; }
        public string TemplateName { get; set; }
        public string BlackListedBy { get; set; }
        public string RegisteredDlt { get; set; }
        public string VariableCount { get; set; }
        public string ApprovalStatus { get; set; }
        public string ValidityPeriod { get; set; }
       
        private string _templateMessage;

        public string TemplateMessage {
            get
            {
                if (this.UnicodeStatus.HasValue && this.UnicodeStatus == 8)
                {
                    if (_templateMessage.Trim() != "")
                    {
                        _templateMessage = "\\u" + Regex.Replace(_templateMessage, ".{4}", "$0\\u");
                        _templateMessage = Regex.Unescape(_templateMessage.Substring(0, _templateMessage.Length - 2));
                    }
                }
                else
                {
                    _templateMessage= _templateMessage.Replace("\r\n", "");
                }
                return _templateMessage;
            }
            set
            {
                _templateMessage = value;
            }
        }

        public int? UnicodeStatus { get; set; }
    }
}