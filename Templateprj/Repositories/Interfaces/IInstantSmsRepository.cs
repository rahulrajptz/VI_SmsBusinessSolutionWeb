using Templateprj.Models.InstantSms;

namespace Templateprj.Repositories.Interfaces
{
    public interface IInstantSmsRepository
    {
        InstantSmsModel GetInstantSms();

        SmsCountModel GetSmsContent(string smsContent, int templateId);

        string SendInstantSms(InstantSmsCommand command, out string response);
        //string getstatusReport(InstantSmsCommand command, out string response);
    }
}