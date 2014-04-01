using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing.Conventions
{
    public class OneModelInHandlerConvention : IBuildRoutes
    {
        private readonly Predicate<ActionCall> _match = a => true;

        public OneModelInHandlerConvention(Predicate<ActionCall> match=null)
        {
            if (match!=null) _match = match;
        }

        public virtual bool Match(ActionCall action)
        {
            return _match(action);          
        }

        protected virtual string FormatControllerName(string name)
        {
            return name;
        }

        protected virtual bool IsGetAction(MethodInfo method)
        {
            return method.Name.StartsWith("get", StringComparison.OrdinalIgnoreCase);
        }

        string GenerateUrlAndDefaults(ActionCall info, RouteValueDictionary defaults)
        {
            var sb = new StringBuilder();
            sb.Append(FormatControllerName(info.GetControllerName()));
            
            if (info.IsGet())
            {
                GenerateForGet(info, defaults, sb);   
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Allows you to control the parameter name and prefix.
        /// The route param will be {member_name} to lower
        /// By default it checks for [RouteSegmentPrefix]
        /// </summary>
        /// <param name="member"></param>
        /// <param name="sb"></param>
        protected virtual void FormatRouteSegment(MemberInfo member,StringBuilder sb)
        {
            var prefix = member.GetSingleAttribute<RouteSegmentPrefixAttribute>();
            if (prefix != null)
            {
                sb.AppendFormat("/{0}", prefix.Prefix);
            }
            sb.Append("/{" + member.Name.ToLower() + "}");
        }

        private void GenerateForGet(ActionCall info, RouteValueDictionary defaults, StringBuilder sb)
        {
            var args = info.Method.GetParameters();
            if (args.Length > 0)
            {
                var pinfo = args.First();

                var inst = pinfo.ParameterType.CreateInstance();

                foreach (
                    var member in
                        pinfo.ParameterType.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                            .Where(d =>
                                d.MemberType == MemberTypes.Field || d.MemberType == MemberTypes.Property)
                            .Where(d => IsValidForRoute(d.GetMemberType()))
                            .Where(d=> !d.HasCustomAttribute<ExcludeFromRouteAttribute>())
                    )
                {
                    var name = member.Name.ToLower();
                   FormatRouteSegment(member,sb);
                    var value = GetMemberValue(member, inst);
                    var type = member.GetMemberType();

                    if (value == null)
                    {
                       if (!type.Is<string>() && (type.IsNullable() || type.IsClass))
                        {
                            defaults[name] = UrlParameter.Optional;
                        }
                    }
                    else
                    {
                        if (value.Equals(type.GetDefault()))
                        {
                            if (!type.IsValueType)
                            {
                                defaults[name] = UrlParameter.Optional;
                            }
                        }
                        else
                        {
                            if (type.Is<string>() && (string) value == string.Empty)
                            {
                                defaults[name] = UrlParameter.Optional;
                            }
                            else
                            {
                                defaults[name] = value;
                            }                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Base method MUST be invoked
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool IsValidForRoute(Type type)
        {
           if (type.IsPrimitive) return true;
            var allowed = new[]
            {
                typeof (DateTime)
                ,typeof (DateTime?)
                ,typeof (TimeSpan?)
                ,typeof (TimeSpan)
                ,typeof (string)
                ,typeof (Guid)
                ,typeof (Guid?)
                ,typeof (decimal)
                ,typeof (decimal?)
                ,typeof (int?)
                ,typeof (bool?)
                ,typeof (Single?)
                
            };

            return allowed.Contains(type);
        }

        object GetMemberValue(MemberInfo mi, object inst)
        {
            if (mi.MemberType == MemberTypes.Property)
            {
                return mi.As<PropertyInfo>().GetValue(inst);
            }
            return mi.As<FieldInfo>().GetValue(inst);
        }

        public virtual IEnumerable<Route> Build(ActionCall actionInfo)
        {
            var route = actionInfo.CreateRoute();
            route.Url = GenerateUrlAndDefaults(actionInfo, route.Defaults);
            if (actionInfo.IsGet()) route.ConstrainToGet();
            if (actionInfo.IsPost()) route.ConstrainToPost();            
            return new[] { route };
        }
    }
}