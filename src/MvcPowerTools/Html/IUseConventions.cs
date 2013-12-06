namespace MvcPowerTools.Html
{
    public interface IUseConventions
    {
        IHaveModelConventions GetConventions(ModelInfo info);

        IBuildElement GetDefaultBuilder();
    }
}