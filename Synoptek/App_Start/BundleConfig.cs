using System.Web.Optimization;

namespace Synoptek
{
    public class BundleConfig
    {

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            //added to restrict scripts which are loading twice 
            bundles.IgnoreList.Ignore("*.unobtrusive-ajax.min.js", OptimizationMode.WhenDisabled);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/Custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive*"
                       ));
             
            bundles.Add(new StyleBundle("~/bundles/bootstrapCss").Include(
                   "~/Content/bootstrap.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                   "~/Scripts/bootbox.min.js"
                ));
             
            bundles.Add(new ScriptBundle("~/bundles/bootstrapJs").Include(
                "~/Scripts/jquery.min.js",
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/bootstrap-datepicker.min.js"
            ));
             
            bundles.Add(new StyleBundle("~/bundles/lightboxCss").Include(
                   "~/Content/lightbox.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/lightbox").Include(
                 "~/Scripts/lightbox-plus-jquery.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/sumoselectCss").Include(
                   "~/Content/sumoselect.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/sumoselect").Include(
                 "~/Scripts/jquery.sumoselect.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/dropzoneCss").Include(
                   "~/Content/dropzone.css"
                ));
              
            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                 "~/Scripts/dropzone.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/backstretch").Include(
                 "~/Scripts/jquery.backstretch.min.js",
                 "~/Scripts/steps.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/steps").Include(
                 "~/Content/steps.css"
           ));

            bundles.Add(new ScriptBundle("~/bundles/TinyMCEJs").Include(
                "~/Scripts/tinymce.min.js",
                "~/Scripts/theme.min.js",
                "~/Scripts/bootstrapValidator.js"
            )); 
            
            bundles.Add(new StyleBundle("~/bundles/TinyMCECss").Include(
                   "~/Content/skins/lightgray/content.min.css",
                   "~/Content/skins/lightgray/skin.min.css"
                ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/font-awesome.min.css", 
                                                                    "~/Content/custom.css", 
                                                                    "~/Content/bootstrap-datepicker3.min.css"
                                                                ));

            bundles.Add(new ScriptBundle("~/bundles/JqueryValidation").Include(
               "~/Scripts/JqueryValidation.js"
           ));
        }
    }
}