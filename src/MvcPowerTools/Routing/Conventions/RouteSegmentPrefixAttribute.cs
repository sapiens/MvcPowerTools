using System;

namespace MvcPowerTools.Routing.Conventions
{
    
    /// <summary>
    /// When you want to prefix a route segment. Instead of {action}/{page} you want {action}/page/{page}. 
    /// Used by <see cref="OneModelInHandlerConvention"/>    
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property)]
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