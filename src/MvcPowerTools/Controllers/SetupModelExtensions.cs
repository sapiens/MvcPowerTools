namespace MvcPowerTools.Controllers
{
    public static class SetupModelExtensions
    {
        public static T Create<T>(this ISetupModel<T> setup) where T : class, new()
        {
            var res= new T();
            setup.Setup(res);
            return res;
        }
    }
}