using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcPowerTools.Filters
{
    class FilterActionInfo
    {
        public IEnumerable<Filter> Filters { get; private set; }

        public FilterActionInfo(MethodInfo action,IEnumerable<Filter> filters)
        {
            Filters = filters;
            Key = CreateKey(action.DeclaringType, action.Name);
        }

        public string Key { get; private set; }
           
        public static string CreateKey(Type ctrl, string actionName)
        {
            return ctrl.GetHashCode() + actionName;
        }
    }
}