using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseFly.Startup))]
namespace HouseFly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
