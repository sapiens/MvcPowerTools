using System;
using System.Reflection;

namespace MvcPowerTools.Filters
{
    class LambdaHostConvention : IFilterConvention
    {
        private readonly Predicate<MethodInfo> _match;

        public LambdaHostConvention(Predicate<MethodInfo> match)
        {
            _match = match;            
        }

        public int? Order { get; private set; }

        /// <summary>
        /// Filter instance
        /// </summary>
        public object Instance { get; internal set; }
        public bool Match(MethodInfo action)
        {
            return _match(action);
        }
    }
}