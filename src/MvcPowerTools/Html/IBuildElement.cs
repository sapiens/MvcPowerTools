using HtmlTags;

namespace MvcPowerTools.Html
{
    public interface IBuildElement : ISelectConvention
    {
        HtmlTag Build(ModelInfo info);
    }
}