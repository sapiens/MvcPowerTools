using HtmlTags;

namespace MvcPowerTools.HtmlConventions
{
    public interface IGenerateHtml
    {
        HtmlTag GenerateElement(ModelInfo info);
    }
}