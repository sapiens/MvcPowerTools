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
   
        public ActionCall(MethodInfo method,Type controller=null)
        {
            method.MustNotBeNull();
            if (controller == null)
            {
                _controller = method.DeclaringType;
            }
            else
            {
                _controller = controller;
            }
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

        public ParameterInfo GetActionArgument(string name)
        {
            return Method.GetParameters().FirstOrDefault(d => d.Name.Equals(name,StringComparison.OrdinalIgnoreCase));            
        }

        /// <summary>
        /// Gets action arguments which are not user defined classes names
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetArgumentNames()
        {
            return Method.GetParameters().Where(d => !d.ParameterType.IsUserDefinedClass()).Select(d => d.Name);
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