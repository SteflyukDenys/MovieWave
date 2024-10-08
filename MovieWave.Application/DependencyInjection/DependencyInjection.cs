﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.Application.Mapping;
using MovieWave.Application.Services;
using MovieWave.Application.Validations.FluentValidations.MediaItem;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Interfaces.Services;

namespace MovieWave.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(MediaItemMapping));

		InitService(services);
	}

	private static void InitService(this IServiceCollection services)
	{
		services.AddScoped<IValidator<CreateMediaItemDto>, CreateMediaItemValidator>();
		services.AddScoped<IValidator<UpdateMediaItemDto>, UpdateMediaItemValidator>();

		services.AddScoped<IMediaItemService, MediaItemService>();
	}
}
