using System;
using System.Collections;

namespace MvcPowerTools.Html.Internals
{
    internal class ModelTypeAdviser
    {
        public static IGenerateHtml GetGenerator(ModelInfo info,
            IHaveModelConventions conventions)
        {
           
            if (info.HasAttribute<DisplayTemplateAttribute>())
            {
                return DisplayTemplateGenerator.Instance;
            }
            
            if (info.HasAttribute<EditorTemplateAttribute>())
            {
                return EditorTemplateGenerator.Instance;
            }

            if (conventions.IsIgnored)
            {
                return NullHtmlGenerator.Instance;
            }
            
            if (conventions.Builder != null)
            {
                return conventions.CreateGenerator();
            }
            if (info.Type.IsUserDefinedClass())
            {
                if (info.HasAttribute<AsOneElementAttribute>())
                {
                    return new PrimitiveTypeGenerator(conventions); 
                }
                return new CustomTypeGenerator(info.Meta, conventions);
            }
            if (info.Type == typeof (string) || !info.Type.DerivesFrom<IEnumerable>())
            {
                return new PrimitiveTypeGenerator(conventions);
            }
            return NullHtmlGenerator.Instance;
        }
    }
}