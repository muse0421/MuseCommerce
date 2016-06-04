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
using MuseCommerce.Core.Security;
using MuseCommerce.Data.Security;

namespace MuseCommerce.Web.Areas.Manage.Controllers
{
    public class MGRoleController : MuseController
    {
        private readonly ISecurityService _securityService;

        public MGRoleController()
        {
            _securityService = new SecurityService();
        }

        // GET: Manage/MGRole
        [CheckPermission(Permissions = new[] { PredefinedPermissions.ModuleQuery, PredefinedPermissions .ModuleManage})]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MGRoleInfo(string qname)
        {

            var jsAjax = Request.IsAjaxRequest();
           

            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGRole> Temp = context.Set<MGRole>();

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

        public JsonResult MGRole(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var jsAjax = Request.IsAjaxRequest();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGRole> Temp = context.Set<MGRole>();


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

        public JsonResult MGRoleAssignment(string id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {                
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = true;
                IQueryable<MGRole> Temp = context.Set<MGRole>().Include("FPermissions");


                if (!string.IsNullOrEmpty(id))
                {
                    Temp = Temp.Where(p => p.Id.StartsWith(id));
                }
                
                var oData = Temp.FirstOrDefault();
               
                var items = new
                {
                    data = oData
                };

                return Json(items,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult PutMGRole(MGRole oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGRole>().Where(p => p.Id == oData.Id).First();
                oTemp.FName = oData.FName;
                oTemp.FDescription = oData.FDescription;

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
        public JsonResult SaveMGRole(MGRole oData)
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

                    context.Set<MGRole>().Add(oData);

                    context.SaveChanges();

                  
                }
            }
            catch (Exception EX)
            {
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 3, EX.ToString());
            }

            NoticeMessageSubSystem.SendMessage("新增角色:" + oData.FName);

            var items = new
            {
                success = true
            };

            json.Data = items;

            return json;
        }

        [HttpDelete]
        public JsonResult DeleteMGRole(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var oTemp = context.Set<MGRole>().Where(p => p.Id == FID).First();
                    
                    context.MGRole.Remove(oTemp);

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

         [HttpPost]
        public JsonResult SaveMGRolePermission(MGRole oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGRole>().Include("FPermissions")
                    .Where(p => p.Id == oData.Id).First();
                oTemp.FName = oData.FName;
                oTemp.FDescription = oData.FDescription;

                oTemp.ModifiedBy = User.Identity.Name;
                oTemp.ModifiedDate = DateTime.Now;

                
                oData.FPermissions.ForEach(item => {
                    if (!oTemp.FPermissions.Exists(m => m.Id == item.Id))
                    {
                        context.Set<MGPermission>().Attach(item);
                        oTemp.FPermissions.Add(item);
                    }
                });

                oTemp.FPermissions.ForEach(item =>
                {
                    if (!oData.FPermissions.Exists(m => m.Id == item.Id))
                    {
                        oTemp.FPermissions.Remove(item);
                    }
                }); 
                

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