using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public class RoutingPolicySettings
    {
        public RoutingPolicySettings()
        {
            CreateHandler=()=>new MvcRouteHandler();            
        }
        /// <summary>
        /// Namespace from where the controllers start. Default is [assembly_name].Controllers
        /// </summary>
        public string NamespaceRoot { get; set; }
        /// <summary>
        /// Returns an implementation of IRouteHandler, default is MvcRouteHandler
        /// </summary>
        public Func<IRouteHandler> CreateHandler { get; set; }
    }
}