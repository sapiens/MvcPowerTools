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
            //var handlerType = typeof(IHandleQuery<,>).MakeGenericType(typeof(TInput), typeof(TViewModel));
            //var handler = (IHandleQuery<TInput, TViewModel>)DependencyResolver.Current.GetService(handlerType);
            //if (handler.IsNull()) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(typeof(TInput).Name,typeof(TViewModel).Name));
            //var model = handler.Handle(input);
            var model = input.QueryTo<TViewModel>();
            return GetView(model);
        }

             
        protected virtual ActionResult GetView(TViewModel model)
        {
            return View(model);
        }
        
    }
}