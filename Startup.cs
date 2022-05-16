using System.Text;
using System.Text.Json;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GraduationProjectAPI
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContextPool<ApplicationDbContext>(options =>
				options.UseSqlServer(_config.GetConnectionString("DefaultConnection"), o => o.UseNetTopologySuite()));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
					{
						options.SaveToken = true;
						options.TokenValidationParameters = new TokenValidationParameters()
						{
							ValidateLifetime = true,
							ValidateIssuer = false,
							ValidateAudience = false,
							ValidateIssuerSigningKey = true,
							ValidIssuer = _config["JWT:Issuer"],
							ValidAudience = _config["JWT:Audience"],
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]))
						};
					});

			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
					options.InvalidModelStateResponseFactory = actionContext => new BadRequest(actionContext.ModelState))
				.AddJsonOptions(options =>
					{
						options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
						options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
						options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
					});

			services.AddScoped<IAuthenticationTokenGenerator, JwtGenerator>();

			services.AddMemoryCache();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseDeveloperExceptionPage();
			//app.UseStaticFiles();
			app.Use(async (context, next) =>
			{
				//new Task(() =>
				//{
				//	if (context.Request.Method == "POST" && !string.IsNullOrWhiteSpace(context.Request.ContentType))
				//		HttpRequestLogger.Log(context.Request);
				//}).Start();

				Paths.InitCommon(context.Request);
				await next.Invoke();
			});

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}
