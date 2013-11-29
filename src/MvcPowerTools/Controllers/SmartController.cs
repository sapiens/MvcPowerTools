using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CavemanTools.Web;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.Controllers
{
  
    /// <summary>
    /// Base controller that handles automatically invalid model state.
    /// </summary>
    [Obsolete("Use the SmartControllerAttribute",true)]
    public abstract class SmartController:Controller
    {
        private const string ViewNameKey = "_smart-view";
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attr = filterContext.ActionDescriptor.GetSingleAttribute<SmartActionAttribute>();
            if (attr==null)
            {
                HandleActionExecuting(filterContext);
            }
           
        }

        internal void HandleActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.IsPost())
            {
                currentModel = null;
                EstablishModel(filterContext.ActionParameters, filterContext.ActionDescriptor);
                if (!ModelState.IsValid)
                {
                    filterContext.Result = ActionFailResult(currentModel)();
                }
            }
        }

        void EstablishModel(IDictionary<string,object> args,ActionDescriptor ad)
        {
            if (args.Count >1)
            {
                //check if model param is specified by attribute
                var attr = ad.GetCustomAttributes(typeof (ModelIsArgumentAttribute), true).Cast<ModelIsArgumentAttribute>().FirstOrDefault();
                if (attr!=null)
                {
                    if (attr.ParameterName!=null)
                    {
                        currentModel =args.Where(p => p.Key == attr.ParameterName).Select(d => d.Value).FirstOrDefault();
                    }
                    else
                    {
                        currentModel = args.Values.Skip(attr.Position).Take(1).FirstOrDefault();
                    }
                }
            }
            
            if(currentModel==null)
            {
                //we setup the first param as the model
                currentModel = args.Values.FirstOrDefault();
            }
            
            //if model is a value type ignore
            if(currentModel!=null && !(currentModel.GetType().IsClass) && !(currentModel is string))
            {
               Debug.WriteLine("Action argument which is not an object is ignored by SmartController");
                // throw new InvalidModelException("Model must be of a reference type"); 
            }
        }

        private dynamic currentModel;
        private List<IPopulateModel> _pop = new List<IPopulateModel>();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var attr = filterContext.ActionDescriptor.GetSingleAttribute<SmartActionAttribute>();
            if (attr == null)
            {
                HandleActionExecuted(filterContext);
            }
         
        }

        internal void HandleActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Canceled) return;
            if (filterContext.HttpContext.Request.IsPost())
            {
                if (!ModelState.IsValid)
                {
                    filterContext.Result = ActionFailResult(currentModel)();
                }
            }
        }

        /// <summary>
        /// Returns a ViewResult handler
        /// Default ActionFailResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">view model</param>
        /// <returns></returns>
        protected Func<ActionResult> ViewResultError<T>(T model,string viewName=null) 
        {
            if (viewName == null) viewName = this.GetActionName();
            
            return () => View(viewName,(object) PopulateModel<T>(model));
        }

        /// <summary>
        /// Returns a JsonResult handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">view model</param>
        /// <returns></returns>
        protected Func<ActionResult> JsonResultError<T>(T model)
        {
            return () => Json(PopulateModel(model));
        }

        /// <summary>
        /// Returns a handler with the action result to use when a model update has errors.
       /// </summary>
        protected virtual Func<ActionResult> ActionFailResult<T>(T model)
        {
            return ViewResultError(model,HttpContextRegistry.Get<string>(ViewNameKey));
        }

        /// <summary>
        /// Sets the action which populates a view model
        /// </summary>
        /// <typeparam name="T">TModel</typeparam>
        /// <param name="action">action</param>
        protected void SetupModel<T>(Action<T> action) where T : class
        {
            var vm = new PopulateModel<T>(action);
            _pop.Add(vm);
        }

        /// <summary>
        /// Get view model with populated data
        /// </summary>
        /// <typeparam name="T">View model</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected T PopulateModel<T>(T model)
        {
            if (typeof(T).IsClass)
            {
                var filler = _pop.Find(d => d.ModelType.Equals(typeof (T))) as PopulateModel<T>;
                if (filler != null) filler.Map(model);
            }
            return model;
        }
    }
}