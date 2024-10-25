using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.Application.Mapping;
using MovieWave.Application.Services;
using MovieWave.Application.Validations;
using MovieWave.Application.Validations.FluentValidations.MediaItem;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Interfaces.Validations;

namespace MovieWave.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(MappingProfile));


		InitService(services);
	}

	private static void InitService(this IServiceCollection services)
	{
		services.AddScoped<IMediaItemValidator, MediaItemValidator>();
		services.AddScoped<IValidator<CreateMediaItemDto>, CreateMediaItemValidator>();
		services.AddScoped<IValidator<UpdateMediaItemDto>, UpdateMediaItemValidator>();

		services.AddScoped<IMediaItemService, MediaItemService>();
		services.AddScoped<IMediaItemTypeService, MediaItemTypeService>();
		services.AddScoped<IDataSeederService, DataSeederService>();
		services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
		services.AddScoped<IAuthService, AuthService>();


	}
}
