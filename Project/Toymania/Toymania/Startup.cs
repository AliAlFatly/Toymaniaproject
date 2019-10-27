using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Toymania.Startup))]
namespace Toymania
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
