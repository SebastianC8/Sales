using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Sales.API.Startup))]

namespace Sales.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
