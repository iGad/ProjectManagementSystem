﻿using System.Web;
using System.Web.Optimization;

namespace ProjectManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").IncludeDirectory("~/Frontend/Scripts", "*.js", false));
            
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular/angular.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-animate/angular-animate.js",
                        "~/Scripts/angular-aria/angular-aria.js",
                        "~/Scripts/angular-messages/angular-messages.js",
                        "~/Scripts/angular-ui-router/angular-ui-router.js",
                        "~/Scripts/i18n/*.js", 
                        "~/Scripts/angular-material/angular-material.js",
                        "~/Scripts/angular-material/svg-assets-cache.js",
                        "~/Frontend/Scripts/ngFileUpload/ng-file-upload.js",
                        "~/Frontend/Scripts/ngFileUpload/ng-file-upload-shim.js"));
            

            bundles.Add(new ScriptBundle("~/bundles/signalR").IncludeDirectory("~/Frontend/Scripts/signalR", "*.js"));
            bundles.Add(new ScriptBundle("~/bundles/ui-sortable").IncludeDirectory("~/Frontend/Scripts/ui-sortable", "*.js"));
            bundles.Add(new ScriptBundle("~/bundles/uigrid").IncludeDirectory("~/Scripts/ui-grid", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/site").IncludeDirectory("~/Frontend/Scripts/angapp/main","*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/global").IncludeDirectory("~/Frontend/Scripts/angapp/global", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/login").IncludeDirectory("~/Frontend/Scripts/angapp/login", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/dataUpdater").IncludeDirectory("~/Frontend/Scripts/angapp/updater", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/moment").IncludeDirectory("~/Frontend/Scripts/moment", "*.js", true));
            //bundles.Add(new ScriptBundle("~/Scripts/users").IncludeDirectory("~/Frontend/Scripts/angapp/users"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            //bundles.Add(new StyleBundle("~/Content/angular-material").Include("~/Content/*.css"));
            bundles.Add(new StyleBundle("~/Content/jquery-ui").IncludeDirectory("~/Frontend/Content/css/jquery-ui", "*.css", false));
            bundles.Add(new StyleBundle("~/Content/angular-material").IncludeDirectory("~/Frontend/Content/css/angular-material","*.css"));
            bundles.Add(new StyleBundle("~/Content/ui-grid").IncludeDirectory("~/Frontend/Content/css/ui-grid", "*.css"));
            bundles.Add(new StyleBundle("~/Content/reset").Include("~/Content/reset.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Frontend/Content/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Frontend/Content/css/site.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
        }
    }
}
