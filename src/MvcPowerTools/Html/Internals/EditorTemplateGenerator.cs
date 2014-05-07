using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    class EditorTemplateGenerator : IGenerateHtml
    {
        public static IGenerateHtml Instance=new EditorTemplateGenerator();
        
        public HtmlTag GenerateElement(ModelInfo info)
        {
            return info.EditorTemplate();
        }
    }
}