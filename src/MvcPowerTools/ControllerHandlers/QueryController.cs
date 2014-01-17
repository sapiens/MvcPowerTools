using System;
using System.Web.Mvc;

namespace MvcPowerTools.ControllerHandlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput">If you don't need input data, use <see cref="NoInput"/></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class QueryController<TInput, TViewModel> : Controller
        where TViewModel : class where TInput : class
    {

        public abstract ActionResult Get(TInput input);
        
        protected ActionResult Handle(TInput input)
        {
            input.MustNotBeNull("input");
            var model = input.QueryTo<TViewModel>();
            if (model == null) return NullModelResult(input);
            return GetView(model);
        }

        protected virtual ActionResult NullModelResult(TInput input)
        {
            return this.HttpNotFound();
        }
             
        protected virtual ActionResult GetView(TViewModel model)
        {
            return View(model);
        }
        
    }
}