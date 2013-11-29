using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines
{
    public class FlexibleViewEngine:IViewEngine
    {
        private FlexibleViewEngineSettings _settings=new FlexibleViewEngineSettings();

        public FlexibleViewEngine(Action<FlexibleViewEngineSettings> config=null)
        {
            if (config != null)
            {
                config(_settings);
            }
            
        }
        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <returns>
        /// The partial view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="partialViewName">The name of the partial view.</param><param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return GetView(controllerContext, partialViewName, null, useCache, true);
        }

        /// <summary>
        /// Finds the specified view by using the specified controller context.
        /// </summary>
        /// <returns>
        /// The page view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="viewName">The name of the view.</param>
        /// <param name="masterName">The name of the master.</param>
        /// <param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return GetView(controllerContext, viewName, masterName, useCache, false);
        }

        /// <summary>
        /// True if the view is part of Mvc display or editor templates
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static bool IsMvcTemplate(string viewName)
        {
            return viewName.StartsWith("DisplayTemplates") || viewName.StartsWith("EditorTemplates");
        }


        /// <summary>
        /// Finds view and returns result
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="useCache">Ignored</param>
        /// <param name="isPartial"></param>
        /// <returns></returns>
        ViewEngineResult GetView(ControllerContext controllerContext, string viewName, string masterName, bool useCache,bool isPartial)
        {
            if (_settings.Conventions.Count == 0)
            {
                throw new MissingViewConventionsException();
            }
            
            List<string> searched = new List<string>();

            foreach (var convention in _settings.Conventions)
            {
                if (!convention.Match(controllerContext, viewName))
                {
                    continue;
                }

                var viewPath = convention.GetViewPath(controllerContext, viewName);

                if (!FileExists(viewPath))
                {
                    searched.Add(viewPath+" [FlexibleViewEngine]");
                    continue;
                }

                var masterPath = "";

                if (!isPartial)
                {
                    masterPath = convention.GetMasterPath(controllerContext, masterName);
                    if (!masterPath.IsNullOrEmpty(true) && !FileExists(masterPath))
                    {
                        searched.Add(viewPath + " [FlexibleViewEngine]");
                        continue;
                    }
                }
                
                var factory = _settings.FindViewFactoryByExtension(GetFileExtension(viewPath));
                if (factory == null)
                {
                    searched.Add(viewPath + " [FlexibleViewEngine]");
                    continue;
                }
                
                var settings = new ViewCreationData(controllerContext, viewPath, masterPath);
                
                var view = isPartial?factory.CreatePartial(settings):factory.Create(settings);
                return new ViewEngineResult(view, this);
            }

            return new ViewEngineResult(searched.ToArray());
        }

        bool FileExists(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
        }

        string GetFileExtension(string relativeFilePath)
        {
            var info = new FileInfo(HostingEnvironment.MapPath(relativeFilePath));
            return info.Extension.TrimStart('.');
        }

        /// <summary>
        /// Releases the specified view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param><param name="view">The view.</param>
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            var disp = view as IDisposable;
            if (disp != null)
            {
                disp.Dispose();
            }
        }
       
    }
}