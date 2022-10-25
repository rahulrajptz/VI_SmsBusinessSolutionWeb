using Newtonsoft.Json;
using System.Web.Mvc;

namespace Templateprj.Models.Managements
{
    public class TemplateModel
    {
        public string TemplateName { get; set; }
        public int TemplateTypeId { get; set; }
        public int StatusId { get; set; }
        public int ContentTypeId { get; set; }
        public string TemplateId { get; set; }
        public string HeaderSenderId { get; set; }

        [JsonIgnore]
        public SelectList TemplateTypes { get; set; }
   
        [JsonIgnore]
        public SelectList Status { get; set; }

        [JsonIgnore]
        public SelectList ContentTypes { get; set; }
        [JsonIgnore]
        public SelectList ApprovalStatus { get; set; }   
        [JsonIgnore]
        public SelectList ConsentType { get; set; }

        [JsonIgnore]
        public SelectList Headers { get; set; }
      
        [JsonIgnore]
        public SelectList DeliveryStatus { get; set; }

        [JsonIgnore]
        public int ConsentTypeId { get; set; }

        [JsonIgnore]
        public int ApprovalStatusId { get; set; }

        [JsonIgnore]
        public int DeliveryStatusId { get; set; }

        [JsonIgnore]
        public int HeaderId { get; set; }

    }
}