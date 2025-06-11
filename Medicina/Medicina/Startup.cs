using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Medicina.Startup))]
namespace Medicina
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
