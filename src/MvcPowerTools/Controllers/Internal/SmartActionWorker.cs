namespace MvcPowerTools.Controllers.Internal
{
    internal class SmartActionWorker
    {
        private readonly IContextFacadeForSmartAction _context;
       
        public SmartActionWorker(IContextFacadeForSmartAction context)
        {
            _context = context;
        }

       
        public void CheckBeforeAction()
        {
            if (Context.IsPost)
            {
                HasModel=Context.EstablishModel();
                if (HasModel)
                {
                    HandleValidity();
                }
            }
        }

        void HandleValidity()
        {
            if (!Context.IsModelValid)
            {
                Context.SetResultForInvalidModel();
            }
        }

        public void CheckAfterAction()
        {
            if (HasModel) HandleValidity();
        }

        

        public bool HasModel { get; private set; }

        public IContextFacadeForSmartAction Context
        {
            get { return _context; }
        }
    }
}