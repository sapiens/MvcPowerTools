using FluentValidation.Mvc;
using StartItUp;


namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           StartupTasks.RunFromAssemblyOf<ConfigTask_1_Container>();
            FluentValidationModelValidatorProvider.Configure();
        }

    }
}
