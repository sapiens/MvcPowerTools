using System;

namespace MvcPowerTools.HtmlConventions
{
    public interface IConfigureActionCriteria
    {
        IConfigureAction If(Predicate<ModelInfo> info);
        IConfigureModifier Always { get; }
    }
}