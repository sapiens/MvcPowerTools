﻿using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace HtmlConventionsSample
{
    public class ConfigTask_1_Container
    {
        public static void Run()
        {
            var c = new ContainerBuilder();
            c.RegisterAssemblyModules(typeof (ConfigTask_1_Container).Assembly);
            StaticConfig.Container = c.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(StaticConfig.Container));
        }
    }
}