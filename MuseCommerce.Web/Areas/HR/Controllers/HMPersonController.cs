using MuseCommerce.Data.Model.HR;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Areas.HR.Controllers
{
    public class HMPersonController : MuseController
    {
        // GET: HR/HMPerson
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult HMPersonInfo(string qname, int start, int length)
        {
            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<HMPerson> Temp = context.Set<HMPerson>();

                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FName.StartsWith(qname));
                }

                total = Temp.Count();

                Temp = Temp.OrderBy(p => p.Code).Skip(start).Take(length);

                var oData = Temp.ToList();

                var items = new
                {
                    draw = 1,
                    start = start,
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = oData
                };

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult HMPerson(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<HMPerson> Temp = context.Set<HMPerson>();


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
        public JsonResult PutHMPerson(HMPerson oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<HMPerson>().Where(p => p.Id == oData.Id).First();
                oTemp.Code = oData.Code;
                oTemp.FName = oData.FName;

                oTemp.ModifiedDate = DateTime.Now;
                oTemp.ModifiedBy = User.Identity.Name;

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
        public JsonResult SaveHMPerson(HMPerson oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                oData.Id = Guid.NewGuid().ToString();

                oData.CreatedBy = User.Identity.Name;
                oData.CreatedDate = DateTime.Now;

                context.Set<HMPerson>().Add(oData);

                context.SaveChanges();

                var items = new
                {
                    success = true
                };

                json.Data = items;

                return json;
            }
        }

        [HttpDelete]
        public JsonResult DeleteHMPerson(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var oTemp = context.Set<HMPerson>().Where(p => p.Id == FID).First();

                    context.HMPerson.Remove(oTemp);

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

                var oTemp = context.Set<HMPerson>().Where(p => p.Id == FID).First();
                //oTemp.FForbidden = true;

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

                var oTemp = context.Set<HMPerson>().Where(p => p.Id == FID).First();
                //oTemp.FForbidden = false;

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