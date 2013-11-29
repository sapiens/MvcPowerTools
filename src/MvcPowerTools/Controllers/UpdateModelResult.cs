//http://lostechies.com/jimmybogard/2011/06/22/cleaning-up-posts-in-asp-net-mvc/

using System;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    //todo review update model result
    /// <summary>
    /// Action result handling success and failure cases for a model update
    /// </summary>
    /// <typeparam name="T">model type</typeparam>
    public class UpdateModelResult<T> : ActionResult
    {
        public Func<ActionResult> Failure { get; private set; }
        public Func<ActionResult> Success { get; private set; }
        public T Model { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="action">Action which processes model</param>
        /// <param name="success">Function to return result if successful</param>
        /// <param name="failure">Function to return result if it fails</param>
        public UpdateModelResult(T model,Action<T> action, Func<ActionResult> success, Func<ActionResult> failure)
        {
            Model = model;
            Success = success;
            Failure = failure;
            Modify = action;
        }

        protected Action<T> Modify { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                Failure().ExecuteResult(context);

                return;
            }

            Modify(Model);

            //in case the action yielded invalid state
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                Failure().ExecuteResult(context);

                return;
            }

            Success().ExecuteResult(context);
        }
    }
}