using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    public abstract class AbstractResultForInvalidModel<T>:IResultForInvalidModel<T> where T : class, new()
    {
        private ISetupModel<T> _modelSetup=NullSetupModel<T>.Instance;
        
        
        public ISetupModel<T> ModelSetup
        {
            get { return _modelSetup; }
            set
            {
                if (value != null) _modelSetup = value;
            }
        }   

        public object Data { get; set; }
        public abstract ActionResult GetResult(Controller controller);

        public T Model
        {
            get { return (T) Data; }
            set { Data = value; }
        }

        protected void SetupModel()
        {
            _modelSetup.Setup(Model);
        }
    }
}