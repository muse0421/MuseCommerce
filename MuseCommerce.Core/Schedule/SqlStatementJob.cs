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
    public class SqlStatementJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string CommandText = "";
            IJobDetail jobDetail = context.JobDetail;

            if (jobDetail.JobDataMap != null)
            {
                foreach (var item in jobDetail.JobDataMap)
                {
                    if (item.Key == "CommandText")
                    {
                        CommandText = item.Value.ToString();
                    }
                }
            }

            ExecSqlStatement.Run(CommandText);
        }
    }
}
