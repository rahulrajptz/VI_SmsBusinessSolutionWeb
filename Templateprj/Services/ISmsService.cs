using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Templateprj.Models;
using Templateprj.Models.ApiModels;

namespace Templateprj.Services
{
    public interface ISmsService
    {
        APIResponse ProcessInput(string clientIpAddress, string apikey, BulkSms msisdn);
    }
}