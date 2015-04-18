using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeFest.NinjaFamily.FamilyTreeApp.Startup))]
namespace CodeFest.NinjaFamily.FamilyTreeApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
