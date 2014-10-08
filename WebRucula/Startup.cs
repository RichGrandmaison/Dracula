using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebRucula.Startup))]
namespace WebRucula
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
