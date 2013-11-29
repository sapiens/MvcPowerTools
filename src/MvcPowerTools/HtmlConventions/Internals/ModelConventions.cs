using System.Collections.Generic;

namespace MvcPowerTools.HtmlConventions.Internals
{
    internal class ModelConventions : IHaveModelConventions
    {
        public static ModelConventions Ignored = new ModelConventions();

        private ModelConventions()
        {
            IsIgnored = true;
        }

        public ModelConventions(IUseConventions registry)
        {
            Registry = registry;
            Modifiers = new IModifyElement[0];
        }


        /// <summary>
        /// Can be null
        /// </summary>
        public IBuildElement Builder { get; set; }

        public IEnumerable<IModifyElement> Modifiers { get; internal set; }

        public IGenerateHtml CreateGenerator()
        {
            return new GeneratorWrapper(Builder, Modifiers);
        }

        public IUseConventions Registry { get; private set; }
        public bool IsIgnored { get; private set; }
    }
}