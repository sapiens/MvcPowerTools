using System;

namespace MvcPowerTools.Routing
{
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