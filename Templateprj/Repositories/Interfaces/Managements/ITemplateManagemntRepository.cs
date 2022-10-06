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
        string GetTemplates(TemplateModel model);
        string SaveAccount(ManagementModel model, out string response);
        string DeleteTemplate(string id, out string response);
    }
}