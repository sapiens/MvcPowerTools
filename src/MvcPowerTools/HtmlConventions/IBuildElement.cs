using HtmlTags;

namespace MvcPowerTools.HtmlConventions
{
    public interface IBuildElement : ISelectConvention
    {
        HtmlTag Build(ModelInfo info);
    }
}