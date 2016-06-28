using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MuseCommerce.Core.Common;
using System.Data.Entity.Validation;

namespace MuseCommerce.Web.Areas.Order.Controllers
{
    public class PORequestController : MuseController
    {
        // GET: Order/PORequest
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PORequestInfo(string qbillno, DateTime? qsdate, DateTime? qedate)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<PORequest> Temp = context.Set<PORequest>();

                total = Temp.Count();
                if (!string.IsNullOrEmpty(qbillno))
                {
                    Temp = Temp.Where(p => p.FBillNo.StartsWith(qbillno));
                }
                if (qsdate.HasValue==true)
                {
                    Temp = Temp.Where(p => p.FDate.CompareTo(qsdate.Value) >= 0);
                }
                if (qedate.HasValue == true)
                {
                    Temp = Temp.Where(p => p.FDate.CompareTo(qedate.Value) <= 0);
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


        public JsonResult POTranTypeInfo(string qname)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
            var items = new
            {
                data = EnumExtensions.EnumToEnumerable<POTranType>()
            };

            json.Data = items;

            return json;

        }


        public ActionResult Display()
        {
            return View();
        }

        public JsonResult PORequest(string id)
        {  
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<PORequest> Temp = context.Set<PORequest>().Include("PORequestEntrys").Include("PORequestEntrys.FItem");


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

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPut]
        public JsonResult PutPORequest(PORequest oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string errmsg = "";
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    #region
                    context.Configuration.ProxyCreationEnabled = false;
                    var oTemp = context.Set<PORequest>().Include("PORequestEntrys")
                        .Where(p => p.Id == oData.Id).First();

                    oTemp.FDate = oData.FDate;
                    oTemp.FTranType = oData.FTranType;
                    oTemp.PoAddress.StreetName = oData.PoAddress.StreetName;
                    oTemp.PoAddress.StreetNumber = oData.PoAddress.StreetNumber;
                    oTemp.FNote = oData.FNote;


                    oTemp.ModifiedBy = User.Identity.Name;
                    oTemp.ModifiedDate = DateTime.Now;

                    if (oData.PORequestEntrys != null)
                    {
                        foreach (var item in oData.PORequestEntrys)
                        {
                            if (item.Id == "Add")
                            {
                                item.FInterID = oData.Id;
                                item.Id = Guid.NewGuid().ToString();
                                item.CreatedDate = DateTime.Now;
                                item.CreatedBy = User.Identity.Name;
                            }
                            item.FItem = null;
                        }
                    }

                    oData.PORequestEntrys.ForEach(item =>
                    {
                        if (!oTemp.PORequestEntrys.Exists(m => m.Id == item.Id))
                        {                            
                            oTemp.PORequestEntrys.Add(item);
                        }
                    });

                    for (int i = 0; i < oTemp.PORequestEntrys.Count; i++)
                    {
                        var item = oTemp.PORequestEntrys[i];
                        if (oData.PORequestEntrys.Exists(m => m.Id == item.Id))
                        {
                            var modifitem = oData.PORequestEntrys.Where(p => p.Id == item.Id).First();
                            item.FQty = modifitem.FQty;
                            item.FPrice = modifitem.FPrice;
                        }
                    };

                    for (int i = 0; i < oTemp.PORequestEntrys.Count; i++)
                    {
                        var item = oTemp.PORequestEntrys[i];
                        if (!oData.PORequestEntrys.Exists(m => m.Id == item.Id))
                        {
                            oTemp.PORequestEntrys.Remove(item);
                        }
                    };

                    context.SaveChanges();

                    #endregion

                }
            }
            catch (DbEntityValidationException ex)
            {
                errmsg = ex.ToString();
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 1, ex.ToString());
                MvcApplication.mySource.TraceInformation("Informational message.");
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 1, ex.ToString());
                MvcApplication.mySource.TraceInformation("Informational message.");
            }

            
            var items = new
            {
                success = true
            };

            json.Data = items;

            return json;
        }

        public ActionResult CreateJ()
        {
            var oData = new PORequest();
            oData.PORequestEntrys = new List<PORequestEntry>();
            return View(oData);
        }

        public ActionResult Create()
        {
            return View();
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
                    oData.Id = Guid.NewGuid().ToString();
                    oData.FStatus = "N";


                    oData.CreatedBy = User.Identity.Name;
                    oData.CreatedDate = DateTime.Now;

                    if (oData.PORequestEntrys != null)
                    {
                        foreach (var item in oData.PORequestEntrys)
                        {                            
                            item.Id = Guid.NewGuid().ToString();
                            item.CreatedDate = DateTime.Now;
                            item.CreatedBy = User.Identity.Name;
                            item.FItem = null;
                        }
                    }

                    context.Set<PORequest>().Add(oData);

                    context.SaveChanges();

                }
            }
            catch (DbEntityValidationException ex)
            {
                errmsg = ex.ToString();
                MvcApplication.mySource.TraceEvent(TraceEventType.Error, 1, ex.ToString());
                MvcApplication.mySource.TraceInformation("Informational message.");
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