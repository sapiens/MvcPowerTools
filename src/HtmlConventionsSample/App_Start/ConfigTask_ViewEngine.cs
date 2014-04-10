using MvcPowerTools.ViewEngines;
using MvcPowerTools.ViewEngines.Conventions;

namespace HtmlConventionsSample
{
    public class ConfigTask_ViewEngine
    {
        public static void Run()
        {
            FlexibleViewEngine.Enable(c =>
            {
                c.Conventions.Clear();
                c.Conventions.Add(new ViewsNearController());
                c.Conventions.Add(new PartialsNearController());
                c.RegisterConventions(typeof (ConfigTask_ViewEngine).Assembly);
            },true);
        }
    }
}