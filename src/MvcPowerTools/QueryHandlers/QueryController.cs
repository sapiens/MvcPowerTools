using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcPowerTools.QueryHandlers
{
    public abstract class QueryController<TInput, TViewModel> : Controller
        where TViewModel : class where TInput : class
    {

        public abstract ActionResult Get(TInput input);
        
        protected ActionResult Handle(TInput input)
        {
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(typeof(TInput), typeof(TViewModel));
            var handler = (IHandleQuery<TInput, TViewModel>)DependencyResolver.Current.GetService(handlerType);
            handler.MustNotBeNull();
            var model = handler.Handle(input);
            return GetView(model);
        }

        

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