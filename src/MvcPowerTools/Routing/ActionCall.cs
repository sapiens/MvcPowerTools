using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcPowerTools.Routing
{
    public class ActionCall:IEquatable<ActionCall>
    {
        private Type _controller;
        private MethodInfo _method;
        private IDictionary<string, ParameterInfo> _args=new Dictionary<string, ParameterInfo>();

        public ActionCall(MethodInfo method, RoutingConventionsSettings settings)
        {
            method.MustNotBeNull();
            _controller = method.DeclaringType;
            settings.MustNotBeNull();
            Settings = settings;
            _method = method;
            foreach (var arg in method.GetParameters().Where(p => !p.ParameterType.IsUserDefinedClass()))
            {
                _args[arg.Name] = arg;
            }
        }
        
      

        public Type Controller
        {
            get { return _controller; }
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        public IDictionary<string, ParameterInfo> Arguments
        {
            get
            {
                return _args;
            }
        }

        public RoutingConventionsSettings Settings { get; private set; }
       

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