using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(IS4WithIdenity.Areas.Identity.IdentityHostingStartup))]
namespace IS4WithIdenity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}