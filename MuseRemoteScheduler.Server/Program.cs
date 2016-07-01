using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MuseCommerce.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace MuseRemoteScheduler.Server
{
    class Program
    {
        static void Main(string[] args)
        {            
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource));

            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);

            Logger.SetLogWriter(logWriterFactory.Create());

            HostFactory.Run(x =>                                 
            {
                x.Service<SchedulerServer>(s =>                       
                {
                    s.ConstructUsing(name => new SchedulerServer());     
                    s.WhenStarted(tc => tc.Start());            
                    s.WhenStopped(tc => tc.Stop());              
                });
                x.RunAsLocalSystem();                                         
                x.SetDescription("SchedulerServer Topshelf Host");      
                x.SetDisplayName("SchedulerServer");                     
                x.SetServiceName("SchedulerServer");                     
            });    
        }
    }
}
