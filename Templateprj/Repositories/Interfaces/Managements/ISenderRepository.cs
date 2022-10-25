using Templateprj.Models.InstantSms;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ISenderRepository
    {
        ManagementModel GetAccount();
        string SaveAccount(ManagementModel model, out string response);
    }
}