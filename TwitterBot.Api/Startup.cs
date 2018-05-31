using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Options;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Logging;
using TwitterBot.Infrastructure.Repository;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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

            var appsettings = Configuration.GetSection("Appsettings").Get<Appsettings>();

            services.AddTransient(x => appsettings);

            services.AddTransient<Infrastructure.Logging.ILogger>(x => new FileLogger(appsettings.Logfile));

            services.AddTransient(x => new TwitterServiceOptions(appsettings));
            services.AddScoped<TwitterService>();

            services.AddScoped<IRepository<TwitterProfile>, TwitterProfileRepository>();
            services.AddScoped<IRepository<Word>, WordRepository>();
            services.AddScoped<IRepository<WordOccurrence>, WordOccurrenceRepository>();
            services.AddScoped<IRepository<BotOptions>, BotOptionRepository>();

            services.AddScoped<TwitterProfileTrainer>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc();

            app.UseDirectoryBrowser();
            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = 
                new List<string> { "index.html" } });

        }
    }
}
