using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using CavemanTools.Model;

namespace MvcPowerTools.HtmlConventions
{
    public class ModelInfo
    {
        public ModelMetadata Meta { get; private set; }
        public ViewContext ViewContext { get; private set; }

        public ModelInfo(ModelMetadata meta, ViewContext viewContext)
        {
            Meta = meta;
            ViewContext = viewContext;
            if (meta.ContainerType != null)
            {
                PropertyDefinition = meta.ContainerType.GetProperty(meta.PropertyName);

                Name = meta.PropertyName;
                ParentType = meta.ContainerType;
            }
            else
            {
                IsRootModel = true;
                Name = meta.ModelType.Name;
            }
            RawValue = meta.Model;
            Type = meta.ModelType;
        }

        public ModelState ModelState
        {
            get { return ViewContext.ViewData.ModelState[Name]; }
        }

      

        public ModelErrorCollection ModelErrors
        {
            get
            {
                if (ModelState == null) return new ModelErrorCollection();
                return ModelState.Errors;
            }
        }

        public bool ValidationFailed
        {
            get { return ModelErrors.Count > 0; }
        }

        public PropertyInfo PropertyDefinition { get; private set; }
        public bool IsRootModel { get; private set; }
        public Type Type { get; private set; }

        public bool HasAttribute<T>() where T : Attribute
        {
            if (PropertyDefinition != null)
            {
                return PropertyDefinition.HasCustomAttribute<T>();
            }
            else
            {
                return Type.HasCustomAttribute<T>();
            }
        }

        public T GetAttribute<T>() where T : Attribute
        {
            if (PropertyDefinition != null)
            {
                return PropertyDefinition.GetSingleAttribute<T>();
            }
            else
            {
                return Type.GetSingleAttribute<T>();
            }
        }

        public string Name { get; private set; }

        public string HtmlId { get; set; }

        public string HtmlName { get; set; }

        public object RawValue { get; private set; }

        public string ValueAsString
        {
            get
            {
                if (RawValue == null) return "";
                return RawValue.ToString();
            }
        }

        public T Value<T>()
        {
            return (T) RawValue;
        }

        public Type ParentType { get; private set; }
    }
}