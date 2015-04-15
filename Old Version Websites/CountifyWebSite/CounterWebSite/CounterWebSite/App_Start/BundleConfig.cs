using System.Web;
using System.Web.Optimization;

namespace CounterWebSite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ** Shared ** //
            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/stickytoolbar.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));

            // ** Emails ** //
            bundles.Add(new StyleBundle("~/Content/emails/css").Include("~/Content/emails.css"));

            // ** Pages ** //
            //home/index
            bundles.Add(new ScriptBundle("~/bundles/home/index").Include("~/Scripts/home/index.js"));
            bundles.Add(new StyleBundle("~/Content/home/index").Include("~/Content/home/index.css"));

            //account/manage
            bundles.Add(new ScriptBundle("~/bundles/account/manage").Include(
                "~/Scripts/account/update.js",
                "~/Scripts/subscriptions/update.js"));
            bundles.Add(new StyleBundle("~/Content/account/manage").Include(
                "~/Content/account/update.css",
                "~/Content/subscriptions/update.css"));

            //stats/index
            bundles.Add(new ScriptBundle("~/bundles/stats/index").Include("~/Scripts/stats/index.js"));
            bundles.Add(new StyleBundle("~/Content/stats/index").Include("~/Content/stats/index.css"));

            // ** Libraries ** //
            //jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            //jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            //jquery-validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            //Modernizr
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            //superslides
            bundles.Add(new ScriptBundle("~/bundles/superslides").Include(
                "~/Scripts/jquery.easing.{version}.js",
                "~/Scripts/jquery.animate-enhanced.min.js",
                "~/Scripts/superslides/jquery.superslides.js"));
            bundles.Add(new StyleBundle("~/Content/superslide").Include(
                "~/Scripts/superslides/superslides.css"));

            //jqPlot
            bundles.Add(new ScriptBundle("~/bundles/jqPlot").Include(
                "~/Scripts/jqPlot/jquery.jqplot.js",
                "~/Scripts/jqPlot/jqplot.dateAxisRenderer.js"));
            bundles.Add(new StyleBundle("~/Content/jqPlot").Include(
                "~/Scripts/jqPlot/jquery.jqplot.css"));

            //blockUI
            bundles.Add(new ScriptBundle("~/bundles/blockUI").Include(
            "~/Scripts/jquery.blockUI.js"));

            //nprogress
            bundles.Add(new ScriptBundle("~/bundles/nprogress").Include(
                "~/Scripts/nprogress/nprogress.js"));
            bundles.Add(new StyleBundle("~/Content/nprogress").Include(
                "~/Scripts/nprogress/nprogress.css"));
        }
    }
}