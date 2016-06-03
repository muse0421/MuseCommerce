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
    public class MGAccountController : MuseController
    {
        // GET: Manage/MGAccount
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MGAccountInfo(string qname)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGAccount> Temp = context.Set<MGAccount>();

                total = Temp.Count();
                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FUseName.StartsWith(qname));
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

        public JsonResult MGAccount(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGAccount> Temp = context.Set<MGAccount>();


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
                IQueryable<MGAccount> Temp = context.Set<MGAccount>().Include("FRoles");
                

                if (!string.IsNullOrEmpty(id))
                {
                    Temp = Temp.Where(p => p.Id.StartsWith(id));
                }
                var oData = Temp.FirstOrDefault();
                
                var items = new
                {
                    data = oData
                };

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult PutMGAccount(MGAccount oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGAccount>().Where(p => p.Id == oData.Id).First();
                oTemp.FUseName = oData.FUseName;
                oTemp.FUserType = oData.FUserType;

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
        public JsonResult SaveMGAccount(MGAccount oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                oData.Id = Guid.NewGuid().ToString();
                oData.CreatedBy = User.Identity.Name;
                oData.CreatedDate = DateTime.Now;

                context.Set<MGAccount>().Add(oData);

                
                context.SaveChanges();


            }


            NoticeMessageSubSystem.SendMessage("新增帳號:" + oData.FUseName);

            var items = new
            {
                success = true
            };

            json.Data = items;

            return json;
        }

        [HttpDelete]
        public JsonResult DeleteMGAccount(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var oTemp = context.Set<MGAccount>().Where(p => p.Id == FID).First();

                    context.MGAccount.Remove(oTemp);

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
        public JsonResult SaveMGRoleAssignment(MGAccount oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<MGAccount>().Include("FRoles")
                    .Where(p => p.Id == oData.Id).First();
               
                oTemp.ModifiedBy = User.Identity.Name;
                oTemp.ModifiedDate = DateTime.Now;


                oData.FRoles.ForEach(item =>
                {
                    if (!oTemp.FRoles.Exists(m => m.Id == item.Id))
                    {
                        context.Set<MGRole>().Attach(item);
                        oTemp.FRoles.Add(item);
                    }
                });

                oTemp.FRoles.ForEach(item =>
                {
                    if (!oData.FRoles.Exists(m => m.Id == item.Id))
                    {
                        oTemp.FRoles.Remove(item);
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