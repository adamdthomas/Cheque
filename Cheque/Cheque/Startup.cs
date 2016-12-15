using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cheque.Startup))]
namespace Cheque
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
