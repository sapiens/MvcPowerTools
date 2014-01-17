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

        protected ActionResult Handle(TModel input,Func<TResult,ActionResult> handlerSuccess)
        {
            if (!ModelState.IsValid) return GetView(input);
            var result = input.Request<TResult>();
            return handlerSuccess(result);
        }


        protected virtual ActionResult GetView(TModel model)
        {
            return View(model);
        }

        protected NoResult NoResult()
        {
            return new NoResult();
        }
    }
}