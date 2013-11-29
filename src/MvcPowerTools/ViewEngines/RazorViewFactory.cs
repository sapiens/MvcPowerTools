using System.Linq;
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines
{
    public class RazorViewFactory:IViewFactory
    {
        public const string Id = "Razor";
        private string[] _extensions = new []{"cshtml","vbhtml"};
        public bool IsSuitableFor(string fileExtension)
        {
            return _extensions.Contains(fileExtension);
        }

        public IView Create(ViewCreationData settings)
        {
            return new RazorView(settings.Context, settings.ViewPath,settings.MasterPath, true,_extensions);
        }

        public IView CreatePartial(ViewCreationData settings)
        {
            return new RazorView(settings.Context, settings.ViewPath, null, false, _extensions);
        }
    }
}