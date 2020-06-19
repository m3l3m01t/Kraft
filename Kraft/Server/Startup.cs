#define HAVE_REDIS

using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

using GraphQL.Server.Ui.GraphiQL;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Server;
using Microsoft.EntityFrameworkCore;
using Kraft.Server.Data;
using Kraft.Server.Hubs;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using Kraft.Server.Services;

namespace Kraft.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true; // optional
            })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

            // services.AddGraphQL(opt =>
            // {
            // });
            services.AddHealthChecks();

            services.AddSignalR();
            services.AddControllersWithViews();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddRazorPages();

            //services.AddDbContext<KraftContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("KraftContext")));
            services.AddDbContext<KraftContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("SqliteContext")));

#if HAVE_REDIS
            services.AddTransient<IConnectionMultiplexer>(sp => {

                var config = sp.GetService<IOptions<AppSettings>>().Value;

                return ConnectionMultiplexer.Connect(config.RedisHost);
            });
#endif

            services.AddSingleton<Beacon>();

            services.AddHostedService<HeartBeator>();

            services.Configure<AppSettings>(Configuration.GetSection("Settings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseHealthChecks("/healthz");

            app.ApplicationServices
              .UseBootstrapProviders()
              .UseFontAwesomeIcons();

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapRazorPages();
                endpoints.MapControllers();

                endpoints.MapHub<KafkaHub>("/khub");

                // endpoints.MapHealthChecks("/healthz").RequireAuthorization();
                endpoints.MapFallbackToFile("index.html");

                //var nested = endpoints.CreateApplicationBuilder();
            });

        }
    }
}
