using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Leagues.Startup))]
namespace Leagues
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
