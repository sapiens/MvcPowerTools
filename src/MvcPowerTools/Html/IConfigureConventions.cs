using System;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public interface IConfigureConventions : IConfigureActionCriteria
    {
        IConfigureConventions Add(IBuildElement builder);
        IConfigureConventions Add(IModifyElement modifier);
        IConfigureConventions DefaultBuilder(Func<ModelInfo, HtmlTag> action);
        IConfigureConventions Ignore(Predicate<ModelInfo> predicate);
    }
}