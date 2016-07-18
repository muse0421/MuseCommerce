using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Areas.MGCode.Controllers
{
    public class ItemController : MuseController
    {
        // GET: MGCode/Item
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult ItemCoreInfo(string qnumber, string qname, int start, int length)
        {
            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<ItemCore> Temp = context.Set<ItemCore>();

                if (!string.IsNullOrEmpty(qnumber))
                {
                    Temp = Temp.Where(p => p.FNumber.StartsWith(qnumber));
                }
                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FName.StartsWith(qname));
                }

                total = Temp.Count();

                Temp = Temp.OrderBy(p => p.FNumber).Skip(start).Take(length);

                var oData = Temp.ToList();
                
                var items = new
                {
                    draw=1,
                    start = start,
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = oData
                };

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ItemCore(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<ItemCore> Temp = context.Set<ItemCore>();


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
        public JsonResult PutItemCore(ItemCore oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<ItemCore>().Where(p => p.Id == oData.Id).First();
                oTemp.FName = oData.FName;
                oTemp.FHelpCode = oData.FHelpCode;
                oTemp.FModel = oData.FModel;
                oTemp.FNumber = oData.FNumber;
                oTemp.FShortNumber = oData.FShortNumber;                               

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
        public JsonResult SaveItemCore(ItemCore oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                oData.Id = Guid.NewGuid().ToString();

                oData.CreatedBy = User.Identity.Name;
                oData.CreatedDate = DateTime.Now;

                context.Set<ItemCore>().Add(oData);

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
        public JsonResult Forbidden(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<ItemCore>().Where(p => p.Id == FID).First();
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

                var oTemp = context.Set<ItemCore>().Where(p => p.Id == FID).First();
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