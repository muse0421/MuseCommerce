using MuseCommerce.Core.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Areas.Manage.Controllers
{
    public class MGJobController : Controller
    {
        // GET: Manage/MGJob
        public ActionResult Index()
        {
            return View();
        }
        [HttpPut]
        public JsonResult AddMyJob(string name,string group,string message)
        {            
            var schedulerFactory = new MuseSchedulerFactory();
            string guid = Guid.NewGuid().ToString();
            schedulerFactory.AddMyJob(name, group, message);

            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var items = new
            {
                success = true
            };
            return json;
        }

        [HttpPut]
        public JsonResult AddMailJob(string name, string group, string cronExpression
            , string recipients
            , string subject
            , string body)
        {
            //string cronExpression = "3/10 * * * * ? ";
            //string recipients = "xpy.liu@kingmaker-footwear.com";
            //string subject = "測試郵件";
            //string body = "測試郵件";


            var schedulerFactory = new MuseSchedulerFactory();
            string guid = Guid.NewGuid().ToString();
            schedulerFactory.AddMailJob(name, group, cronExpression, recipients, subject, body);

            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var items = new
            {
                success = true
            };
            return json;
        }
        [HttpPut]
        public JsonResult AddSqlStatementJob(string name,string group,string cronExpression,string CommandText)
        {            
            //string cronExpression = "3/2 * * * * ? ";
            //string CommandText = "update ItemCores set [ModifiedDate]=getdate() where [Id]='001'";

            var schedulerFactory = new MuseSchedulerFactory();
            string guid = Guid.NewGuid().ToString();
            schedulerFactory.AddSqlStatementJob(name, group, cronExpression, CommandText);

            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var items = new
            {
                success = true
            };
            return json;
        }
    }
}