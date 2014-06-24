using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if WEBAPI

namespace WebApiPowerTools
#else
namespace MvcPowerTools
#endif
{
    
    /// <summary>
    /// Specifies in which order the class' properties/fields should be considered by a conventions (routing,html) engine 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertiesOrderAttribute:Attribute
    {
        public string[] Properties { get; set; }

        public PropertiesOrderAttribute(params string[] properties)
        {
            properties.MustNotBeEmpty();
            Properties = properties;
        }

        public IEnumerable<MemberInfo> Sort(IEnumerable<MemberInfo> members)
        {
            return Sort(members, d => d.Name);            
        }

        public IEnumerable<T> Sort<T>(IEnumerable<T> data, Func<T, string> nameProjection)
        {
            return Properties.Select(d => data.First(m => nameProjection(m) == d));
        }
    }
}