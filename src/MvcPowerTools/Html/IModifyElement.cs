using HtmlTags;

namespace MvcPowerTools.Html
{
    public interface IModifyElement : ISelectConvention
    {
        HtmlTag Modify(HtmlTag tag, ModelInfo info);
    }
}