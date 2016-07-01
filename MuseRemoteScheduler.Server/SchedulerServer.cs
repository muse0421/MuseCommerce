using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseRemoteScheduler.Server
{
    public class SchedulerServer
    {
        StdSchedulerFactory schedulerFactory;

        public SchedulerServer()
        {
            schedulerFactory = new StdSchedulerFactory();
        }

        public void Start()
        {
            Console.WriteLine("Starting scheduler...");

            var scheduler = schedulerFactory.GetScheduler();

           
            scheduler.Start();
        }

        public void Stop()
        {
            if (schedulerFactory != null)
            {
                var scheduler = schedulerFactory.GetScheduler();
                scheduler.Shutdown(true);
            }
        }
    }
}
