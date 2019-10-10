using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TradeTracker.Startup))]
namespace TradeTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
