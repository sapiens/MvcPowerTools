using System;

namespace MvcPowerTools.HtmlConventions
{
    public class MissingConventionProfileException : Exception
    {
        public MissingConventionProfileException()
            : base("There is no MvcConventions profile set for the request")
        {
            
        }

    }
}