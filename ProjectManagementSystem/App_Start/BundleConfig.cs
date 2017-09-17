using System.Web;
using System.Web.Optimization;

namespace ProjectManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").IncludeDirectory("~/Frontend/Scripts", "*.js", false));
            
            bundles.Add(new ScriptBundle("~/bundles/angular").IncludeDirectory("~/assets/libs", "*.js", true));
            
            bundles.Add(new ScriptBundle("~/bundles/site").IncludeDirectory("~/app/main","*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/shared").IncludeDirectory("~/app/shared", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/login").IncludeDirectory("~/app/login", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/dataUpdater").IncludeDirectory("~/app/updater", "*.js", true));
            //bundles.Add(new ScriptBundle("~/bundles/moment").IncludeDirectory("~/Frontend/Scripts/moment", "*.js", true));
            //bundles.Add(new ScriptBundle("~/Scripts/users").IncludeDirectory("~/Frontend/Scripts/angapp/users"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/angular-material").IncludeDirectory("~/assets/css/angular-material","*.css"));
            bundles.Add(new StyleBundle("~/styles/angular-toastr").IncludeDirectory("~/assets/css/angular-toastr", "*.css"));
            bundles.Add(new StyleBundle("~/Content/jquery-ui").IncludeDirectory("~/assets/css/jquery-ui", "*.css", false));
            //bundles.Add(new StyleBundle("~/Content/angular-material").IncludeDirectory("~/assets/css/angular-material","*.css"));
            bundles.Add(new StyleBundle("~/Content/ui-grid").IncludeDirectory("~/assets/css/ui-grid", "*.css"));
            bundles.Add(new StyleBundle("~/Content/reset").Include("~/assets/css/reset.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").IncludeDirectory("~/assets/css","*.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/assets/css/site.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
        }
    }
}
