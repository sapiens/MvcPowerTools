namespace MvcPowerTools.Html.Internals
{
    internal class PrimitiveTypeGenerator : BaseGenerator
    {
        public PrimitiveTypeGenerator(IHaveModelConventions conventions) : base(conventions)
        {
        }

        protected override void Configure(IHaveModelConventions conventions)
        {
            conventions.Builder = conventions.Registry.GetDefaultBuilder();
        }
    }
}