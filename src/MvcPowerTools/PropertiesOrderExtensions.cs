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
            return members.OrderAsAnnotated(m.DeclaringType, d => d.Name);
        }

        /// <summary>
        /// Orders the members according to the <see cref="PropertiesOrderAttribute"/> specified on their declaring type.        
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderAsAnnotated<T>(this IEnumerable<T> members, Type modelType,
            Func<T, string> nameProjection)
        {
            var attrib = modelType.GetCustomAttribute<PropertiesOrderAttribute>();
            return attrib.ReturnDefaultOrResult(a => a.Sort(members,nameProjection), members); 
        }
    }
}