using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Ads.WebApi.Startup))]

namespace Ads.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
