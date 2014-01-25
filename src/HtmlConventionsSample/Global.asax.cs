using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CavemanTools.Model;
using FluentValidation.Mvc;
using HtmlConventionsSample.Controllers;
using HtmlConventionsSample.Models;
using HtmlTags;
using HtmlTags.Extended.Attributes;
using MvcPowerTools.Controllers;
using MvcPowerTools.Filters;
using MvcPowerTools.Html;
using MvcPowerTools.Html.Conventions;
using MvcPowerTools.ViewEngines;

namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Container();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HtmlConventionsManager.LoadModule(new DataAnnotationModifiers(), new SemanticModifiers(), new CommonEditorModifiers());
            HtmlConventions();
            Filters();
            HtmlConventionsManager.LoadModule( new CommonHtmlBuilders());
            FluentValidationModelValidatorProvider.Configure();

            ViewEngine();
        }

        private void Container()
        {
            var cb = new ContainerBuilder();
            var assembly = typeof(HomeController).Assembly;
            cb.RegisterControllers(assembly);

            cb.RegisterType<IndexQuery>().AsImplementedInterfaces();
            
            ValidModelOnlyAttribute.RegisterContainerTypes(t=> cb.RegisterGeneric(t));
            cb.RegisterType<ValidModelOnlyAttribute>().AsSelf().SingleInstance();
           
            var models = assembly.GetExportedTypes().Where(d => d.Name.EndsWith("Model")).ToArray();
            
            cb.RegisterTypes(models).AsSelf();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(cb.Build()));
        }

        private void ViewEngine()
        {
           FlexibleViewEngine.Enable(cfg =>
           {
               //cfg.Conventions.Add();
           },removeOtherEngines:true);            
        }

        void Filters()
        {
            FiltersConventions.Config.RegisterControllers(typeof(HomeController).Assembly);
            
            FiltersConventions.Config
                .If(d => d.HasCustomAttribute<HttpPostAttribute>())
                .Use<ValidModelOnlyAttribute>();

            FiltersConventions.Config.BuildAndEnable();
        }

        void HtmlConventions()
        {
            var profile = HtmlConventionsManager.DefaultProfile;
            profile.SetDefaults();
            var labels = profile.Labels;

            labels.If(d =>
            {
                return d.Type.Is<string>() && d.ParentType.Is<IdName>();
            })
                .Modify((tag, info) =>
                {
                    return tag.Text("[IdName]" + info.Name);
                });

            var display = profile.Displays;
            
            display
                .ForType<IdName>()
                .Build(m =>
                {
                    if (m.RawValue == null) return HtmlTag.Empty();
                var div = new DivTag();
                var idLabel = new HtmlTag("span", div).Text("Id").Style("font-weight", "bold");
                var id = HtmlTag.Placeholder().Text(m.Value<IdName>().Id.ToString());
                div.Children.Add(id);
                div.Append(m.ConventionsRegistry().Labels.GenerateTags(m.GetInfoForProperty<IdName>(d => d.Name)));
                return div;
            });

            display.ForType<FluentMyModel>()
                .Modify((tag, info)=>
                {
                    var innerInfo = info.GetInfoForProperty<FluentMyModel>(d => d.Model.Name);
                    var innerTag=info.ConventionsRegistry().Displays.GenerateTags(innerInfo);
                    
                        
                        innerTag.Style("color", "green");                                     
                    return tag.Append(innerTag);
                });

            var editors = profile.Editors;
            editors.Ignore<IdName>();
            
            
            //editors
            //    .ForType<bool>()
            //    .Modify((tag, info) =>
            //    {
            //       tag.Children.RemoveAll(t => t.IsInputElement());
            //       tag.Children.Insert(0, new MvcCheckboxElement(info.HtmlId,info.HtmlName,info.Value<bool>()));
            //       return tag;
            //    });
            
            editors
                .If(m => !m.RawValue.Is<bool>() && !m.Type.IsUserDefinedClass() && !m.Type.IsArray)
                .Modify((tag, info) =>
                {
                    var label=tag.GetChild<LabelTag>();
                    if (label!=null) label.AddClass("block");
                    return tag;
                });

            editors.ForType<string[]>().Build(m =>
            {
                var all = HtmlTag.Placeholder();
                int i = 0;
                foreach (var item in m.Value<string[]>())
                {
                    var ch = new MvcCheckboxElement(m.HtmlId + "_" + item, m.HtmlName + "[" + i + "]", true);
                    all.Append(ch);
                    all.Append(new LabelTag(ch.Tag.Id(), item));
                    //var rd = new RadionButtonTag(m.HtmlName, item).Id(m.HtmlId + "_" + item);
                    //all.Children.Add(rd);
                    //all.Children.Add(new LabelTag(rd.Id(),item));
                    i++;
                }
                return all;
            });
            

            editors.Always.Modify((tag, i) =>
            {
                var wrapper = new DivTag();
                wrapper.AddClass("form-field");
                tag.WrapWith(wrapper);
                return wrapper;
            });

            //display.ForType<TplTest>().Build(m =>
            //{
            //    return m.RenderTemplate();
            //});
        }
    }
}
