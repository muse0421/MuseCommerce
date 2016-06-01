using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using MuseCommerce.Web.SignalR;
using Owin;

[assembly: OwinStartupAttribute(typeof(MuseCommerce.Web.Startup))]
namespace MuseCommerce.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var hubConfiguration = new HubConfiguration {  };
           
            app.MapSignalR("/signalr", hubConfiguration);
            
        }
    }
}
