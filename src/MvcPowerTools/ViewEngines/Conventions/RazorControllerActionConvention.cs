namespace MvcPowerTools.ViewEngines.Conventions
{
    public class RazorControllerActionConvention:BaseRazorMvcConvention
    {
        protected override bool IsShared
        {
            get { return false; }
        }
    }
}