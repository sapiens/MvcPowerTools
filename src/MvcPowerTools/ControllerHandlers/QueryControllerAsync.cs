using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcPowerTools.ControllerHandlers
{
    public abstract class QueryControllerAsync<TInput, TViewModel> : Controller
        where TViewModel : class where TInput : class
    {

        public abstract Task<ActionResult> Get(TInput input);
        
        protected async Task<ActionResult> HandleAsync(TInput input)
        {
            input.MustNotBeNull("input");
            var model = await input.QueryAsyncTo<TViewModel>();
            if (model == null) return NullModelResult(input);
            return GetView(model);
            
        }

        protected virtual ActionResult NullModelResult(TInput input)
        {
            return HttpNotFound();
        }

        protected virtual ActionResult GetView(TViewModel model)
        {
            return View(model);
        }
        
    }
}