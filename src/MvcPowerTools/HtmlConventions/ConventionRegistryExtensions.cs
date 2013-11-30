using System;
using HtmlTags;
using MvcPowerTools.HtmlConventions.Internals;

namespace MvcPowerTools.HtmlConventions
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
            conventions.Editors.DefaultBuilder(DefaultBuilders.TextboxBuilder);
            conventions.Labels.DefaultBuilder(DefaultBuilders.LabelBuilder);
            return conventions;
        }
    }
}