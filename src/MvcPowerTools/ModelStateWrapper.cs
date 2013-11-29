using System;
using System.Web.Mvc;
using CavemanTools.Model.Validation;

namespace MvcPowerTools
{
	/// <summary>
	/// IValidationDictionary implementation for ModelState
	/// </summary>
	public class ModelStateWrapper : IValidationDictionary
	{

		private readonly ModelStateDictionary _modelState;
		
		public ModelStateWrapper(ModelStateDictionary modelState)
		{
			_modelState = modelState;			
		}
	      

		#region IValidationDictionary Members

		public void AddError(string key, string errorMessage)
		{
			_modelState.AddModelError(key, errorMessage);
		}

		public bool HasErrors
		{
			get { return !IsValid; }
		}

		public bool IsValid
		{
			get { return _modelState.IsValid; }
		}

	    public void CopyTo(IValidationDictionary other)
	    {
	        throw new NotImplementedException();
	    }

	    #endregion
	}
}