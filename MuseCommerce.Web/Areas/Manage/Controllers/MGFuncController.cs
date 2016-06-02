﻿using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace MuseCommerce.Web.Areas.Manage.Controllers
{
    public class MGFuncController : MuseController
    {
        // GET: Manage/MGFunc
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MGFuncInfo(string qname,string qurl)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            int total = 0;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGFunc> Temp = context.Set<MGFunc>();

                total = Temp.Count();
                if (!string.IsNullOrEmpty(qname))
                {
                    Temp = Temp.Where(p => p.FName.StartsWith(qname));
                }
                if (!string.IsNullOrEmpty(qurl))
                {
                    Temp = Temp.Where(p => p.FUrl.StartsWith(qurl));
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

        public JsonResult MGFunc(string id)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<MGFunc> Temp = context.Set<MGFunc>();


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
        public JsonResult PutMGFunc(MGFunc oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;


                var oTemp = context.Set<MGFunc>().Where(p => p.Id == oData.Id).First();
                oTemp.FName = oData.FName;
                oTemp.FUrl = oData.FUrl;

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
        public JsonResult SaveMGFunc(MGFunc oData)
        {
            JsonResult json = new JsonResult() { };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;

                oData.CreatedBy = User.Identity.Name;
                oData.CreatedDate= DateTime.Now;

                context.Set<MGFunc>().Add(oData);
               
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