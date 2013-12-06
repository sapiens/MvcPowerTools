using HtmlTags;

namespace MvcPowerTools.Html
{
    public interface IGenerateHtml
    {
        HtmlTag GenerateElement(ModelInfo info);
    }
}