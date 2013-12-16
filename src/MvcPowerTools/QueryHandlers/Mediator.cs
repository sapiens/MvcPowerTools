using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcPowerTools.QueryHandlers
{
    public static class Mediator
    {
        public static TOut QueryTo<TOut>(this object model)  where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler= (dynamic)DependencyResolver.Current.GetService(handlerType);
            AssertionsExtensions.MustNotBeNull(handler);
            return (TOut) handler.Handle((dynamic)model);
        }
           
        public static async Task<TOut> QueryAsyncTo<TOut>(this object model)  where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQueryAsync<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler= (dynamic)DependencyResolver.Current.GetService(handlerType);
            AssertionsExtensions.MustNotBeNull(handler);
            return await (Task<TOut>) handler.HandleAsync((dynamic)model);
        }
           
    }
}