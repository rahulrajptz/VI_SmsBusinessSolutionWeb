using System.Web.Optimization;

namespace Templateprj
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Font")
                 .Include("~/assets/css/css.css", new CssRewriteUrlTransform())
                 .Include("~/assets/css/all.css", new CssRewriteUrlTransform())
                 .Include("~/assets/css/ionicons.min.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/Content/Cusom")
                 .Include("~/assets/css/CustomCss.css"));
            bundles.Add(new StyleBundle("~/Content/CusomInner")
                 .Include("~/assets/css/CustomCssInner.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                            "~/assets/css/bootstrap.css",
                            "~/assets/css/bootstrap.min.css",
                             "~/Content/bootstrap-toggle.min.css",
                            "~/assets/css/now-ui-dashboard.min.css",
                            "~/assets/css/demo.css",
                            "~/assets/css/animate.css",
                            "~/assets/Toastr/toastr.min.css",
                            "~/Content/ionicons.min.css",
                             "~/Content/CustomForm.css",
                             "~/Content/bootstrap.css",
                            //"~/assets/css/flatpickr.min.css",
                            "~/assets/JqueryUi/jquery-ui.min.css",
                            "~/assets/JqueryUi/jquery-ui.structure.min.css",
                            "~/assets/JqueryUi/jquery-ui.theme.min.css",
                             "~/assets/css/jquery.dataTables.min.css",
                              "~/Content/bootstrap-select.css",
                              "~/Content/flatpickr.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/CustomForm.css"));

            bundles.Add(new StyleBundle("~/Content/DataTable")
               .Include("~/assets/css/jquery.dataTables.min.css")
               .Include("~/Content/buttons.dataTables.min.css")
            .Include("~/Content/select2.min.css"));


            bundles.Add(new ScriptBundle("~/Scripts/CommonJS")
                                       .Include("~/Scripts/jquery-3.1.1.min.js")
                                       .Include("~/Scripts/CommonMessage.js")
                                       .Include("~/assets/js/core/popper.min.js")
                                       .Include("~/assets/js/core/bootstrap.min.js")
                                       .Include("~/assets/js/plugins/perfect-scrollbar.jquery.min.js")
                                       .Include("~/assets/js/plugins/moment.min.js")
                                       .Include("~/assets/js/plugins/bootstrap-switch.js")
                                       .Include("~/assets/js/plugins/sweetalert2.min.js")
                                       .Include("~/Scripts/jquery.validate*")
                                       .Include("~/assets/js/plugins/jquery.bootstrap-wizard.js")
                                       .Include("~/assets/js/plugins/bootstrap-selectpicker.js")
                                       .Include("~/assets/js/plugins/bootstrap-datetimepicker.js")
                                       .Include("~/assets/js/plugins/bootstrap-tagsinput.js")
                                       .Include("~/assets/js/plugins/jasny-bootstrap.min.js")
                                       .Include("~/Content/Js/jasny-bootstrap.min.js")
                                       .Include("~/assets/js/plugins/fullcalendar.min.js")
                                       .Include("~/assets/js/plugins/jquery-jvectormap.js")
                                       .Include("~/assets/js/plugins/nouislider.min.js")
                                       .Include("~/assets/js/plugins/chartjs.min.js")
                                       .Include("~/assets/js/plugins/bootstrap-notify.js")
                                       .Include("~/assets/js/now-ui-dashboard.min.js")
                                       .Include("~/assets/js/demo.js")
                                       .Include("~/assets/js/jquery.sharrre.js")
                                       .Include("~/assets/Toastr/toastr.min.js")
                                       //.Include("~/assets/js/flatpickr.min.js")
                                       .Include("~/assets/JqueryUi/jquery-ui.min.js")
                                       .Include("~/assets/js/bootstrap-toggle.min.js")
                                       .Include("~/Scripts/bootstrap.bundle.min.js")
                                       .Include("~/Scripts/bootstrap-select.min.js")
                                       .Include("~/Scripts/flatpickr.js")
                                       .Include("~/assets/js/plugins/jquery.dataTables.min.js")
                                       .Include("~/Scripts/CommonValidation.js"));
            bundles.Add(new ScriptBundle("~/Scripts/XL")
                                      .Include("~/Scripts/xls.core.min.js")
                                      .Include("~/Scripts/xlsX.core.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DataTable")
                                     .Include("~/assets/js/plugins/jquery.dataTables.min.js")
                                     .Include("~/Scripts/dataTables.buttons.min.js")
                                     .Include("~/Scripts/buttons.flash.min.js")
                                     .Include("~/Scripts/jszip.min.js")
                                     .Include("~/Scripts/buttons.html5.min.js")
                                     .Include("~/Scripts/buttons.print.min.js")
                                      .Include("~/Scripts/select2.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DataTablePDFMake")
                         .Include("~/Scripts/pdfmake/pdfmake.min.js")
                         .Include("~/Scripts/pdfmake/vfs_fonts.js"));
        }
    }
}
