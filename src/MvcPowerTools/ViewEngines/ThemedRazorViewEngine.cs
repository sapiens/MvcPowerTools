/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This software is subject to the Microsoft Public License (Ms-PL). 
 * A copy of the license can be found in the license.htm file included 
 * in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

/* Modified for CavemanTools*/

using System.Web.Mvc;
using MvcPowerTools.ViewEngines.MvcEngine;

namespace MvcPowerTools.ViewEngines
{
	public class ThemedRazorViewEngine : ThemedBuildManagerViewEngine
    {
        public ThemedRazorViewEngine()
        {
            AreaViewLocationFormats = new[] {
                                                "~/Areas/{2}/Themes/{3}/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Themes/{3}/{1}/{0}.vbhtml",
                                               
                                                "~/Areas/{2}/Themes/{3}/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Themes/{3}/Shared/{0}.vbhtml",
                                               

                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                               
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.vbhtml",
                                               
                                            };

            AreaMasterLocationFormats = new[] {
                                                  "~/Areas/{2}/Themes/{3}/{1}/{0}.cshtml",
                                                  "~/Areas/{2}/Themes/{3}/{1}/{0}.vbhtml",
                                               
                                                  "~/Areas/{2}/Themes/{3}/Shared/{0}.cshtml",
                                                  "~/Areas/{2}/Themes/{3}/Shared/{0}.vbhtml",
                                               

                                                  "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                  "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                               
                                                  "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                  "~/Areas/{2}/Views/Shared/{0}.vbhtml",
                                               
                                              };

            AreaPartialViewLocationFormats = new[] {
                                                       "~/Areas/{2}/Themes/{3}/{1}/{0}.cshtml",
                                                       "~/Areas/{2}/Themes/{3}/{1}/{0}.vbhtml",
                                                 
                                                       "~/Areas/{2}/Themes/{3}/Shared/{0}.cshtml",
                                                       "~/Areas/{2}/Themes/{3}/Shared/{0}.vbhtml",                                                 

                                                       "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                       "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                 
                                                       "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                       "~/Areas/{2}/Views/Shared/{0}.vbhtml",
                                                 
                                                   };

			//ViewLocationFormats = new[] {
			//                                "~/Views/Themes/{2}/{1}/{0}.cshtml",
                                            
			//                                "~/Views/Themes/{2}/Shared/{0}.cshtml",
                                            

			//                                "~/Views/{1}/{0}.cshtml",
                                            
			//                                "~/Views/Shared/{0}.cshtml",
                                            
			//                            };

			//MasterLocationFormats = new[] {
			//                                  "~/Views/Themes/{2}/{1}/{0}.cshtml",
                                            
			//                                  "~/Views/Themes/{2}/Shared/{0}.cshtml",
                                            

			//                                  "~/Views/{1}/{0}.cshtml",
                                            
			//                                  "~/Views/Shared/{0}.cshtml",
                                            
			//                              };

			//PartialViewLocationFormats = new[] {
			//                                       "~/Views/Themes/{2}/{1}/{0}.cshtml",
                                                   
			//                                       "~/Views/Themes/{2}/Shared/{0}.cshtml",
                                                   

			//                                       "~/Views/{1}/{0}.cshtml",
                                                   
			//                                       "~/Views/Shared/{0}.cshtml",
                                                   
			//                                   };
			//ss
			ViewLocationFormats = new[] {
                                            "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                            "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                            
                                            "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                            "~/Themes/{2}/Views/Shared/{0}.vbhtml",
                                            

                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/{1}/{0}.vbhtml",
                                            
                                            "~/Views/Shared/{0}.cshtml",
                                            "~/Views/Shared/{0}.vbhtml",
                                            
                                        };

			MasterLocationFormats = new[] {
                                              "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                              "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                            
                                              "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                              "~/Themes/{2}/Views/Shared/{0}.vbhtml",
                                            

                                              "~/Views/{1}/{0}.cshtml",
                                              "~/Views/{1}/{0}.vbhtml",
                                            
                                              "~/Views/Shared/{0}.cshtml",
                                              "~/Views/Shared/{0}.vbhtml",
                                            
                                          };

			PartialViewLocationFormats = new[] {
                                                   "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                                   "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                                   
                                                   "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                                   "~/Themes/{2}/Views/Shared/{0}.vbhtml",
                                                   

                                                   "~/Views/{1}/{0}.cshtml",
                                                   "~/Views/{1}/{0}.vbhtml",
                                                   
                                                   "~/Views/Shared/{0}.cshtml",
                                                   "~/Views/Shared/{0}.vbhtml",
                                                   
                                               };


			ViewStartFileExtensions = new[] { "cshtml",  "vbhtml", };
        }

        public string[] ViewStartFileExtensions { get; set; }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(controllerContext, partialPath, null, false, ViewStartFileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, viewPath, masterPath, true, ViewStartFileExtensions);
        }       
    }
}