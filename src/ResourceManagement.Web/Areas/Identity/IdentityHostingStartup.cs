using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManagement.Web.Models;

[assembly: HostingStartup(typeof(ResourceManagement.Web.Areas.Identity.IdentityHostingStartup))]
namespace ResourceManagement.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ResourceManagementWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ResourceManagementWebContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ResourceManagementWebContext>();
            });
        }
    }
}