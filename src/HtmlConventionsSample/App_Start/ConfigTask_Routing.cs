using HtmlConventionsSample.Browse.Posts;
using HtmlConventionsSample._Auxilia;
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
                c.HomeIs<PostsListController>(d => d.Get(new PagedInput()));
            });
        }
    }
    
}
