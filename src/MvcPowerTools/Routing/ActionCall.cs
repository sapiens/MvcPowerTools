using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if WEBAPI
namespace WebApiPowerTools.Routing
#else
namespace MvcPowerTools.Routing    
#endif
{
    public class ActionCall:IEquatable<ActionCall>
    {
        private Type _controller;
        private MethodInfo _method;
   
        public ActionCall(MethodInfo method)
        {
            method.MustNotBeNull();
            _controller = method.DeclaringType;
        
            _method = method;
        
        }
        
        public Type Controller
        {
            get { return _controller; }
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        
        public const string EmptyRouteUrl = "___";


        public bool Equals(ActionCall other)
        {
            if (other == null) return false;
            return (other.Method == Method);
        }

        public override bool Equals(object obj)
        {
            return Equals((ActionCall) obj);
        }
    }
}