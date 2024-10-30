using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieWave.DAL;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Settings;

namespace MovieWave.API;

public static class Startup
{
	/// <summary>
	/// Connecting Authentication and Authorization
	/// </summary>
	/// <param name="services"></param>
	public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
	{
		//services.AddIdentityCore<User>()
		//	.AddRoles<IdentityRole<Guid>>()
		//	.AddEntityFrameworkStores<AppDbContext>()
		//	.AddSignInManager()
		//	.AddTokenProvider(TokenOptions.DefaultProvider, typeof(DataProtectorTokenProvider<User>))
		//	.AddTokenProvider(TokenOptions.DefaultEmailProvider, typeof(EmailTokenProvider<User>))
		//	.AddTokenProvider(TokenOptions.DefaultPhoneProvider, typeof(PhoneNumberTokenProvider<User>))
		//	.AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, typeof(AuthenticatorTokenProvider<User>));

		services.AddIdentityCore<User>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
			})
			.AddRoles<IdentityRole<Guid>>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddCookie(IdentityConstants.ExternalScheme)
			.AddJwtBearer(options =>
			{
				var jwtSettings = builder.Configuration.GetSection(JwtSettings.DefaultSection).Get<JwtSettings>();

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtKey)),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(1)
				};
			})
			.AddGoogle(options =>
			{
				var googleSettings = builder.Configuration.GetSection(GoogleAuthSettings.DefaultSection).Get<GoogleAuthSettings>();
				options.ClientId = googleSettings.ClientId;
				options.ClientSecret = googleSettings.ClientSecret;
				options.CallbackPath = googleSettings.CallbackPath;
				options.SignInScheme = IdentityConstants.ExternalScheme;
			});
		services.Configure<CookiePolicyOptions>(options =>
		{
			options.CheckConsentNeeded = context => true;
			options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
			options.Secure = CookieSecurePolicy.Always;
		});
		services.AddAuthorization();
	}

	/// <summary>
	/// Connecting swagger
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddApiVersioning()
			.AddApiExplorer(options =>
			{
				options.DefaultApiVersion = new ApiVersion(1, 0);
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
			});

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo()
			{
				Version = "v1",
				Title = "MovieWave.API",
				Description = "This is version 1.0",
				TermsOfService = new Uri("https://github.com/SteflyukDenys/MovieWave")
			});

			options.SwaggerDoc("v2", new OpenApiInfo()
			{
				Version = "v2",
				Title = "MovieWave.API",
				Description = "This is version 2.0",
				TermsOfService = new Uri("https://github.com/SteflyukDenys/MovieWave")
			});

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				In = ParameterLocation.Header,
				Description = "Please enter a valid token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference()
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					Array.Empty<string>()
				}
			});
		});
	}
}