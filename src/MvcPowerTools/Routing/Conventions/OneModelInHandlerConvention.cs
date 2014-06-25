using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#if WEBAPI
using System.Web.Http;
using System.Web.Http.Routing;

#else
using System.Web.Mvc;
using System.Web.Routing;
#endif


#if WEBAPI

namespace WebApiPowerTools.Routing.Conventions
#else
namespace MvcPowerTools.Routing.Conventions
#endif
{

#if WEBAPI
    public class OneModelInHandlerConvention : IBuildRoutes

#else
		 public class OneModelInHandlerConvention : IBuildRoutes
#endif   
    {
        private readonly Predicate<ActionCall> _match = a => true;

        public OneModelInHandlerConvention(Predicate<ActionCall> match=null)
        {
            if (match!=null) _match = match;
        }       
      
        /// <summary>
        /// Customize route for a given action and controller
        /// </summary>
        /// <param name="callInfo"></param>
        /// <param name="sb"></param>
        protected virtual void FormatRouteTemplate(ActionCall callInfo, StringBuilder sb)
        {
            sb.Append(callInfo.Controller.ControllerNameWithoutSuffix());
        }

        /// <summary>
        /// Allows overrider to write route params from model members
        /// </summary>
        /// <param name="member"></param>
        /// <param name="sb"></param>
        protected virtual void FormatRouteTemplate(MemberInfo member, StringBuilder sb)
        {
            var prefix = member.GetSingleAttribute<RouteSegmentPrefixAttribute>();
            if (prefix != null)
            {
                sb.AppendFormat("/{0}", prefix.Prefix);
            }
            sb.Append("{" + member.Name.ToLower() + "}");            
        }

        protected virtual bool CanBeRouteParameter(MemberInfo info)
        {
            return (info.MemberType == MemberTypes.Field || info.MemberType == MemberTypes.Property)
                   && !info.HasCustomAttribute<ExcludeFromRouteAttribute>()
                   && IsValidForRoute(info.GetMemberType());

        }

        /// <summary>
        /// Sets constraint for the specified model member (route parameters)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="builder"></param>
        /// <param name="data"></param>
        protected virtual void SetConstraint(MemberInfo info, RouteBuilderInfo builder, IDictionary<string, object> data)
        {
            var constraint = builder.GetConstraint(info.DeclaringType);
            constraint.IfNotNullDo(d => data[info.Name.ToLower()]=d);            
        }

        /// <summary>
        /// Sets route param's default value with rules:
        /// a null string sets no defaults, an empty string means optional; value type with default value is ignored;
        /// nullable means optional; classes except string are ignored. Rules are implemented in <see cref="Extensions.SetDefaultValue"/> method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="modelInstance"></param>
        /// <param name="data"></param>
        protected virtual void SetDefaultValue(MemberInfo info, object modelInstance, IDictionary<string, object> data)
        {
            info.SetDefaultValue(modelInstance,data);
        }

        /// <summary>
        /// Sets route constraints based on action/controller type
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        protected void SetConstraints(RouteBuilderInfo info, IDictionary<string, object> data)
        {
          
        }

        public virtual bool Match(ActionCall action)
        {
            return _match(action);          
        }


        /// <summary>
        /// Only primitives, nullables and string are valid
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsValidForRoute(Type type)
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

     
        string BuildUrlTemplate(ModelInfo info)
        {
            var urlBuilder = new StringBuilder();
            FormatRouteTemplate(info.Action, urlBuilder);
           
          if (!info.HasModel || IsCommand(info.Action)) return urlBuilder.ToString();
            
            info.Members
                .ForEach(m =>
                {
                    urlBuilder.Append("/");
                    FormatRouteTemplate(m, urlBuilder);
                });
            return urlBuilder.ToString();
     
        }

        /// <summary>
        /// When a controller action is a command, the input model is ignored
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual bool IsCommand(ActionCall action)
        {
            return !action.IsGet();
        }

#if WEBAPI
        public virtual IEnumerable<IHttpRoute> Build(RouteBuilderInfo info)
#else
		 public virtual IEnumerable<Route> Build(RouteBuilderInfo info)
#endif
        {
           var modelInfo=new ModelInfo(info.ActionCall,CanBeRouteParameter);
            var route = info.CreateRoute(BuildUrlTemplate(modelInfo));

            if (!IsCommand(info.ActionCall))
            {
                modelInfo.Members.ForEach(m =>
                {
                    SetDefaultValue(m, modelInfo.Instance, route.Defaults);
                    SetConstraint(m, info, route.Constraints);
                });
            }

            if (!IsCommand(info.ActionCall)) route.ConstrainToGet();
            
            if (info.ActionCall.IsPost()) route.ConstrainToPost();

            SetConstraints(info,route.Constraints);
                
            return new[] { route };
        }

        #region Inner class

        private class ModelInfo
        {
            private readonly ActionCall _action;
            private readonly Func<MemberInfo, bool> _filterMembers;

            public ModelInfo(ActionCall action, Func<MemberInfo, bool> filterMembers)
            {
                _action = action;
                _filterMembers = filterMembers;
                Members = Enumerable.Empty<MemberInfo>();
                var modelInfo = action.Method.GetParameters().FirstOrDefault();
                HasModel = modelInfo != null;
                if (!HasModel) return;
                Members = modelInfo.ParameterType.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(_filterMembers);

                Members = Members.ToArray().OrderAsAnnotated();             
             
                Instance = modelInfo.ParameterType.CreateInstance();
            }

            public bool HasModel { get; private set; }
            public object Instance { get; private set; }
            public IEnumerable<MemberInfo> Members { get; private set; }

            public ActionCall Action
            {
                get { return _action; }
            }
        }

        #endregion
    }
    
}