using System;
using System.Reflection;

namespace MvcPowerTools.Filters
{
    public interface IMatchAction
    {
        IConfigureAction If(Predicate<MethodInfo> info);
    }
}