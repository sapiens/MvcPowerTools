using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public class RoutingConventionsSettings
    {
        public RoutingConventionsSettings()
        {
            CreateHandler=()=>new MvcRouteHandler();                
        }
        /// <summary>
        /// Namespace from where the controllers start. Default is [assembly_name]
        /// </summary>
        public string NamespaceRoot { get; set; }
        /// <summary>
        /// Gets or sets an implementation of IRouteHandler, default is MvcRouteHandler
        /// </summary>
        public Func<IRouteHandler> CreateHandler { get; set; }
    }
}