using Autofac;
using Autofac.Integration.Mvc;

namespace HtmlConventionsSample.Container
{
    public class AspMvc:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof (AspMvc).Assembly);
            //builder.Register()
        }
    }
}