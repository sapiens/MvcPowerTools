using HtmlConventionsSample.Browse.Posts;
using MvcPowerTools.ControllerHandlers;
using MvcPowerTools.Routing;


namespace HtmlConventionsSample
{
    public class ConfigTask_Routing
    {
        public static void Run()
        {
    
            RoutingConventions.Configure(c =>
            {
                c.RegisterControllers(typeof (ConfigTask_Routing).Assembly);
                c.UseOneModelInHandlerConvention();
                c.HomeIs<IndexController>(d => d.Get(NoInput.Instance));
            });
        }
    }
    
}
