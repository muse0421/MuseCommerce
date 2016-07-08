using MuseCommerce.Core.Redis;
using MuseCommerce.Core.Schedule;
using ProtoBuf;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
        public JsonResult AddMyJob(string name, string group, string message)
        {
            var schedulerFactory = new MuseSchedulerFactory();
            string guid = Guid.NewGuid().ToString();
            schedulerFactory.AddMyJob(name, group, message);
                    
            var items = new
            {
                success = true
            };
            return Json(items, JsonRequestBehavior.AllowGet);
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
                        
            var items = new
            {
                success = true
            };
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult AddSqlStatementJob(string name, string group, string cronExpression, string CommandText)
        {
            //string cronExpression = "3/2 * * * * ? ";
            //string CommandText = "update ItemCores set [ModifiedDate]=getdate() where [Id]='001'";

            var schedulerFactory = new MuseSchedulerFactory();
            string guid = Guid.NewGuid().ToString();
            schedulerFactory.AddSqlStatementJob(name, group, cronExpression, CommandText);
                        
            var items = new
            {
                success = true
            };
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RedisIndex()
        {
            return View();
        }

        [HttpPut]
        public JsonResult InQuene(string message)
        {
            
            //redis.EnqueueItemOnList("MessageQuene", message);


            for (int i = 0; i < 100; i++)
            {
                MemoryStream xmlw = new MemoryStream();
                TestQuene TempTest = new TestQuene() { Id = i, data = new List<string>(new string[] { "1", "2", "3" }) };
                Serializer.Serialize<TestQuene>(xmlw, TempTest);
                message = Convert.ToBase64String(xmlw.ToArray());
                Console.WriteLine("EnqueueItemOnList=" + message);
                redis.EnqueueItemOnList("MessageQuene", message);
            }
            //将数据序列化后存入本地文件  
           
               

            var items = new
            {
                success = true,
                message = message
            };
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        
        static IRedisClient redis = RedisManager.GetClient();
                
        
    }


}