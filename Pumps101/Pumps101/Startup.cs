using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pumps101.Startup))]
namespace Pumps101
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
