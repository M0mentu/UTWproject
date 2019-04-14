using System.Web;
using System.Web.Optimization;

namespace UTWproject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



                 bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/sweetalert.js",
                      "~/Scripts/inv/bootstrap.min.js",
                      "~/Scripts/inv/easing.js",
                      "~/Scripts/inv/jquery-3.2.1.min.js",
                      "~/Scripts/inv/owl.carousel.js",
                      "~/Scripts/inv/parallax.min.js",
                      "~/Scripts/inv/custom.js",
                      "~/Scripts/inv/popper.js",
                      "~/Scripts/jquery-1.10.2.min.js",
                      "~/Scripts/bootstrap.min.js",


                      "~/Scripts/canvasjs.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/inv/animate.css",
                      "~/Content/inv/bootstrap.min.css",
                      "~/Content/inv/font-awesome.min.css",
                      "~/Content/inv/main_styles.css",
                      "~/Content/inv/owl.carousel.css",
                      "~/Content/inv/owl.theme.default.css",
                      "~/Content/inv/responsive.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/sweetalert.css"));
        }
    }
}
