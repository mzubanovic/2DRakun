using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(_2DRakun.Startup))]

namespace _2DRakun
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configuration for the self-hosted application will go here.
            // This will involve setting up middleware to handle MVC requests.
            // For now, this class serves as the entry point for the OWIN host.
        }
    }
}
