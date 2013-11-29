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
	public class ThemedWebformViewEngine : ThemedBuildManagerViewEngine
    {
        public ThemedWebformViewEngine()
        {
            MasterLocationFormats = new[]
                                        {
                                            "~/Themes/{2}/Views/{1}/{0}.master",
                                            "~/Themes/{2}/Views/Shared/{0}.master",

                                            "~/Views/{1}/{0}.master",
                                            "~/Views/Shared/{0}.master"
                                        };

            AreaMasterLocationFormats = new[]
                                            {
                                                "~/Areas/{2}/Themes/{3}/{1}/{0}.master",
                                                "~/Areas/{2}/Themes/{3}/Shared/{0}.master",

                                                "~/Areas/{2}/Views/{1}/{0}.master",
                                                "~/Areas/{2}/Views/Shared/{0}.master"
                                            };

            ViewLocationFormats = new[]
                                      {
                                          "~/Themes/{2}/Views/{1}/{0}.aspx",
                                          "~/Themes/{2}/Views/{1}/{0}.ascx",
                                          "~/Themes/{2}/Views/Shared/{0}.aspx",
                                          "~/Themes/{2}/Views/Shared/{0}.ascx",

                                          "~/Views/{1}/{0}.aspx",
                                          "~/Views/{1}/{0}.ascx",
                                          "~/Views/Shared/{0}.aspx",
                                          "~/Views/Shared/{0}.ascx"
                                      };

            AreaViewLocationFormats = new[]
                                          {
                                              "~/Areas/{2}/Themes/{3}/{1}/{0}.aspx",
                                              "~/Areas/{2}/Themes/{3}/{1}/{0}.ascx",
                                              "~/Areas/{2}/Themes/{3}/Shared/{0}.aspx",
                                              "~/Areas/{2}/Themes/{3}/Shared/{0}.ascx",

                                              "~/Areas/{2}/Views/{1}/{0}.aspx",
                                              "~/Areas/{2}/Views/{1}/{0}.ascx",
                                              "~/Areas/{2}/Views/Shared/{0}.aspx",
                                              "~/Areas/{2}/Views/Shared/{0}.ascx"
                                          };

            PartialViewLocationFormats = ViewLocationFormats;
            AreaPartialViewLocationFormats = AreaViewLocationFormats;
			FileExtensions = new[] {
                "aspx",
                "ascx",
                "master",
            };

        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new WebFormView(controllerContext, partialPath, null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new WebFormView(controllerContext, viewPath, masterPath);
        }
    }
}