using System.Collections.Generic;
using MvcPowerTools.ViewEngines.Conventions;

namespace MvcPowerTools.ViewEngines
{
    /// <summary>
    /// By default it supports Razor 
    /// </summary>
    public class FlexibleViewEngineSettings
    {
        public FlexibleViewEngineSettings()
        {
            ViewFactories=new List<IViewFactory>();
            Conventions=new List<IFindViewConvention>();
            ViewFactories.Add(new RazorViewFactory());
            Conventions.Add(new RazorControllerActionConvention());
            Conventions.Add(new RazorSharedFolderConvention());
        }
        /// <summary>
        /// Razor views factory is already added
        /// </summary>
        public List<IViewFactory> ViewFactories { get; private set; }

        /// <summary>
        /// Asp.Net mvc standard convention for Razor c# are already added
        /// </summary>
        public List<IFindViewConvention> Conventions { get; private set; }

        internal IViewFactory FindViewFactoryByExtension(string extension)
        {
            return ViewFactories.Find(v => v.IsSuitableFor(extension));
        }
    }
}