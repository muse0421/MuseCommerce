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
    public class HMEmployeeController : MuseController
    {
        // GET: HR/HMEmployee
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult HMEmployeeInfo(string qname, int start, int length)
        {
            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<HMEmployee> Temp = context.Set<HMEmployee>()
                .Include("FDepartment")
                .Include("FORGDuty");

                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FName.StartsWith(qname));
                }

                total = Temp.Count();

                Temp = Temp.OrderBy(p => p.FNumber).Skip(start).Take(length);

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

        public JsonResult HMEmployee(string id)
        {           
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<HMEmployee> Temp = context.Set<HMEmployee>()
                    .Include("FDepartment")
                    .Include("FORGDuty");


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
        public JsonResult PutHMEmployee(HMEmployee oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var oTemp = context.Set<HMEmployee>().Where(p => p.Id == oData.Id).First();
                oTemp.FNumber = oData.FNumber;
                oTemp.FName = oData.FName;

                oTemp.FDepartmentID = oData.FDepartmentID;
                oTemp.FDutyID = oData.FDutyID;
                oTemp.FHireDate = oData.FHireDate;

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
        public JsonResult SaveHMEmployee(HMEmployee oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                oData.Id = Guid.NewGuid().ToString();

                oData.FHireDate = DateTime.Now;
                oData.FLeaveDate = DateTime.Now;
                oData.FDeleted = false;

                oData.CreatedBy = User.Identity.Name;
                oData.CreatedDate = DateTime.Now;

                context.Set<HMEmployee>().Add(oData);

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
        public JsonResult DeleteHMEmployee(string FID)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var oTemp = context.Set<HMEmployee>().Where(p => p.Id == FID).First();

                    context.HMEmployee.Remove(oTemp);

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

                var oTemp = context.Set<HMEmployee>().Where(p => p.Id == FID).First();
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

                var oTemp = context.Set<HMEmployee>().Where(p => p.Id == FID).First();
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

        public ActionResult UploaderPhoto(HttpPostedFileBase file, string Employeeid)
        {
            var form2 = Request.Form;
            var url2 = Request.Url;

            string fileName = Employeeid + ".jpg";
            //转换只取得文件名，去掉路径。 
            if (fileName.LastIndexOf("\\") > -1)
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            }
            //保存到相对路径下。 
            file.SaveAs(Server.MapPath("~/img/" + fileName));
            //以下代码是将 路径保存到数据库。 
            //string ImagePath = "../../image/img/" + fileName;
            //string sql = "insert into bookinfo(bookphoto)values('" + ImagePath + "')";
            //封装好的代码，直接调用。 
           
            return View(); 

        }

        
    }
}