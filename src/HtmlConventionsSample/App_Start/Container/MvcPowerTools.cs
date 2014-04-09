using System;
using Autofac;
using MvcPowerTools.ControllerHandlers;

namespace HtmlConventionsSample.Container
{
    public class MvcPowerTools:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register command and query handlers
            builder.RegisterAssemblyTypes(typeof (MvcPowerTools).Assembly)
                .Where(d => d.ImplementsGenericInterface(typeof (IHandleAction<,>)))
                .AsImplementedInterfaces();
        }
    }
}