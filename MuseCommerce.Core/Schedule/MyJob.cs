using Microsoft.Practices.Unity;
using MuseCommerce.Core.Log;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MuseCommerce.Core.Schedule
{
    public class MyJob : IJob
    {       
        public void Execute(IJobExecutionContext context)
        {

            ILog ExLog = new EntLibLog();

            IJobDetail jobDetail = context.JobDetail;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Group={0},Name={1}", jobDetail.Key.Group, jobDetail.Key.Name);
            if (jobDetail.JobDataMap != null)
            {
                foreach (var item in jobDetail.JobDataMap)
                {
                    sb.AppendFormat("Key={0},Value={1}", item.Key, item.Value);
                }
            }
            ExLog.Error("My job is Running " + sb.ToString());
            Debug.WriteLine("My job is Running " + sb.ToString());
        }
    }
}
