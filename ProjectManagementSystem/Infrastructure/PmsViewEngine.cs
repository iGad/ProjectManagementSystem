using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementSystem.Infrastructure
{
    public class PmsViewEngine : RazorViewEngine
    {
        public PmsViewEngine()
        {
            FileExtensions = new[]
            {
                "cshtml",
                "vbhtml"
            };

            MasterLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Frontend/Views/{1}/{0}.cshtml",
                "~/Frontend/Views/Shared/{0}.cshtml",
            };

            PartialViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Frontend/Views/{1}/{0}.cshtml",
                "~/Frontend/Views/Shared/{0}.cshtml",
            };

            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Frontend/Views/{1}/{0}.cshtml",
                "~/Frontend/Views/Shared/{0}.cshtml",
            };
        }
    }
}