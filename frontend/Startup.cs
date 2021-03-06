using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Frontend
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddAuthentication("Bearer").AddIdentityServerAuthentication("Bearer", cfg =>
            {
                cfg.ApiName = "frontend";
                cfg.ApiSecret = "frontendpassword";
                cfg.Authority = "http://localhost:8888";
                cfg.RequireHttpsMetadata = false;
            });
#endif
            services.AddControllers().AddDapr();

            services.AddDaprClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
#if DEBUG
            app.UseAuthentication();

            app.UseAuthorization();
#endif
            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();

                endpoints.MapControllers();
            });
        }
    }
}