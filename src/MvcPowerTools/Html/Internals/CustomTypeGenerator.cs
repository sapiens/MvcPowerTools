using System;
using System.Web.Mvc;
using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    internal class CustomTypeGenerator : IGenerateHtml
    {
        protected ModelMetadata Meta;
        protected IHaveModelConventions Conventions;

        public CustomTypeGenerator(ModelMetadata meta, IHaveModelConventions conventions)
        {
            Meta = meta;
            Conventions = conventions;
        }

        public HtmlTag GenerateElement(ModelInfo info)
        {
            var tag = HtmlTag.Placeholder();
            foreach (var property in Meta.Properties.OrderAsAnnotated(info.Type,m=>m.PropertyName))
            {
                var child = GetTag(property, info);
                tag.Children.Add(child);
            }
            Conventions.Builder = new NopBuilder(tag);
            return Conventions.CreateGenerator().GenerateElement(info);
        }

        protected virtual HtmlTag GetTag(ModelMetadata property, ModelInfo parentInfo)
        {
            var info = new ModelInfo(property, parentInfo.ViewContext);
            if (parentInfo.HtmlName.IsNullOrEmpty())
            {
                info.HtmlId = info.HtmlName = property.PropertyName;
            }

            else
            {
                info.HtmlId = parentInfo.HtmlId + "_" + info.Name;
                info.HtmlName = parentInfo.HtmlName + "." + info.Name;
            }
            var propConventions = Conventions.Registry.GetConventions(info);
            var propGenerator = ModelTypeAdviser.GetGenerator(info, propConventions);
            return propGenerator.GenerateElement(info);
        }
    }

    //class CustomTypeEditorGenerator : BaseCustomTypeGenerator
    //{
    //    public CustomTypeEditorGenerator(ModelMetadata meta, IHaveModelConventions conventions) : base(meta, conventions)
    //    {
    //    }

    //    protected override HtmlTag GetTag(ModelMetadata property,ModelInfo parentInfo)
    //    {
    //        var info=new ModelInfo(property,parentInfo.ViewContext);
    //        if (parentInfo.HtmlName.IsNullOrEmpty())
    //        {
    //            info.HtmlId=info.HtmlName = property.PropertyName;                 
    //        }

    //        else
    //        {
    //            info.HtmlId = parentInfo.HtmlId + "_" + info.Name;
    //            info.HtmlName = parentInfo.HtmlName + "." + info.Name;
    //        }
    //        var propConventions = Conventions.Registry.GetConventions(info);
    //        var propGenerator = ModelTypeAdviser.GetEditorGenerator(info, property,propConventions);
    //        return propGenerator.GenerateElement(info);
    //    }
    //}
}