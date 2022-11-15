using System.Collections.Generic;
using Templateprj.Models.Managements;

namespace Templateprj.Repositories.Interfaces
{
    public interface ISenderRepository
    {
        string GetSenderIds();

        AddSenderModel GetSenderIdById(int id);

        string SaveSenderId(List<AddSenderModel> commands, out string response, out string data);
        string DeleteSenderId(int id, out string response);
        string UpdateSenderId(UpdateSenderIdCommand command, out string response);

    }
}