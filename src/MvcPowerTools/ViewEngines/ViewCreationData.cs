using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines
{
    public class ViewCreationData
    {
        public ControllerContext Context { get; private set; }
        public string ViewPath { get; private set; }
        /// <summary>
        /// Should be ignored for partials
        /// </summary>
        public string MasterPath { get; private set; }
     
        internal ViewCreationData(ControllerContext context,string viewPath,string masterPath)
        {
            Context = context;
            ViewPath = viewPath;
            MasterPath = masterPath;
        }
    }
}