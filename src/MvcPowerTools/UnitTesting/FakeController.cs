using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcPowerTools.UnitTesting
{
    public class FakeController:Controller
    {
        public FakeController()
        {
            ViewData= new ViewDataDictionary();
            
        }  
    }

    public class FakeActionDescriptor:ActionDescriptor
    {
        /// <summary>
        /// Executes the action method by using the specified parameters and controller context.
        /// </summary>
        /// <returns>
        /// The result of executing the action method.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="parameters">The parameters of the action method.</param>
        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns the parameters of the action method.
        /// </summary>
        /// <returns>
        /// The parameters of the action method.
        /// </returns>
        public override ParameterDescriptor[] GetParameters()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the action method.
        /// </summary>
        /// <returns>
        /// The name of the action method.
        /// </returns>
        public override string ActionName
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the controller descriptor.
        /// </summary>
        /// <returns>
        /// The controller descriptor.
        /// </returns>
        public override ControllerDescriptor ControllerDescriptor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}