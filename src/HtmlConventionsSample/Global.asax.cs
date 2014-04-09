using FluentValidation.Mvc;


namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           FluentValidationModelValidatorProvider.Configure();
        }

    }
}
