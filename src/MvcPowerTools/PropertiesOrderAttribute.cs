using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcPowerTools
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
            return Properties.Select(d => members.First(m => m.Name == d));
        }
    }
}