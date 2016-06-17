using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using MuseCommerce.Data.Model.Security;
using MuseCommerce.Web.SignalR;

namespace MuseCommerce.Web.Areas.Manage.Controllers
{
    public class MGPermissionController : MuseController
    {
        // GET: Manage/MGPermission
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MGPermissionInfo(string qname)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGPermission> Temp = context.Set<MGPermission>();

                total = Temp.Count();
                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FName.StartsWith(qname));
                }
                var oData = Temp.ToList();

                var items = new
                {
                    recordsTotal = total,
                    data = oData
                };

                json.Data = items;

                return json;
            }
        }

        public JsonResult MGPermission(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGPermission> Temp = context.Set<MGPermission>();


                if (!string.IsNullOrEmpty(id))
                {
                    Temp = Temp.Where(p => p.Id.StartsWith(id));
                }
                var oData = Temp.FirstOrDefault();

                var items = new
                {
                    data = oData
                };

                json.Data = items;

                return json;
            }
        }

        [HttpPut]
        public JsonResult PutMGPermission(MGPermission oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGPermission>().Where(p => p.Id == oData.Id).First();
                oTemp.FName = oData.FName;
                oTemp.FDescription = oData.FDescription;
                oTemp.FCode = oData.FCode;

                oTemp.ModifiedBy = User.Identity.Name;
                oTemp.ModifiedDate = DateTime.Now;

                context.SaveChanges();

                var items = new
                {
                    success = true
                };

                json.Data = items;

                return json;
            }
        }

        [HttpPost]
        public JsonResult SaveMGPermission(MGPermission oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    oData.Id = Guid.NewGuid().ToString();
                    oData.CreatedBy = User.Identity.Name;
                    oData.CreatedDate = DateTime.Now;

                    context.Set<MGPermission>().Add(oData);

                    context.SaveChanges();


                }
            }
            catch (Exception EX)
            {
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 3, EX.ToString());
            }

            NoticeMessageSubSystem.SendMessage("新增權限:" + oData.FName);

            var items = new
            {
                success = true
            };

            json.Data = items;

            return json;
        }

        [HttpDelete]
        public JsonResult DeleteMGPermission(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var oTemp = context.Set<MGPermission>().Where(p => p.Id == FID).First();

                    context.MGPermission.Remove(oTemp);

                    context.SaveChanges();

                }
            }
            catch (Exception EX)
            {
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 3, EX.ToString());
            }

            var items = new
            {
                success = true
            };

            json.Data = items;

            return json;
        }

        [HttpPut]
        public JsonResult Forbidden(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGPermission>().Where(p => p.Id == FID).First();
                oTemp.FForbidden = true;

                oTemp.ModifiedBy = User.Identity.Name;
                oTemp.ModifiedDate = DateTime.Now;

                context.SaveChanges();

                var items = new
                {
                    success = true
                };

                json.Data = items;

                return json;
            }
        }

        [HttpPut]
        public JsonResult Restore(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGPermission>().Where(p => p.Id == FID).First();
                oTemp.FForbidden = false;

                oTemp.ModifiedBy = User.Identity.Name;
                oTemp.ModifiedDate = DateTime.Now;

                context.SaveChanges();

                var items = new
                {
                    success = true
                };

                json.Data = items;

                return json;
            }
        }

    }
}