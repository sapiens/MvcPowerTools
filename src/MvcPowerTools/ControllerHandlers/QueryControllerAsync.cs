using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcPowerTools.ControllerHandlers
{
    public abstract class QueryControllerAsync<TInput, TViewModel> : Controller
        where TViewModel : class where TInput : class
    {

        public abstract Task<ActionResult> Get(TInput input);

        protected async Task<ActionResult> HandleAsync(TInput input, Func<TViewModel, ActionResult> resultConfig = null, Func<ActionResult> nullModelResult = null)
        {
            input.MustNotBeNull("input");
            var model = await input.QueryAsyncTo<TViewModel>();
            if (model == null)
            {
                if (nullModelResult != null) return nullModelResult();
                return NullModelResult(input);
            }

            if (resultConfig != null)
            {
                return resultConfig(model);
            }
            return View(model);  
            
        }

        protected ActionResult NullModelResult(TInput input)
        {
            return HttpNotFound();
        }       
        
    }
}