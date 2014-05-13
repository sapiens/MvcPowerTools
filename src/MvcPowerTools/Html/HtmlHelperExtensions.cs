using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class HtmlHelperExtensions
    {
        
        /// <summary>
        /// Uses the defined html conventions to build an editor for the view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="html"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static HtmlTag Edit<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
           return html.GenerateFor(HtmlConventionsManager.EditorKey, property);
        }


        public static HtmlTag Label<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            return html.GenerateFor(HtmlConventionsManager.LabelKey, property);
        }

        public static HtmlTag ValidationMessage<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            return html.GenerateFor(HtmlConventionsManager.ValidationKey, property);
        }

        /// <summary>
        /// Uses html conventions to generate tags using the specified registry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="html"></param>
        /// <param name="registry">Registry with conventions (Displays,Editor,Lables etc)</param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static HtmlTag GenerateFor<T, R>(this HtmlHelper<T> html,string registry, Expression<Func<T, R>> property)
        {
            registry.MustNotBeEmpty();
            ModelMetadata metadata;
            var info = CreateModelInfo(html, property, out metadata);
            return info.ConventionsRegistry()[registry].GenerateTags(info);
        }

        /// <summary>
        /// Uses the defined html conventions to build an editor for the view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlTag EditModel<T>(this HtmlHelper<T> html)
        {
            var dt = html.ViewData.ModelMetadata;
            var info = new ModelInfo(dt, html.ViewContext);
            return info.ConventionsRegistry().Editors.GenerateTags(info);
        }

        /// <summary>
        /// Uses the defined html conventions to display the view model member
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="html"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static HtmlTag Display<T, R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            return html.GenerateFor(HtmlConventionsManager.DisplayKey, property);
            //ModelMetadata metadata;
            //var info = CreateModelInfo(html, property, out metadata);
            //return info.ConventionsRegistry().Displays.GenerateTags(info);
        }

        /// <summary>
        /// Uses the defined html conventions to display the view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlTag DisplayModel<T>(this HtmlHelper<T> html)
        {
            var dt = html.ViewData.ModelMetadata;
            var info = new ModelInfo(dt, html.ViewContext);
            return info.ConventionsRegistry().Displays.GenerateTags(info);
        }

        public static HtmlTag LinkTo<T>(this UrlHelper url, string text, object model = null, string action = "get", bool urlEncoded = true) where T : Controller
        {
            var controller = typeof (T).ControllerNameWithoutSuffix();
            return LinkTo(url, controller, text, model, action,urlEncoded);
        }

        /// <summary>
        /// Creates the url for the specifeid controller and action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string CreateFor<T>(this UrlHelper url, object model = null, string action = "get") where T : Controller
        {
            var controller = typeof (T).ControllerNameWithoutSuffix();
            if (model != null)
            {
                var data= new RouteValueDictionary(model.ToDictionary());
                return url.Action(action, controller, data);
            }
            return url.Action(action, controller, model);
        }

        public static HtmlTag LinkTo(this UrlHelper url, string controller, string text, object model = null, string action = "get",bool urlEncoded=true)           
        {
            var tagUrl = url.Action(action, controller, model);
            if (!urlEncoded) tagUrl = url.RequestContext.HttpContext.Server.UrlDecode(tagUrl);
            return new LinkTag(text, tagUrl);
        }

        /// <summary>
        /// Uses the defined html conventions to build a widget for model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HtmlTag DisplayWidget<T>(this HtmlHelper<T> html, object model)
        {
            model.MustNotBeNull();
            var metadata=ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var info = new ModelInfo(metadata, html.ViewContext);
            return info.ConventionsRegistry().Displays.GenerateTags(info);
        }
        
        
        /// <summary>
        /// Uses the defined html conventions to build an editor widget for model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HtmlTag EditorWidget<T>(this HtmlHelper<T> html, object model)
        {
            model.MustNotBeNull();
            var metadata=ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var info = new ModelInfo(metadata, html.ViewContext);
            return info.ConventionsRegistry().Editors.GenerateTags(info);
        }

        ///// <summary>
        ///// Renders a partial view for the specified model. 
        ///// The view must be in a DisplayTemplate directory and must have either the type name or a specified name
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="R"></typeparam>
        ///// <param name="html"></param>
        ///// <param name="model"></param>
        ///// <param name="templateName"></param>
        ///// <returns></returns>
        //public static MvcHtmlString DisplayTemplate<T, R>(this HtmlHelper<T> html, R model,string templateName=null)
        //{
        //    return new MvcHtmlString(DisplayTemplate(html.ViewContext, model));
        //}

        public const string PartialModelCacheKey = "partial-model-";

        internal static string RenderTemplate<T>(ViewContext context, T model,string templateName,string property=null) where T:class 
        {
            templateName.MustNotBeNull();
            if (model==null) throw new ArgumentNullException("model","Model must not be null");
            context.HttpContext.Set(PartialModelCacheKey+templateName, model);
            var viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(context.Controller.ControllerContext, templateName);
            
            if (viewResult.View == null)
            {
                var sb = new StringBuilder();
                sb.AppendLine(
                    "Partial for type '{0}' not found. Make sure you have a partial with the type name in the DisplayTemplates directory. Example: ~/Views/Shared/DisplayTemplates/MyType.cshtml"
                        .ToFormat(model.GetType().Name));
                sb.AppendLine("Searched locations:");
                foreach (var path in viewResult.SearchedLocations)
                {
                    sb.AppendLine(path);
                }
                throw new InvalidOperationException(sb.ToString());
            }
            context.HttpContext.Items.Remove(PartialModelCacheKey);
            using (var sw = new StringWriter())
            {
                var vc = new ViewContext(context.Controller.ControllerContext, viewResult.View,
                    new ViewDataDictionary(context.ViewData)
                    {
                        Model = model                        
                    }, context.TempData, sw);
                if (property != null)
                {
                    vc.ViewData.TemplateInfo.HtmlFieldPrefix =
                        context.ViewData.TemplateInfo.GetFullHtmlFieldName(property);
                }
                
                viewResult.View.Render(vc, sw);
                return sw.ToString();
            }
        }

       
        public static MvcForm BeginForm(this HtmlHelper html, string controller = null, string action = null, Action<FormTag> config = null)
        {
            var form = new FormTag();
            form.Method("POST");

            if (controller.IsNullOrEmpty())
            {
                controller = html.ViewContext.GetControllerName();
            }

            if (action.IsNullOrEmpty())
            {
                action = html.ViewContext.GetActionName();
            }

            if (config != null) config(form);
            form.Action(UrlHelper.GenerateUrl(null, action, controller, null,
                html.RouteCollection, html.ViewContext.RequestContext, true));
            form.NoClosingTag();
            html.ViewContext.Writer.Write(form.ToString());
            return new MvcForm(html.ViewContext);
        }

        public static MvcForm BeginForm<T>(this HtmlHelper html,Action<FormTag> config = null, string action = "Post") where T:Controller
        {
            return BeginForm(html,typeof (T).ControllerNameWithoutSuffix(), action, config);
        }

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