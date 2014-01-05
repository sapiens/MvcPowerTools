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
            if (handler.IsNull()) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(typeof(TInput).Name,typeof(TViewModel).Name));
            var model = handler.Handle(input);
            return GetView(model);
        }

             
        protected virtual ActionResult GetView(TViewModel model)
        {
            return View(model);
        }
        
    }
}