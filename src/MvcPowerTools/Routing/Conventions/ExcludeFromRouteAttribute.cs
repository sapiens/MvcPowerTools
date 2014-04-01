using System;

namespace MvcPowerTools.Routing.Conventions
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