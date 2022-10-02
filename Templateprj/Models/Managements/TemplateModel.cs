using Newtonsoft.Json;
using System.Web.Mvc;

namespace Templateprj.Models.Managements
{
    public class TemplateModel
    {
        public string TemplateName { get; set; }
        public int TemplateTypeId { get; set; }
        public int TemplateId { get; set; }
        public int HeaderSenderId { get; set; }
        public int StatusId { get; set; }
        public int ContentTypeId { get; set; }
      
        [JsonIgnore]
        public SelectList TemplateTypes { get; set; }

        [JsonIgnore]
        public SelectList TemplateIds { get; set; }

        [JsonIgnore]
        public SelectList HeaderSenders { get; set; }

        [JsonIgnore]
        public SelectList Status { get; set; }

        [JsonIgnore]
        public SelectList ContentTypes { get; set; }
    }
}