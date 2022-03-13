using System.Text;
using System.Text.Json;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Utilities;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

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
				options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.SaveToken = false;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = _config["JWT:Issuer"],
						ValidAudience = _config["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]))
					};
				});

			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
					options.InvalidModelStateResponseFactory = actionContext => new BadRequest(actionContext.ModelState))
				.AddJsonOptions(options =>
					options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);

			services.AddAutoMapper(options => options.AddProfile<MapperProfile>());
			services.AddScoped<IAuthenticationTokenGenerator, JwtGenerator>();

			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();
			services.AddSwaggerGen();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("swagger/v1/swagger.json", "Graduation Project API");
				c.RoutePrefix = string.Empty;
			});

			app.Use(async (context, next) =>
			{
				// Do loging
				HttpRequestLogger.Log(context.Request);
				// Do work that doesn't write to the Response.
				await next.Invoke();
				// Do logging or other work that doesn't write to the Response.
			});
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}
