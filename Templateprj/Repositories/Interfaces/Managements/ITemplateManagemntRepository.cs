using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ITemplateManagemntRepository
    {
        TemplateModel GetTemplateFilters();
        TemplateAutoFilItemModel TemplateAutoFilItems();
        string GetTemplates(TemplateModel model);
        string SaveTemplate(RegisterTemplateCommand command, out string response);
        string DeleteTemplate(string id, out string response);
    }
}