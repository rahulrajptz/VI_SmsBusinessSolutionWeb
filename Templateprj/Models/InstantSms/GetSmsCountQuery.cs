namespace Templateprj.Models.InstantSms
{
    public class GetSmsCountQuery
    {
        public string SMSContent { get; set; }
        public int TemplateId { get; set; }
        public int reportStatus { get; set; }
        public string unicodeStatus { get; set; }
        public string DBMessage { get; set; }

    }
}