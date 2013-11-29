using System;
using System.Web.Mvc;

namespace MvcPowerTools.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ModelIsArgumentAttribute:ActionFilterAttribute
    {
        /// <summary>
        /// Specify the name of the action parameter which is the model
        /// </summary>
        /// <param name="paramName"></param>
        public ModelIsArgumentAttribute(string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException("paramName");
            ParameterName = paramName;
        }

        /// <summary>
        /// Specify the position of the model in action's argument list
        /// </summary>
        /// <param name="position"></param>
        public ModelIsArgumentAttribute(int position)
        {
            Position = position;
        }

        /// <summary>
        /// Specify the position of the model in the parameter list
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Specify which parameter is the model
        /// </summary>
        public string ParameterName { get; private set; }
    }
}