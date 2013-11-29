using HtmlTags;

namespace MvcPowerTools.HtmlConventions
{
    public interface IModifyElement : ISelectConvention
    {
        HtmlTag Modify(HtmlTag tag, ModelInfo info);
    }
}