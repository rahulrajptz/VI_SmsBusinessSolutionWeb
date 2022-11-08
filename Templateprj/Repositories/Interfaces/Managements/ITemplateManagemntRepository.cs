using System.Collections.Generic;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ITemplateManagemntRepository
    {
        TemplateModel GetTemplateFilters(bool excludeAll = false);
        TemplateAutoFilItemModel TemplateAutoFilItems();
        string SaveTemplate(List<RegisterTemplateCommand> commands, out string response, out string data);
        string UpdateTemplate(UpdateTemplateCommand command, out string response);
        string DeleteTemplate(int id, out string response);
        string GetTemplates(TemplateModel model);
        RegisterTemplateCommand GetTemplateById(int id);
    }
}