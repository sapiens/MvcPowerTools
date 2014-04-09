using System.Web;
using System.Web.Mvc;
using MvcPowerTools.Filters;

namespace HtmlConventionsSample
{
    public class ConfigTask_Filters
    {
        public static void Run()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            FiltersConventions.Config.RegisterControllers(typeof(ConfigTask_Filters).Assembly);
            FiltersConventions.Config.RegisterConventions(typeof (ConfigTask_Filters).Assembly);
            FiltersConventions.Config.BuildAndEnable();
            
        }
    }
    
}
