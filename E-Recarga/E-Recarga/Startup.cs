using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Recarga.Startup))]
namespace E_Recarga
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
