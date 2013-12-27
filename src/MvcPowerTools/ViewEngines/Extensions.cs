using System;
using System.Reflection;

namespace MvcPowerTools.ViewEngines
{
    public static class Extensions
    {
        public static FlexibleViewEngineSettings RegisterConventions(this FlexibleViewEngineSettings cfg,params Assembly[] assemblies)
        {
            foreach (var asm in assemblies)
            {
                foreach (var tp in asm.GetTypesDerivedFrom<IFindViewConvention>(true))
                {
                    var conv = (IFindViewConvention)Activator.CreateInstance(tp);
                    cfg.Conventions.Add(conv);
                }
            }
            return cfg;
        }
    }
}