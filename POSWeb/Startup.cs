using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(POSWeb.Startup))]
namespace POSWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
