namespace MvcPowerTools.Controllers.Internal
{
    interface IContextFacadeForSmartAction
    {
        bool IsPost { get; }
        bool IsModelValid { get; }
        bool EstablishModel();
        void SetResultForInvalidModel();
    }
}