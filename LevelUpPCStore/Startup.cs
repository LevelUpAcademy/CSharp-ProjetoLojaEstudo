using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LevelUpPCStore.Startup))]
namespace LevelUpPCStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
