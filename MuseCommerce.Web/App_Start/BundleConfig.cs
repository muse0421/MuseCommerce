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
                    "~/Content/plugins/iCheck/custom.css",
                    "~/Content/plugins/toastr/toastr.min.css",
                    "~/Content/plugins/datapicker/datepicker3.css",
                    "~/Content/plugins/layer/laydate/need/laydate.css",
                    "~/Content/plugins/iCheck/custom.css",
                    "~/Content/plugins/sweetalert/sweetalert.css",
                     "~/Content/plugins/jasny/jasny-bootstrap.min.css",
                     "~/Content/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css",

                     "~/Scripts/plugins/ngDialog/css/ngDialog.css",
                     "~/Scripts/plugins/ngDialog/css/ngDialog-theme-default.css",
                     "~/Scripts/plugins/ngDialog/css/ngDialog-theme-flat.css",

                     "~/Scripts/plugins/v-modal/v-modal.min.css",

                    "~/Content/style.min862f.css"));

           
            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                   "~/Content/plugins/bootstrap-table/bootstrap-table.min.css"));

            bundles.Add(new StyleBundle("~/Content/webuploader").Include(
                  "~/Content/plugins/webuploader/webuploader.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery19").Include(
                     "~/Scripts/jquery-1.9.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/underscore-min.js",
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
                       "~/Scripts/plugins/dataTables/jquery.dataTables.js",
                       "~/Scripts/plugins/dataTables/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/webuploader").Include(                       
                       "~/Scripts/plugins/webuploader/webuploader.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/baseform").Include(
                                    "~/Scripts/plugins/datapicker/bootstrap-datepicker.js",
                                    //"~/Scripts/plugins/layer/laydate/laydate.js",
                                    "~/Scripts/plugins/jasny/jasny-bootstrap.min.js",
                                    "~/Scripts/plugins/iCheck/icheck.min.js",                                    
                                    "~/Scripts/plugins/validate/jquery.validate.min.js",
                                    "~/Scripts/plugins/validate/messages_zh.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ngbaseform").Include(                                   
                                   "~/Scripts/plugins/ngDialog/js/ngDialog.js",
                                    "~/Scripts/plugins/v-modal/v-modal.js"
                                   ));

            bundles.Add(new ScriptBundle("~/bundles/otherplugin").Include(
                        "~/Scripts/hplus.js-v=2.2.0.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/jquery.steps.min.js",
                       
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

            #region

            bundles.Add(new ScriptBundle("~/bundles/mgfuncapp").Include(
                       "~/Scripts/App/mgfunc.js"));
            bundles.Add(new ScriptBundle("~/bundles/mgroleapp").Include(
                      "~/Scripts/App/mgrole.js"));
            bundles.Add(new ScriptBundle("~/bundles/mgaccountapp").Include(
                     "~/Scripts/App/mgaccount.js"));
            bundles.Add(new ScriptBundle("~/bundles/mgpermissionapp").Include(
                     "~/Scripts/App/mgpermission.js"));

            bundles.Add(new ScriptBundle("~/bundles/mgitemapp").Include(
                    "~/Scripts/App/mgitem.js"));

            bundles.Add(new ScriptBundle("~/bundles/porequest").Include(
                       "~/Scripts/App/porequest.js"));

            bundles.Add(new ScriptBundle("~/bundles/mgjobapp").Include(
                       "~/Scripts/App/mgjob.js"));

            bundles.Add(new ScriptBundle("~/bundles/ORGDuty").Include(
                      "~/Scripts/App/ORGDuty.js"));

            bundles.Add(new ScriptBundle("~/bundles/ORGDepartment").Include(
                      "~/Scripts/App/ORGDepartment.js"));

            bundles.Add(new ScriptBundle("~/bundles/HMEmployee").Include(
                     "~/Scripts/App/HMEmployee.js"));

            #endregion
        }
    }
}
