using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Areas.Order.Controllers
{
    public class PORequestController : Controller
    {
        // GET: Order/PORequest
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PORequestInfo(string FBillNo)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<PORequest> Temp = context.Set<PORequest>();

                total = Temp.Count();
                if (!string.IsNullOrEmpty(FBillNo))
                {
                    Temp = Temp.Where(p => p.FBillNo.StartsWith(FBillNo));
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

        public JsonResult PORequest(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<PORequest> Temp = context.Set<PORequest>();


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
        public JsonResult PutPORequest(PORequest oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;


                var oTemp = context.Set<PORequest>().Where(p => p.Id == oData.Id).First();
              

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
        public JsonResult SavePORequest(PORequest oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string errmsg = "";
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;

                    oData.CreatedBy = User.Identity.Name;
                    oData.CreatedDate = DateTime.Now;

                    foreach (var item in oData.PORequestEntrys)
                    {
                        item.CreatedDate = DateTime.Now;
                    }

                    context.Set<PORequest>().Add(oData);

                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 1, ex.ToString());
                MvcApplication.mySource.TraceInformation("Informational message.");
            }

            var items = new
            {
                success = true,
                data = errmsg
            };

            json.Data = items;

            return json;
        }
    }
}