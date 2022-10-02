using Templateprj.Models.InstantSms;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface IAccountManagemntRepository
    {
        ManagementModel GetAccount();
        string SaveAccount(ManagementModel model, out string response);
    }
}