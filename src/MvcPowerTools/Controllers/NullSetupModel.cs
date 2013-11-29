namespace MvcPowerTools.Controllers
{
    public class NullSetupModel<T>:ISetupModel<T> where T : class, new()
    {
        public static readonly NullSetupModel<T> Instance=new NullSetupModel<T>();

        private NullSetupModel()
        {
            
        }
        public void Setup(T model)
        {
            
        }
    }
}