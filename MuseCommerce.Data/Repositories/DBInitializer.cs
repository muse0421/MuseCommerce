using MuseCommerce.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Repositories
{
    public class DBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            MGFunc mgfun1 = new MGFunc();
            mgfun1.Id = "001";
            mgfun1.FName = "权限管理";
            mgfun1.FUrl = "/Manage/MGFunc/index";
            mgfun1.CreatedDate = DateTime.Now;

            MGFunc mgfun2 = new MGFunc();
            mgfun2.Id = "002";
            mgfun2.FName = "用户管理";
            mgfun2.FUrl = "/Home/NotFound";
            mgfun2.CreatedDate = DateTime.Now;
            mgfun2.FParentID = "001";

            MGFunc mgfun3 = new MGFunc();
            mgfun3.Id = "003";
            mgfun3.FName = "角色管理";
            mgfun3.FUrl = "/Home/Error";
            mgfun3.CreatedDate = DateTime.Now;
            mgfun3.FParentID = "001";

            MGFunc mgfun4 = new MGFunc();
            mgfun4.Id = "004";
            mgfun4.FName = "对象权限设定";
            mgfun4.FUrl = "/Home/Lockout";
            mgfun4.CreatedDate = DateTime.Now;
            mgfun4.FParentID = "001";

            MGFunc mgfun5 = new MGFunc();
            mgfun5.Id = "005";
            mgfun5.FName = "订购单管理";
            mgfun5.FUrl = "/Order/PORequest/index";
            mgfun5.CreatedDate = DateTime.Now;
            mgfun5.FParentID = "001";

            context.MGFunc.Add(mgfun1);
            context.MGFunc.Add(mgfun2);
            context.MGFunc.Add(mgfun3);
            context.MGFunc.Add(mgfun4);
            context.MGFunc.Add(mgfun5);


            ItemCore Item1 = new ItemCore();
            Item1.Id = "001";
            Item1.FName = "香蕉";
            Item1.FModel = "001";
            Item1.CreatedDate = DateTime.Now;

            ItemCore Item2 = new ItemCore();
            Item2.Id = "002";
            Item2.FName = "苹果";
            Item2.FModel = "001";
            Item2.CreatedDate = DateTime.Now;

            ItemCore Item3 = new ItemCore();
            Item3.Id = "003";
            Item3.FName = "雪梨";
            Item3.FModel = "001";
            Item3.CreatedDate = DateTime.Now;

            context.ItemCore.Add(Item1);
            context.ItemCore.Add(Item2);
            context.ItemCore.Add(Item3);

            base.Seed(context);
        }
    }

    public class DBInit
    {
        public static void Seed(ApplicationDbContext context)
        {
            MGFunc mgfun1 = new MGFunc();
            mgfun1.Id = "001";
            mgfun1.FName = "权限管理";
            mgfun1.FUrl = "/Manage/MGFunc/index";
            mgfun1.CreatedDate = DateTime.Now;

            MGFunc mgfun2 = new MGFunc();
            mgfun2.Id = "002";
            mgfun2.FName = "用户管理";
            mgfun2.FUrl = "/Home/NotFound";
            mgfun2.CreatedDate = DateTime.Now;
            mgfun2.FParentID = "001";

            MGFunc mgfun3 = new MGFunc();
            mgfun3.Id = "003";
            mgfun3.FName = "角色管理";
            mgfun3.FUrl = "/Home/Error";
            mgfun3.CreatedDate = DateTime.Now;
            mgfun3.FParentID = "001";

            MGFunc mgfun4 = new MGFunc();
            mgfun4.Id = "004";
            mgfun4.FName = "对象权限设定";
            mgfun4.FUrl = "/Home/Lockout";
            mgfun4.CreatedDate = DateTime.Now;
            mgfun4.FParentID = "001";

            MGFunc mgfun5 = new MGFunc();
            mgfun5.Id = "005";
            mgfun5.FName = "订购单管理";
            mgfun5.FUrl = "/Order/PORequest/index";
            mgfun5.CreatedDate = DateTime.Now;
            mgfun5.FParentID = "001";

            context.MGFunc.Add(mgfun1);
            context.MGFunc.Add(mgfun2);
            context.MGFunc.Add(mgfun3);
            context.MGFunc.Add(mgfun4);
            context.MGFunc.Add(mgfun5);


            ItemCore Item1 = new ItemCore();
            Item1.FNumber = "001";
            Item1.Id = "001";
            Item1.FName = "香蕉";
            Item1.FModel = "001";
            Item1.CreatedDate = DateTime.Now;

            ItemCore Item2 = new ItemCore();
            Item2.FNumber = "002";
            Item2.Id = "002";
            Item2.FName = "苹果";
            Item2.FModel = "001";
            Item2.CreatedDate = DateTime.Now;

            ItemCore Item3 = new ItemCore();
            Item3.FNumber = "003";
            Item3.Id = "003";
            Item3.FName = "雪梨";
            Item3.FModel = "001";
            Item3.CreatedDate = DateTime.Now;

            context.ItemCore.Add(Item1);
            context.ItemCore.Add(Item2);
            context.ItemCore.Add(Item3);

            context.SaveChanges();
        }
    }
}
