using Autofac;

namespace HtmlConventionsSample.Container
{
    public class Repositories:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(d => d.Name.EndsWith("Db"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();


        }
    }
}