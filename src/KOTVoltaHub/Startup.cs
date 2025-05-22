using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KOTVoltaHub;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add MVC services for controllers and views
        services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("Client/Home//Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // app.UseAuthentication(); // Uncomment if authentication is needed
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            // Client Area: Default route
            endpoints.MapAreaControllerRoute(
                name: "client_area",
                areaName: "Client",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Client Area: Short route for HomeController
            endpoints.MapAreaControllerRoute(
                name: "client_pages",
                areaName: "Client",
                pattern: "{action=Index}/{id?}",
                defaults: new { controller = "Home" });
        });
    }
}