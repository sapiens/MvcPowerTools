using System;
using System.Web.Mvc;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.Controllers.Internal
{
    internal class SmartContextFacade:IContextFacadeForSmartAction
    {
        private readonly Controller _ctrl;
        private readonly Func<dynamic> _modelIdentifier;
        private readonly Type _validationFailedPolicy;

        private readonly IValidationFailedPolicyFactory _factory;

        //for testing
        internal SmartContextFacade()
        {
            
        }

        public SmartContextFacade(Controller ctrl,Func<dynamic> modelIdentifier,Type validationFailedPolicy,IValidationFailedPolicyFactory factory)
        {
            ctrl.MustNotBeNull();
            modelIdentifier.MustNotBeNull();
            validationFailedPolicy.MustNotBeNull();
            factory.MustNotBeNull();
            _ctrl = ctrl;
            _modelIdentifier = modelIdentifier;
            _validationFailedPolicy = validationFailedPolicy;
            IsPost = ctrl.HttpContext.Request.IsPost();
            _factory = factory;
        }

        public ActionResult Result { get; set; }

        public IOverrideValidationFailedPolicy PolicyOverride { get; set; }

        public bool IsPost { get; set; }
        public bool IsModelValid
        {
            get { return _ctrl.ModelState.IsValid; }
        }

        private dynamic _model;


        public bool EstablishModel()
        {
            _model = _modelIdentifier();
            return _model != null;
        }

        public void SetResultForInvalidModel()
        {
            Result = null;
            dynamic pol=null;
            if (PolicyOverride != null)
            {
                pol = PolicyOverride.GetPolicy(_model);
            }
            else
            {
                pol = _factory.GetInstance(_validationFailedPolicy, _model);
            }
            var result = pol.GetResult(_ctrl);
            Result = result;           
        }
    }
}