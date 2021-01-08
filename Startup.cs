using EasyCaching.Core.Configurations;
using Masivian.api.ruleta.Repositories;
using Masivian.api.ruleta.Repositories.Interfaces;
using Masivian.api.ruleta.Services;
using Masivian.api.ruleta.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Masivian.api.ruleta
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
			services.AddControllers();
			services.AddEasyCaching(options =>
			{
				//use redis cache
				options.UseRedis(redisConfig =>
				{
					//Setup connection
					redisConfig.DBConfig.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6379));
					redisConfig.DBConfig.AllowAdmin = true;
				},
					"roulette");
			});

			services.AddScoped<IRouletteRepository, RouletteRepository>();
			services.AddScoped<IRouletteService, RouletteService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
