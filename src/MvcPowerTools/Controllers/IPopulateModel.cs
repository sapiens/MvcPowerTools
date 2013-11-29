using System;

namespace MvcPowerTools.Controllers
{
    internal interface IPopulateModel
    {
        Type ModelType { get; }
    }
}