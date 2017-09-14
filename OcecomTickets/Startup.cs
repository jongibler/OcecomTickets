using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OcecomTickets.Startup))]
namespace OcecomTickets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
