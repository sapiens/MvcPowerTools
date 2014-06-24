using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcPowerTools
{
    public static class PropertiesOrderExtensions
    {
        /// <summary>
        /// Orders the members according to the <see cref="PropertiesOrderAttribute"/> specified on their declaring type.        
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> OrderAsAnnotated(this IEnumerable<MemberInfo> members)
        {
            var m = members.FirstOrDefault();
            if (m == null) return members;
            var attrib = m.DeclaringType.GetCustomAttribute<PropertiesOrderAttribute>();
            return attrib.ReturnDefaultOrResult(a=>a.Sort(members),members);
        }
    }
}