using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BaiThucHanh5.Startup))]
namespace BaiThucHanh5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
