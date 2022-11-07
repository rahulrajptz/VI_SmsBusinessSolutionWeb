using System.Collections.Generic;

namespace Templateprj.Models.Managements.GetTemplateModels
{
    public class TemplateResponse
    {
        public List<Header> Header { get; set; }
        public List<TemplateVM> Data { get; set; }
        public int? Count { get; set; }
    }
}