using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RealMax.Startup))]
namespace RealMax
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
