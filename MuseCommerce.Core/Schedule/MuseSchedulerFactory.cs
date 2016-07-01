using MuseCommerce.Core.Log;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Schedule
{
    public class MuseSchedulerFactory
    {
        StdSchedulerFactory schedulerFactory;
        IScheduler scheduler;

        public MuseSchedulerFactory()
        {
            schedulerFactory = new StdSchedulerFactory();
            Console.WriteLine("Starting scheduler...");
            scheduler = schedulerFactory.GetScheduler();        
        }

        public void Start()
        {                
            scheduler.Start();
        }

        public void Shutdown()
        {
            scheduler.Shutdown(true);
        }

        public void AddMyJob(string jobName,string groupName, string message)
        {
            // define the job and ask it to run
            var map = new JobDataMap();
            map.Put("msg", message);

            //2.创建出来一个具体的作业
            IJobDetail job = JobBuilder.Create<MyJob>()
                .UsingJobData(map).WithIdentity(jobName,groupName).Build();

            //3.创建并配置一个触发器
            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).WithRepeatCount(int.MaxValue)).Build();

            //4.加入作业调度池中
            scheduler.ScheduleJob(job, trigger);
        }

        public void AddMailJob(string jobName, string groupName,string cronExpression, string recipients, string subject, string body)
        {
            // define the job and ask it to run
            var map = new JobDataMap();
            map.Put("recipients", recipients);
            map.Put("subject", subject);
            map.Put("body", body);

            //2.创建出来一个具体的作业
            IJobDetail job = JobBuilder.Create<MailJob>()
                .UsingJobData(map).WithIdentity(jobName,groupName).Build();

            //3.创建并配置一个触发器

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithCronSchedule(cronExpression)
                .Build();

            //4.加入作业调度池中
            scheduler.ScheduleJob(job, trigger);
        }

        public void AddSqlStatementJob(string jobName, string groupName, string cronExpression, string CommandText)
        {
            // define the job and ask it to run
            var map = new JobDataMap();
            map.Put("CommandText", CommandText);

            //2.创建出来一个具体的作业
            IJobDetail job = JobBuilder.Create<SqlStatementJob>()
                .UsingJobData(map).WithIdentity(jobName, groupName).Build();

            //3.创建并配置一个触发器
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithCronSchedule(cronExpression)
                .Build();

            //4.加入作业调度池中
            scheduler.ScheduleJob(job, trigger);
        }


        

    }
}
