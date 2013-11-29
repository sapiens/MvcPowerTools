using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CavemanTools.Model;
using FluentValidation.Mvc;
using HtmlConventionsSample.Models;
using HtmlTags;
using HtmlTags.Extended.Attributes;
using MvcPowerTools.HtmlConventions;

namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Conventions();

            FluentValidationModelValidatorProvider.Configure();
        }

        void Conventions()
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
            
            
            editors
                .ForType<bool>()
                .Modify((tag, info) =>
                {
                   tag.Children.RemoveAll(t => t.IsInputElement());
                   tag.Children.Insert(0, new MvcCheckboxElement(info.HtmlId,info.HtmlName,info.Value<bool>()));
                   return tag;
                });
            
            editors
                .If(m => !m.RawValue.Is<bool>() && !m.Type.IsUserDefinedClass() && !m.Type.IsArray)
                .Modify((tag, info) =>
                {
                    tag.GetChild<LabelTag>().AddClass("block");
                    return tag;
                });

            editors.ForType<string[]>().Build(m =>
            {
                var all = HtmlTag.Placeholder();
                foreach (var item in m.Value<string[]>())
                {
                    var rd = new RadionButtonTag(m.HtmlName, item).Id(m.HtmlId + "_" + item);
                    all.Children.Add(rd);
                    all.Children.Add(new LabelTag(rd.Id(),item));
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

            editors.Always.Modify((tag, info) =>
            {
                var input = tag.FirstInnerInputTag();
                MvcHelpers.AddValidationAttributes(input, info);
                return tag;
            });

        }
    }
}
