using System.Diagnostics;
using CavemanTools.Logging;
using FluentValidation.Mvc;
using StartItUp;


namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           LogHelper.OutputTo(s=>Debug.Write(s));
            StartupTasks.RunFromAssemblyOf<ConfigTask_1_Container>();
            FluentValidationModelValidatorProvider.Configure();
        }

    }
}
