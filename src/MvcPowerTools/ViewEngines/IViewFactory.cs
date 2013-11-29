using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines
{
    /// <summary>
    /// Used to create view instances for a specific view engine
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// True if the view engine handles files with the specified extensions
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        bool IsSuitableFor(string fileExtension);
        IView Create(ViewCreationData settings);
        IView CreatePartial(ViewCreationData settings);
    }
}