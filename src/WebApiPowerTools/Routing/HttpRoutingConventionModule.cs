namespace WebApiPowerTools
{
    public abstract class HttpRoutingConventionModule
    {
        public abstract void Configure(IConfigureHttpRoutingConventions routing);
    }
}