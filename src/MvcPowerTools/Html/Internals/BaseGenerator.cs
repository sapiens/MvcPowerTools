using HtmlTags;

namespace MvcPowerTools.Html.Internals
{
    internal abstract class BaseGenerator : IGenerateHtml
    {
        private readonly IHaveModelConventions _conventions;

        protected BaseGenerator(IHaveModelConventions conventions)
        {
            _conventions = conventions;
        }

        protected abstract void Configure(IHaveModelConventions conventions);

        public HtmlTag GenerateElement(ModelInfo info)
        {
            Configure(_conventions);
            return _conventions.CreateGenerator().GenerateElement(info);
        }
    }
}