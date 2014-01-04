using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    class DisplayTemplateGenerator : IGenerateHtml
    {
        public static IGenerateHtml Instance=new DisplayTemplateGenerator();
        
        public HtmlTag GenerateElement(ModelInfo info)
        {
            return info.RenderTemplate();
        }
    }
}