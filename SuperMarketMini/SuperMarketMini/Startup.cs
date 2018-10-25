using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SuperMarketMini.Startup))]
namespace SuperMarketMini
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}