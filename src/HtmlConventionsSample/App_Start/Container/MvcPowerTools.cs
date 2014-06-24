using System;
using Autofac;
using CavemanTools.Logging;
using MvcPowerTools.ControllerHandlers;
using MvcPowerTools.Controllers;

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
            ValidModelOnly(builder);
            LogHelper.DefaultLogger.Debug("ValidModelOnly autofac configured");
        }

        void ValidModelOnly(ContainerBuilder cb)
        {
            ValidModelOnlyAttribute.RegisterInContainer(a=>cb.RegisterType(a).SingleInstance(),t=>cb.RegisterGeneric(t));
        }
    }
}