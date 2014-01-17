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
            var handlerType = typeof(IHandleQueryAsync<,>).MakeGenericType(typeof(TInput), typeof(TViewModel));
            var handler = (IHandleQueryAsync<TInput, TViewModel>)DependencyResolver.Current.GetService(handlerType);
            handler.MustNotBeNull();
            var model = await handler.HandleAsync(input);
            return GetView(model);
        }

        protected virtual ActionResult GetView(TViewModel model)
        {
            return View(model);
        }
        
    }
}