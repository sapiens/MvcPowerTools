using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MvcPowerTools.Controllers.Internal;

namespace MvcPowerTools.Controllers
{
    /// <summary>
    /// Invokes action only if the model is valid. Works only for POSTs
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidModelOnlyAttribute:ActionFilterAttribute
    {
        private const string WorkerKey = "_smart-worker";
        /// <summary>
        /// True to disable smart controller for this action only
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets or sets the policy to use when a model validation fails.
        /// If empty, it will default to return a view with the same name as the action
        /// </summary>
        public Type ValidationFailedPolicy { get; set; }

        internal static Type DefaultPolicy = typeof (ViewResultForInvalidModel<>);

        public ValidModelOnlyAttribute()
        {
            Order = 100;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Ignore || filterContext.Result!=null) return;
            var worker = CreateWorker(filterContext.Controller.As<Controller>(),() =>
                                                EstablishModel(filterContext.ActionParameters,
                                                               filterContext.ActionDescriptor
                                                                            .GetSingleAttribute
                                                                   <ModelIsArgumentAttribute>()),filterContext.ActionDescriptor,DependencyResolver.Current);
            worker.CheckBeforeAction();
            if (worker.HasModel)
            {
                filterContext.Result = worker.Context.As<SmartContextFacade>().Result;
                
                filterContext.HttpContext.Set(WorkerKey,worker);
            }

        }

        //internal ActionResult DoWork(SmartActionWorker worker, bool before)
        //{
        //    if (before)
        //    {
        //        worker.CheckBeforeAction()
        //    }

        //    else
        //    {
        //        worker.CheckAfterAction();
        //    }
        //    if (worker.HasModel)
        //    {
        //        return worker.Context.As<SmartContextFacade>().Result;                
        //    }

        //    return null;
        //}

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Ignore || filterContext.Canceled || filterContext.Result!=null) return;
           
            var worker = filterContext.HttpContext.Get<SmartActionWorker>(WorkerKey);
            if (worker != null)
            {
                worker.CheckAfterAction();

                var rez = worker.Context.As<SmartContextFacade>().Result;
                if (rez!=null) filterContext.Result = rez;
            }
        }

        //internal for testing 
        internal SmartActionWorker CreateWorker(Controller ctrl,Func<dynamic> modelId, ICustomAttributeProvider action,IDependencyResolver solver)
        {
            if (ctrl == null) throw new NotSupportedException("This attribute works only with Controller");

            var ipFactory = solver.GetService<IValidationFailedPolicyFactory>();
            if (ipFactory == null)
            {
                ipFactory=new ValidationFailedPolicyActivator(solver);
            }
            
            var facade = new SmartContextFacade(ctrl,modelId,ValidationFailedPolicy??DefaultPolicy, ipFactory);
            
            
            CheckOverridePolicy(action,facade);
            var worker = new SmartActionWorker(facade);
            return worker;
        }

        //internal for testing 
        internal static void CheckOverridePolicy(ICustomAttributeProvider action, SmartContextFacade facade)
        {
            var changePolicy = action.GetCustomAttributes(true).FirstOrDefault(at=> at is IOverrideValidationFailedPolicy).Cast<IOverrideValidationFailedPolicy>();
            if (changePolicy != null)
            {
                facade.PolicyOverride = changePolicy;
            }
        }

        //internal for testing
        internal static dynamic EstablishModel(IDictionary<string, object> args, ModelIsArgumentAttribute attr = null)
        {
            dynamic currentModel = null;
            if (args.Count > 1)
            {
                if (attr != null)
                {
                    if (attr.ParameterName != null)
                    {
                        currentModel = args.Where(p => p.Key == attr.ParameterName).Select(d => d.Value).FirstOrDefault();
                    }
                    else
                    {
                        currentModel = args.Values.Skip(attr.Position).Take(1).FirstOrDefault();
                    }
                }
            }

            if (currentModel == null)
            {
                //we setup the first param as the model
                currentModel = args.Values.FirstOrDefault();
            }

            //if model is a value type ignore
            if (currentModel != null)
            {

                var tp = (Type)currentModel.GetType();
                
                if (!tp.IsUserDefinedClass())
                {
                    currentModel = null;
#if DEBUG
                    Debug.WriteLine("Action argument which is not an object is ignored by SmartController");
#endif
                }

            }

            return currentModel;
        }

       
    }


    
}