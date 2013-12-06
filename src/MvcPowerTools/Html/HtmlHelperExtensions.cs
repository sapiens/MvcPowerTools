using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class HtmlHelperExtensions
    {
        public static HtmlTag Edit<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            ModelMetadata metadata;
            var info = CreateModelInfo(html, property, out metadata);
            return info.ConventionsRegistry().Editors.GenerateTags(info);
        }


        public static HtmlTag EditModel<T>(this HtmlHelper<T> html)
        {
            var dt = html.ViewData.ModelMetadata;
            var info = new ModelInfo(dt, html.ViewContext);
            return info.ConventionsRegistry().Editors.GenerateTags(info);
        }

        public static HtmlTag Display<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            ModelMetadata metadata;
            var info = CreateModelInfo(html, property, out metadata);
            return info.ConventionsRegistry().Displays.GenerateTags(info);
        }

        public static HtmlTag DisplayModel<T>(this HtmlHelper<T> html)
        {
            var dt = html.ViewData.ModelMetadata;
            var info = new ModelInfo(dt, html.ViewContext);
            return info.ConventionsRegistry().Displays.GenerateTags(info);
        }


      
        //public static IDisposable BeginForm(this HtmlHelper html, string controller=null, string action=null,Action<FormTag> config=null)
        //{
        //    var form = new FormTag();
        //    form.Method("POST");

        //    if (controller.IsNullOrEmpty())
        //    {
        //        controller = html.ViewContext.GetControllerName();
        //    }

        //    if (action.IsNullOrEmpty())
        //    {
        //        action = html.ViewContext.GetActionName();
        //    }
            
        //    if (config != null) config(form);
        //    return null;
        //}

        private static ModelInfo CreateModelInfo<T, R>(HtmlHelper<T> html, Expression<Func<T, R>> property,
            out ModelMetadata metadata)
        {
            metadata = ModelMetadata.FromLambdaExpression(property, html.ViewData);
            var info = new ModelInfo(metadata, html.ViewContext);
            info.HtmlName = ExpressionHelper.GetExpressionText(property);
            info.HtmlId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(info.HtmlName);
            return info;
        }
    }
}