using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcPowerTools.Html
{
    public static class ModelInfoExtensions
    {
        public static HtmlConventionsManager ConventionsRegistry(this ModelInfo info)
        {
            return HtmlConventionsManager.GetCurrentRequestProfile(info.ViewContext.HttpContext);
        }

        
        public static ModelInfo GetInfoForProperty<T>(this ModelInfo info,Expression<Func<T, object>> property)
        {
            var name=ExpressionHelper.GetExpressionText(property);
            return GetInfoForProperty(info, name);
        }

        /// <summary>
        /// 
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
                //for (var i = 1; i < path.Length; i++)
                //{
                //    meta = meta.Properties.FirstOrDefault(d => d.PropertyName == path[i]);

                //}
            }
            return null;
        }
    }
}