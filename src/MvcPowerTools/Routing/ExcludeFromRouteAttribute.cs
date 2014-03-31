using System;

namespace MvcPowerTools.Routing
{
    /// <summary>
    /// Signals that the property should not be included in the route url.
    /// This property will be a query parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromRouteAttribute:Attribute
    {
         
    }
}