using System.Collections.Generic;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ITemplateManagemntRepository
    {
        TemplateModel GetTemplateFilters();
        TemplateAutoFilItemModel TemplateAutoFilItems();
        string GetTemplates(TemplateModel model);
        string SaveTemplate(List<RegisterTemplateCommand> commands, out string response);
        string DeleteTemplate(string id, out string response);
        string GetTemplateFilters(TemplateModel model);
    }
}