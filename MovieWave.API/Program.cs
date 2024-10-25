using MovieWave.API;
using MovieWave.DAL.DependencyInjection;
using MovieWave.Application.DependencyInjection;
using MovieWave.Domain.Settings;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

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

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
