using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Temp.Startup))]
namespace Temp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
