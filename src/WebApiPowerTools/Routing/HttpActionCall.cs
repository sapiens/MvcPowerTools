using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiPowerTools
{
    public class HttpActionCall
    {
        public HttpActionCall(MethodInfo method)
        {
            ControllerType = method.DeclaringType;
            Action = method;
           
        }

        public Type ControllerType { get; private set; }
        public MethodInfo Action { get; private set; }

    }
}