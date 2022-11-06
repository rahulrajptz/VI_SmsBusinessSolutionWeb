using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        private string _templateMessage;

        public string TemplateMessage
        {
            get
            {
                if (this.ExistingUnicodeStatus.HasValue && this.ExistingUnicodeStatus == 8)
                {
                    //  string correctString = "";
                    if (_templateMessage.Trim() != "")
                    {
                        //  correctString = str.Replace("[PARAMETER]", "005B0050004100520041004D0045005400450052005D");
                        _templateMessage = "\\u" + Regex.Replace(_templateMessage, ".{4}", "$0\\u");
                        _templateMessage = Regex.Unescape(_templateMessage.Substring(0, _templateMessage.Length - 2));
                    }
                }
                else if(!string.IsNullOrEmpty(_templateMessage))
                {
                    _templateMessage = _templateMessage.Replace("\r\n", "");
                }
                return _templateMessage;
            }
            set
            {
                _templateMessage = value;
            }
        }
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

        [JsonIgnore]
        public TemplateModel Masters { get; set; }

    }
}