using System;

namespace MvcPowerTools.Controllers
{
    internal class PopulateModel<T>:IPopulateModel
    {
        public PopulateModel(Action<T> action)
        {
            ModelType = typeof (T);
            Map = action;
        }

        public Action<T> Map { get; set; }

        public Type ModelType { get; set; }
    }
}