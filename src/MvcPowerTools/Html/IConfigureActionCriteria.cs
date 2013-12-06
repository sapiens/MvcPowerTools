using System;

namespace MvcPowerTools.Html
{
    public interface IConfigureActionCriteria
    {
        IConfigureAction If(Predicate<ModelInfo> info);
        IConfigureModifier Always { get; }
    }
}