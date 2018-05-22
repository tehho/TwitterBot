using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Options;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api
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
            services.AddEntityFrameworkSqlite().AddDbContext<TwitterContext>(options => options.UseSqlite("Filename=TwitterBot.db"));


            //TODO Fixa inläsning från appsettings;
            var customerToken = new Token
            {
                Key = "GjMrzt4a9YJqKXRTNKjLN2CVi",
                Secret = "w3koS8pDXMxDscBZnT7VFgGFeoNgv0qxgUa5YYcvrv2WoysfRD"
            };
            var accessToken = new Token
            {
                Key = "998554298735845382-cHyJyzufzzSUzceD79y8zb0IkbfrPxi",
                Secret = "B72OlpxIme0yz3ZHRVw0mCMDxKukXTcNuOvhD9d0ySCX8"
            };

            services.AddScoped(x => new TwitterService(new TwitterProfile("rowboat2018"), customerToken, accessToken));
            services.AddTransient<IRepository<TwitterProfile>, TwitterProfileRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();
            app.UseDirectoryBrowser();
            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = 
                new List<string> { "index.html" } });

        }
    }
}
