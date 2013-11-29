using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace MvcPowerTools
{
    public class HttpModuleHost:IHttpModule
    {

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(HttpModuleHost));
        }
         Lazy<IEnumerable<IHttpModule>> _modules 
      = new Lazy<IEnumerable<IHttpModule>>(RetrieveModules);

    private static IEnumerable<IHttpModule> RetrieveModules()
    {
      return DependencyResolver.Current.GetServices<IHttpModule>();
    }

    public void Dispose()
    {
      var modules = _modules.Value;
      foreach (var module in modules)
      {
        var disposableModule = module as IDisposable;
        if (disposableModule != null)
        {
          disposableModule.Dispose();
        }
      }
    }

    public void Init(HttpApplication context)
    {
      var modules = _modules.Value;
      foreach (var module in modules)
      {
        module.Init(context);
      }
    }
  
    }
}