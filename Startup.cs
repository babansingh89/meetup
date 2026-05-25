using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchoolERP_System.Startup))]
namespace SchoolERP_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
