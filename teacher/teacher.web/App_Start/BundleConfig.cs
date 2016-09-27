using System.Web;
using System.Web.Optimization;

namespace teacher.web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #region common
            bundles.Add(new ScriptBundle("~/bundles/commonjs").Include(
          "~/Content/bootstrap/js/bootstrap.min.js",
          "~/Scripts/jquery.unobtrusive-ajax.min.js",
          "~/Scripts/base.min.js",
          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/commoncss").Include(
          "~/Content/bootstrap/css/bootstrap.min.css",
          "~/Content/font-awesome/css/font-awesome.min.css",
          "~/Content/common.min.css"));
            #endregion

            #region bootstrap-daterangepicker
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-daterangepicker/js").Include(
                        "~/Content/bootstrap-daterangepicker/js/moment.js",
                        "~/Content/bootstrap-daterangepicker/js/daterangepicker.js"

                        ));


            bundles.Add(new StyleBundle("~/bundles/bootstrap-daterangepicker/css").Include(
                        "~/Content/bootstrap-daterangepicker/css/daterangepicker-bs3.css"

                        ));
            #endregion

            #region bootstrap-Validator
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-Validator/js").Include(
            "~/Content/bootstrap-Validator/js/bootstrapValidator.min.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/bootstrap-Validator/css").Include(
                        "~/Content/bootstrap-Validator/css/bootstrapValidator.min.css"
                        ));
            #endregion

            #region bootstrap-table
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-table/js").Include(
                        "~/Content/bootstrap-table/js/bootstrap-table.min.js",
                        "~/Content/bootstrap-table/js/locale/bootstrap-table-zh-CN.min.js",
                        "~/Content/bootstrap-table/js/extensions/filter-control/bootstrap-table-filter-control.js",
                        "~/Content/bootstrap-table/js/extensions/export/bootstrap-table-export.min.js",
                        "~/Content/bootstrap-table/js/extensions/tableExport/tableExport.js"
                        ));


            bundles.Add(new StyleBundle("~/bundles/bootstrap-table/css").Include(
                        "~/Content/bootstrap-table/css/bootstrap-table.min.css"));
            #endregion

            #region sweetalert
            bundles.Add(new ScriptBundle("~/bundles/sweetalert/js").Include(
            "~/Content/sweetalert/js/sweetalert.min.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/sweetalert/css").Include(
                        "~/Content/sweetalert/css/sweetalert.min.css"
                        ));
            #endregion

            #region CodeSeven-toastr
            bundles.Add(new ScriptBundle("~/bundles/CodeSeven-toastr/js").Include(
            "~/Content/CodeSeven-toastr/js/toastr.min.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/CodeSeven-toastr/css").Include(
                        "~/Content/CodeSeven-toastr/css/toastr.min.css"
                        ));
            #endregion

            #region bootstrap-checkbox
            bundles.Add(new StyleBundle("~/bundles/bootstrap-checkbox").Include(
                        "~/Content/bootstrap-checkbox/css/bootstrap-checkbox.min.css"
                        ));
            #endregion

            #region bootstrap-colorpicker
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-colorpicker/js").Include(
            "~/Content/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/bootstrap-colorpicker/css").Include(
                        "~/Content/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css"
                        ));
            #endregion

            #region bootstrap-fileinput
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-fileinput/js").Include(
            "~/Content/bootstrap-fileinput/js/fileinput.min.js",
            "~/Content/bootstrap-fileinput/js/locales/zh.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/bootstrap-fileinput/css").Include(
                        "~/Content/bootstrap-fileinput/css/fileinput.min.css"
                        ));
            #endregion

            #region echarts
            bundles.Add(new ScriptBundle("~/bundles/echarts").Include(
            "~/Scripts/echarts.min.js"
            ));
            #endregion
        }
    }
}
