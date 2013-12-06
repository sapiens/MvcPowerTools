using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    internal class NullHtmlGenerator : IGenerateHtml
    {
        public static IGenerateHtml Instance = new NullHtmlGenerator();

        public HtmlTag GenerateElement(ModelInfo info)
        {
            return HtmlTag.Empty();
        }
    }
}