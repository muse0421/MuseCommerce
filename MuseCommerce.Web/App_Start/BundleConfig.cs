using System.Web;
using System.Web.Optimization;

namespace MuseCommerce.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                    "~/Content/bootstrap.min14ed.css",
                    "~/Content/font-awesome.min93e3.css",                    
                    "~/Content/animate.min.css",
                    "~/Content/plugins/toastr/toastr.min.css",
                    "~/Content/plugins/sweetalert/sweetalert.css",
                    "~/Content/style.min862f.css"));

            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                   "~/Content/plugins/bootstrap-table/bootstrap-table.min.css"));
          

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery19").Include(
                     "~/Scripts/jquery-1.9.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                        "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",                       
                        "~/Scripts/plugins/layer/layer.min.js",
                         "~/Scripts/hplus.min.js",
                        "~/Scripts/contabs.min.js",
                        "~/Scripts/plugins/toastr/toastr.min.js",
                        "~/Scripts/plugins/sweetalert/sweetalert.min.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/welcome.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                       "~/Scripts/plugins/bootstrap-table/bootstrap-table.min.js",
                       "~/Scripts/plugins/bootstrap-table/bootstrap-table-mobile.min.js",
                       "~/Scripts/plugins/bootstrap-table/locale/bootstrap-table-zh-CN.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                                    "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/otherplugin").Include(
                        "~/Scripts/hplus.js-v=2.2.0.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/jquery.steps.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/messages_zh.min.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/singnalR").Include(
                       "~/Scripts/jquery.signalR-2.2.0.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angularjs-latest/angular.min.js",
                      "~/Scripts/angularjs-latest/angular-animate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularui").Include(
                     "~/Scripts/angualer-ui/angular-ui-router.js",
                     "~/Scripts/angualer-ui/ui-grid/ng-grid.min.js",
                     "~/Scripts/angualer-ui/ui-grid/ng-grid-layout.js"));

            bundles.Add(new ScriptBundle("~/bundles/mgfuncapp").Include(
                       "~/Scripts/App/mgfunc.js"));
            bundles.Add(new ScriptBundle("~/bundles/mgroleapp").Include(
                      "~/Scripts/App/mgrole.js"));

            bundles.Add(new ScriptBundle("~/bundles/mgaccountapp").Include(
                     "~/Scripts/App/mgaccount.js"));
            bundles.Add(new ScriptBundle("~/bundles/mgpermissionapp").Include(
                     "~/Scripts/App/mgpermission.js"));

            bundles.Add(new ScriptBundle("~/bundles/porequestapp").Include(
                       "~/Scripts/App/porequest.js"));


        }
    }
}
