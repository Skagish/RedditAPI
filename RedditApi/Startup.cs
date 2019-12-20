using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RedditApi.core.Interfaces.IServices;
using RedditApi.Models.IRepo;
using RedditApi.Repositories;
using RedditApi.Services;
using Microsoft.Extensions.Logging;

namespace RedditApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RedditDbSettings>(
              options =>
              {
                  options.ConnectionString = Configuration.GetSection("RedditDbSettings:ConnectionString").Value;
                  options.DatabaseName = Configuration.GetSection("RedditDbSettings:Database").Value;
                  options.Container = Configuration.GetSection("RedditDbSettings:Container").Value;
                  //options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
              });

            services.Configure<RedditDbSettings>(
                Configuration.GetSection(nameof(RedditDbSettings)));
            services.AddSingleton<IRedditDbSettings>(sp =>
                sp.GetRequiredService<IOptions<RedditDbSettings>>().Value);
            services.AddSingleton<ThreadService>();
            services.AddTransient<IThreadRepository, ThreadRepository>();
            services.AddTransient<IThreadService, ThreadService>();
            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, ILoggerProvider provider)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
