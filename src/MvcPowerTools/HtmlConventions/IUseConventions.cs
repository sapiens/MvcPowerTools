namespace MvcPowerTools.HtmlConventions
{
    public interface IUseConventions
    {
        IHaveModelConventions GetConventions(ModelInfo info);

        IBuildElement GetDefaultBuilder();
    }
}