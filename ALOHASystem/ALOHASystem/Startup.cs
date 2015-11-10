using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ALOHASystem.Startup))]
namespace ALOHASystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
