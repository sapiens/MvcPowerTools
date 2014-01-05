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

    /// <summary>
    /// When you want to prefix a route segment
    /// Example: instead of {action}/{page} you want {action}/page/{page}
    /// </summary>
    public class RouteSegmentPrefixAttribute : Attribute
    {
        public string Prefix { get; set; }

        public RouteSegmentPrefixAttribute(string prefix)
        {
            prefix.MustNotBeEmpty();
            Prefix = prefix;
        }
    }
}