using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public class ActionCall
    {
        private Type _controller;
        private MethodInfo _method;
        private IDictionary<string, ParameterInfo> _args=new Dictionary<string, ParameterInfo>();

        public ActionCall(MethodInfo method,RoutingPolicySettings settings)
        {
            method.MustNotBeNull();
            settings.MustNotBeNull();
            Settings = settings;
            _controller = method.DeclaringType;
            _method = method;
            foreach (var arg in method.GetParameters().Where(p=>!p.ParameterType.IsUserDefinedClass()))
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

        public RoutingPolicySettings Settings { get; private set; }
        /// <summary>
        /// Creates a route value dictionary with controller and action values set
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary CreateDefaults()
        {
            var defaults = new RouteValueDictionary();
            var controler = Controller.Name;
            if (controler.EndsWith("Controller"))
            {
                controler = controler.Substring(0, controler.Length - 10);
            }
            defaults["controller"] = controler;
            defaults["action"] = Method.Name;
            return defaults;
        }




        /// <summary>
        /// Sets the defaults for the route params. Only action parameters with default values are considered.
        /// If the value is equal to the type's default value, it's considered optional
        /// User defined params are ignored.
        /// This method should not be used for POST.
        /// </summary>
        /// <param name="defaults"></param>
        public void SetParamsDefaults(RouteValueDictionary defaults)
        {
            foreach (var p in Arguments.Values)
            {
                if (p.RawDefaultValue == DBNull.Value) continue;

                if (p.RawDefaultValue == p.ParameterType.GetDefault())
                {
                    defaults[p.Name] = UrlParameter.Optional;
                }
                else
                {
                    defaults[p.Name] = p.RawDefaultValue;
                }
            }
            //var param = Method.GetParameters().Where(p => p.RawDefaultValue != DBNull.Value && !TypeExtensions.IsUserDefinedClass(p.ParameterType));
            //foreach (var p in param)
            //{
            //    defaults[p.Name] = p.RawDefaultValue;
            //}
        }
    }
}