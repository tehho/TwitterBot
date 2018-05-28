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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddEntityFrameworkSqlServer().AddDbContext<TwitterContext>(options => options.UseSqlServer(connectionString));
            
            services.AddTransient(x => Configuration.GetSection("Appsettings").Get<Appsettings>());
            
            services.AddTransient<TwitterServiceOptions>();
            services.AddScoped<TwitterService>();

            services.AddTransient<IRepository<TwitterProfile>, TwitterProfileRepository>();
            services.AddTransient<IRepository<Word>, WordRepository>();
            services.AddTransient<IRepository<WordOccurrence>, WordOccurrenceRepository>();
            services.AddTransient<IRepository<BotOption>, BotOptionRepository>();

            services.AddTransient<TwitterProfileTrainer>();

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
