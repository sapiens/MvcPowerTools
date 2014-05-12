using MvcPowerTools.Html;
using MvcPowerTools.Html.Conventions;

namespace HtmlConventionsSample
{
    public class ConfigTask_HtmlConventions
    {
        public static void Run()
        {
            //predefined modifiers which mimic default asp.net mvc templating behaviour
            HtmlConventionsManager.LoadModule(new DataAnnotationModifiers(), new CommonEditorModifiers(), new SemanticModifiers(),new TwitterBootstrapFormElements());

            //load all conventions defined in the assembly
            HtmlConventionsManager.LoadModules(typeof(ConfigTask_HtmlConventions).Assembly);
            
            HtmlConventionsManager.LoadWidgets(typeof(ConfigTask_HtmlConventions).Assembly);

            //predefined builders for use with model annotations
            HtmlConventionsManager.LoadModule(new DataAnnotationBuilders(), new CommonHtmlBuilders());   
        }
    }
}