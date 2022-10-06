using System.Collections.Generic;
using System.Web.Mvc;
using Templateprj.Models;
using Templateprj.Models.InstantSms;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ITemplateManagemntRepository
    {
        TemplateModel GetTemplateFilters();
        List<KeyValueModel> GetTemplateNames();
        List<KeyValueModel> GetTemplateIds();
        List<KeyValueModel> GetTemplateHeaders();
        string GetTemplates(TemplateModel model);
        string SaveTemplate(RegisterTemplateCommand command, out string response);
        string DeleteTemplate(string id, out string response);
    }
}