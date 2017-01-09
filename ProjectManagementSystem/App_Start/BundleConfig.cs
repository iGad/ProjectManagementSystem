﻿using System.Web;
using System.Web.Optimization;

namespace ProjectManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular/angular.js", 
                        "~/Scripts/angular-animate/angular-animate.js",
                        "~/Scripts/angular-aria/angular-aria.js",
                        "~/Scripts/angular-messages/angular-messages.js",
                        "~/Scripts/angular-ui-router/angular-ui-router.js",
                        "~/Scripts/i18n/*.js", 
                        "~/Scripts/angular-material/angular-material.js",
                        "~/Scripts/angular-material/svg-assets-cache.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").IncludeDirectory("~/Scripts/Site","*.js", true));



            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/angular-material").Include("~/Content/*.css"));

            bundles.Add(new StyleBundle("~/Content/reset").Include("~/Content/reset.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
        }
    }
}
