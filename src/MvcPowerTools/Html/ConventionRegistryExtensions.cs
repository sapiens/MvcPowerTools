using System;
using System.Reflection;
using HtmlTags;
using MvcPowerTools.Html.Internals;

namespace MvcPowerTools.Html
{
    public static class ConventionRegistryExtensions
    {
        public static IConfigureAction Unless(this IConfigureConventions conventions, Predicate<ModelInfo> predicate)
        {
            return conventions.If(d => !predicate(d));
        }

        public static IConfigureConventions Ignore<T>(this IConfigureConventions conventions)
        {
            return conventions.Ignore(d => d.Type == typeof (T));
        }

        public static IConfigureConventions IgnoreMembersOf<T>(this IConfigureConventions conventions) where T : class
        {
            var tp = typeof (T);
            return conventions.Ignore(m =>
            {
                return tp.GetProperty(m.Name, BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.Public|BindingFlags.IgnoreCase) != null;
            });
        }

        public static IConfigureAction ForType<T>(this IConfigureConventions conventions)
        {
            return conventions.If(d => d.Type.Is<T>());
        }

        public static IConfigureAction ForModelWithAttribute<T>(this IConfigureConventions conventions)
            where T : Attribute
        {
            return conventions.If(d => d.HasAttribute<T>());
        }

        public static HtmlTag GenerateTags(this IDefinedConventions conventions, ModelInfo info)
        {
            var all = conventions.GetConventions(info);
            return ModelTypeAdviser.GetGenerator(info, all).GenerateElement(info);
        }

        public static HtmlConventionsManager SetDefaults(this HtmlConventionsManager conventions)
        {
            conventions.Displays.DefaultBuilder(DefaultBuilders.BasicTagBuilder);
            conventions.Editors.DefaultBuilder(DefaultBuilders.FormInputBuilder);
            conventions.Labels.DefaultBuilder(DefaultBuilders.LabelBuilder);
            return conventions;
        }
    }
}