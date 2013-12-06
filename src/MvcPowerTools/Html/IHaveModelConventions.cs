using System.Collections.Generic;

namespace MvcPowerTools.Html
{
    public interface IHaveModelConventions
    {
        /// <summary>
        /// Can be null
        /// </summary>
        IBuildElement Builder { get; set; }

        IEnumerable<IModifyElement> Modifiers { get; }
        IGenerateHtml CreateGenerator();
        IUseConventions Registry { get; }
        bool IsIgnored { get; }
    }
}