using MuseCommerce.Core.Common;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Schedule
{
    public class MailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            IJobDetail jobDetail = context.JobDetail;

            string recipients = "";
            string body = "";
            string importance = "High";
            string subject = "";
            string body_format = "HTML";

            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("Group={0},Name={1}", jobDetail.Key.Group, jobDetail.Key.Name);

            if (jobDetail.JobDataMap != null)
            {
                foreach (var item in jobDetail.JobDataMap)
                {
                    if (item.Key == "recipients")
                    {
                        recipients = item.Value.ToString();
                    }
                    if (item.Key == "body")
                    {
                        body = item.Value.ToString();
                    }
                    if (item.Key == "importance")
                    {
                        importance = item.Value.ToString();
                    }
                    if (item.Key == "subject")
                    {
                        subject = item.Value.ToString();
                    }
                    if (item.Key == "body_format")
                    {
                        body_format = item.Value.ToString();
                    }
                }
            }

            MailService.Send(recipients, subject, body);
        }
    }
}
