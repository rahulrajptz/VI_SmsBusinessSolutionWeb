using System.Web.Mvc;
using Templateprj.Repositories.Interfaces;
using Templateprj.Repositories.Services;
using Templateprj.Services;
using Unity;
using Unity.AspNet.Mvc;

namespace Templateprj.Ioc
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IInstantSmsRepository, InstantSmsRepository>();
            container.RegisterType<ISmsService, SmsService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}