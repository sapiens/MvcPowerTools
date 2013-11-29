using System;

namespace MvcPowerTools.Routing
{
    /// <summary>
    /// Marks an action as the default page
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HomepageAttribute:Attribute
    {
         public string Url { get; set; }

        public HomepageAttribute()
        {
            Url = "{controller}";
        }
    }
}