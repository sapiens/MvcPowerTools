using HtmlTags;

namespace MvcPowerTools.HtmlConventions.Internals
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