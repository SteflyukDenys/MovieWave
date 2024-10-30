using Microsoft.AspNetCore.CookiePolicy;
using MovieWave.API;
using MovieWave.DAL.DependencyInjection;
using MovieWave.Application.DependencyInjection;
using MovieWave.Domain.Settings;
using Serilog;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection(GoogleAuthSettings.DefaultSection));

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieWave Swagger v1.0");
		c.SwaggerEndpoint("/swagger/v2/swagger.json", "MovieWave Swagger v2.0");
	});
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();
app.Run();