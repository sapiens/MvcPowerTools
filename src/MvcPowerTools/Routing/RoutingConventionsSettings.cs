using System;
using System.Reflection;
#if !WEBAPI
using System.Web.Mvc;
using System.Web.Routing;
#endif

#if WEBAPI

namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing
#endif
{
    public class RoutingConventionsSettings
    {
        public RoutingConventionsSettings()
        {
          #if !WEBAPI
            CreateHandler=()=>new MvcRouteHandler();
#endif
            NamespaceRoot = Assembly.GetCallingAssembly().GetName().Name;   
        }
        /// <summary>
        /// Namespace from where the controllers start. Default is [assembly_name]
        /// </summary>
        public string NamespaceRoot { get; set; }
#if !WEBAPI
        /// <summary>
        /// Gets or sets an implementation of IRouteHandler, default is MvcRouteHandler
        /// </summary>
        public Func<IRouteHandler> CreateHandler { get; set; }
#endif

    }
}