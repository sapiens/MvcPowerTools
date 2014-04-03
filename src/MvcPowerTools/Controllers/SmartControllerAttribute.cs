using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SmartControllerAttribute:ActionFilterAttribute
    {
        ValidModelOnlyAttribute _action=new ValidModelOnlyAttribute();
        
        /// <summary>
        /// Gets or sets the policy to use when a model validation fails.
        /// If empty, it will default to return a view with the same name as the action.
        /// The policy can be overriden at action level using <see cref="ValidModelOnlyAttribute"/>
        /// </summary>
        public Type ValidationFailedPolicy
        {
            get { return _action.ValidationFailedPolicy; }
            set
            {
                if (value!=null) _action.ValidationFailedPolicy = value;
            }
        }

        private const string MustHandleKey = "_smart-controller-handled";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items.Remove(MustHandleKey);
            if (!filterContext.ActionDescriptor.HasCustomAttribute<ValidModelOnlyAttribute>())
            {
                _action.OnActionExecuting(filterContext);
                filterContext.HttpContext.Set(MustHandleKey,true);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {          
            if (filterContext.Canceled) return;
            var handle = filterContext.HttpContext.Get(MustHandleKey, false);
            if (handle)
            {
                _action.OnActionExecuted(filterContext);
            }
        }

        /// <summary>
        /// Must be registered as generic
        /// </summary>
        public static Type[] TypesForDIContainer = new[] {ValidModelOnlyAttribute.DefaultPolicy};

        /// <summary>
        /// Registers types with a DI Container using the provided action
        /// </summary>
        /// <param name="genericTypeRegistration">Action which will register a generic open type with the DI Container</param>
        public static void RegisterInContainerGenericTypes(Action<Type> genericTypeRegistration)
        {
            Array.ForEach(TypesForDIContainer,genericTypeRegistration);
        }
    }
}