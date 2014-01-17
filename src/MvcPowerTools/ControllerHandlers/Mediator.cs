using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcPowerTools.ControllerHandlers
{
    public static class Mediator
    {
        public static TOut QueryTo<TOut>(this object model)  where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler= (dynamic)DependencyResolver.Current.GetService(handlerType);
            if (handler.IsNull()) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(model.GetType().Name, typeof(TOut).Name));
            return (TOut) handler.Handle((dynamic)model);
        }
           
        public static async Task<TOut> QueryAsyncTo<TOut>(this object model)  where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQueryAsync<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler= (dynamic)DependencyResolver.Current.GetService(handlerType);
            if (handler.IsNull()) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(model.GetType().Name, typeof(TOut).Name));
            return await (Task<TOut>) handler.HandleAsync((dynamic)model);
        }


        public static TResult Request<TResult>(this object input) where TResult : class
        {
            input.MustNotBeNull();
            var handlerType = typeof(IHandleCommand<,>).MakeGenericType(input.GetType(), typeof(TResult));
            var handler = (dynamic)DependencyResolver.Current.GetService(handlerType);
            if (handler.IsNull()) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(input.GetType().Name, typeof(TResult).Name));
            return (TResult)handler.Handle((dynamic)input);
        }
    }
}