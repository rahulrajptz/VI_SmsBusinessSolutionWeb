namespace Templateprj.Models.Managements
{
    public class AddSenderModel
    {
        public long? SmId { get; set; }
        public string HeaderId { get; set; }
        public string SenderId { get; set; }
        public string TeleMarketer { get; set; }
        public string Explanation { get; set; }
        public string RegisteredTsp { get; set; }
        public string RequestedDate { get; set; }
        public string StatusDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string BlackListedBy { get; set; }

    }
}