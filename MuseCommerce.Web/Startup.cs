using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Owin;

[assembly: OwinStartupAttribute(typeof(MuseCommerce.Web.Startup))]
namespace MuseCommerce.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var hubConfiguration = new HubConfiguration { };

            app.MapSignalR("/signalr", hubConfiguration);


            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource));

            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);

            Logger.SetLogWriter(logWriterFactory.Create());

            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(configurationSource);

            ExceptionPolicy.SetExceptionManager(factory.CreateManager());

            //var platformWebBootstrapper = new PlatformWebBootstrapper();
            //platformWebBootstrapper.Run();

            //var container = platformWebBootstrapper.Container;

            //container.RegisterInstance(app);

            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}
