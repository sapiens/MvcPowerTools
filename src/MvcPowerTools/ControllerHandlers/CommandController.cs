using System;
using System.Web.Mvc;

namespace MvcPowerTools.ControllerHandlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">Model from POST</typeparam>
    /// <typeparam name="TResult">Use <see cref="NoResult"/> when you don't need to return anything</typeparam>
    public abstract class CommandController<TModel, TResult> : Controller
        where TResult : class where TModel : class
    {
        public abstract ActionResult Post(TModel model);

        /// <summary>
        /// Executes the handler
        /// </summary>
        /// <param name="input">Input model</param>
        /// <param name="resultConfig">What to do with the command's result</param>
        /// <param name="nullModelResult">What to return if the model is invalid</param>
        /// <returns></returns>
        protected ActionResult Handle(TModel input,Func<TResult,ActionResult> handlerSuccess,Func<TModel,ActionResult> invalidModel=null)
        {
            if (!ModelState.IsValid)
            {
                if (invalidModel == null) invalidModel = GetView;
                return invalidModel(input);
            }
            var result = input.SendAndReturn<TResult>();
            return handlerSuccess(result);
        }


        protected virtual ActionResult GetView(TModel model)
        {
            return View(model);

        }
    }
}