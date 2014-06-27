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
        
        /// <summary>
        /// Executes the handler
        /// </summary>
        /// <param name="input">Input model</param>
        /// <param name="resultConfig">Lambda to configure handler result</param>
        /// <param name="nullModelResult">What to return if the result is null</param>
        /// <returns></returns>
        protected ActionResult Handle(TInput input,Func<TViewModel,ActionResult> resultConfig=null,Func<ActionResult> nullModelResult=null)
        {
            input.MustNotBeNull("input");
            var model = input.QueryTo<TViewModel>();
            if (model == null)
            {
                if (nullModelResult != null) return nullModelResult();
                return NullModelResult(input);
            }
            
            if (resultConfig!=null)
            {
                return resultConfig(model);
            }
            return View(model);            
        }

        protected ActionResult NullModelResult(TInput input)
        {
            return this.HttpNotFound();
        }                   
    }
}