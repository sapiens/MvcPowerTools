using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using HtmlTags;

namespace MvcPowerTools.Html
{
    public static class ModelInfoExtensions
    {
        public static bool PropertyOrParentHasAttribute<T>(this ModelInfo info) where T:Attribute
        {
            if (info.HasAttribute<T>()) return true;
            return (info.ParentType != null && info.ParentType.HasCustomAttribute<T>());
        }

        /// <summary>
        /// Gets attribute from property or parent container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetModelAttribute<T>(this ModelInfo info) where T : Attribute
        {
            var at = info.GetAttribute<T>();
            if (at == null && info.ParentType!=null)
            {
                at = info.ParentType.GetCustomAttribute<T>(true);
            }
            return at;
        }

        public static HtmlConventionsManager ConventionsRegistry(this ModelInfo info)
        {
            return HtmlConventionsManager.GetCurrentRequestProfile(info.ViewContext.HttpContext);
        }

        ///// <summary>
        ///// Renders the display template (found in DisplayTemplates/[typename].cshtml
        ///// and wraps it with a html tag
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="info"></param>
        ///// <param name="model"></param>
        ///// <param name="templateName"></param>
        ///// <returns></returns>
        //public static HtmlTag DisplayTemplate<T>(this ModelInfo info,  T model,string templateName=null)
        //{
        //    return HtmlTag.Placeholder().AppendHtml(HtmlHelperExtensions.RenderTemplate(info.ViewContext, model,templateName,info.Name));
        //}

        /// <summary>
        /// Renders the display template (found in DisplayTemplates/[typename].cshtml
        /// and wraps it with a html tag
        /// </summary>
        /// <param name="info"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static HtmlTag DisplayTemplate(this ModelInfo info,string templateName=null)
        {
            if (templateName.IsNullOrEmpty()) templateName = info.Type.Name;
            return HtmlTag.Placeholder().AppendHtml(HtmlHelperExtensions.RenderTemplate(info.ViewContext, info.RawValue,"DisplayTemplates/"+templateName));
        }

        public static HtmlTag EditorTemplate(this ModelInfo info, string templateName = null)
        {
            if (templateName.IsNullOrEmpty()) templateName = info.Type.Name;
            return HtmlTag.Placeholder().AppendHtml(HtmlHelperExtensions.RenderTemplate(info.ViewContext, info.RawValue, "EditorTemplates/" + templateName,info.Name));
        }
        

        /// <summary>
        /// Gets the model info for the specified property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static ModelInfo GetInfoForProperty<T>(this ModelInfo info,Expression<Func<T, object>> property)
        {
            string name = "";
            if (property.Body.NodeType == ExpressionType.Convert)
            {
                var data = property.Body.As<UnaryExpression>().Operand;
                var param = Expression.Parameter(typeof (T));
                var lambda = Expression.Lambda(data, new[] {param});
                name = ExpressionHelper.GetExpressionText(lambda);
            }
            else
            {
                name = ExpressionHelper.GetExpressionText(property);
            }
            
            return GetInfoForProperty(info, name);
        }

        /// <summary>
        /// Gets the model info for the specified property
        /// </summary>
        /// <param name="info"></param>
        /// <param name="propertyName">Property can be specified as a path i.e property1.User.Name</param>
        /// <returns></returns>
        public static ModelInfo GetInfoForProperty(this ModelInfo info,string propertyName)
        {
            propertyName.MustNotBeEmpty();
            var segments = propertyName.Split('.');
            var meta = GetMeta(info.Meta, segments, 0);
            if (meta == null)
            {
                throw new ArgumentException("Specified property wasn't found. Check the spelling, case or path");
            }
            var model= new ModelInfo(meta,info.ViewContext);
            model.HtmlName = propertyName;
            model.HtmlId = info.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName);
            return model;
        }

        static ModelMetadata GetMeta(ModelMetadata info,string[] path,int idx)
        {
            ModelMetadata meta = info.Properties.FirstOrDefault(d=>d.PropertyName==path[idx]);
            
            if (meta != null)
            {
                idx++;
                if (idx == path.Length) return meta;
                return GetMeta(meta, path, idx);
               
            }
            return null;
        }
    }
}