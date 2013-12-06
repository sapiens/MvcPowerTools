using System;
using System.Collections.Generic;
using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    internal class GeneratorWrapper : IGenerateHtml
    {
        private readonly IBuildElement _builder;
        private readonly IEnumerable<IModifyElement> _modifiers;

        public GeneratorWrapper(IBuildElement builder, IEnumerable<IModifyElement> modifiers)
        {
            _builder = builder;
            _modifiers = modifiers;
            builder.MustNotBeNull();
            modifiers.MustNotBeNull();
        }

        public HtmlTag GenerateElement(ModelInfo info)
        {
            var element = _builder.Build(info);
            foreach (var modifier in _modifiers)
            {
                element = modifier.Modify(element, info);
            }
            return element;
        }
    }
}