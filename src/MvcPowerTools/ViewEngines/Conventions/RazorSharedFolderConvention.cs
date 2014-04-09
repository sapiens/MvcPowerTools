namespace MvcPowerTools.ViewEngines.Conventions
{
    public class RazorSharedFolderConvention:BaseRazorMvcConvention
    {
        /// <summary>
        /// Serach in the Shared folder
        /// </summary>
        protected override bool IsShared
        {
            get { return true; }
        }
    }
}