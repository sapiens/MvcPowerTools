using System;

#if WEBAPI

namespace WebApiPowerTools.Routing.Conventions
#else
namespace MvcPowerTools.Routing.Conventions
#endif
{
    /// <summary>
    /// Signals that the property should not be included in the route url.
    /// This property will be a query parameter.
    /// Used by <see cref="OneModelInHandlerConvention"/>
    ///</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromRouteAttribute:Attribute
    {
         
    }
}