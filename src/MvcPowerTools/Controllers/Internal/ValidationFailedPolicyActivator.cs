using System;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers.Internal
{
    public class ValidationFailedPolicyActivator : IValidationFailedPolicyFactory
    {
        private readonly IDependencyResolver _solver;

        public ValidationFailedPolicyActivator(IDependencyResolver solver)
        {
            //if (solver == null) solver = DependencyResolver.Current;
            _solver = solver;
        }

        public IResultForInvalidModel<T> GetInstance<T>(Type policy, T model) where T : class, new()
        {
            var closed=policy.MakeGenericType(typeof(T));
            closed.MustImplement<IResultForInvalidModel<T>>();
            var inst = _solver.GetService(closed);

            if (inst==null) throw new InvalidOperationException("Can't instantiate type '{0}'. The type isn't available from the DI Container or it doesn't exist.".ToFormat(closed));

            var cast=  inst.Cast<IResultForInvalidModel<T>>();
            cast.Model = model;
            cast.ModelSetup = _solver.GetService<ISetupModel<T>>();                           
            return cast;
        }
    }
}