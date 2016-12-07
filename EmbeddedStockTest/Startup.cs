using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmbeddedStockTest.Startup))]
namespace EmbeddedStockTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
