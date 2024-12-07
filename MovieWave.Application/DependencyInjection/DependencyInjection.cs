using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.Application.Mapping;
using MovieWave.Application.Services;
using MovieWave.Application.Validations.FluentValidations.MediaItem;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Interfaces.Validations;
using PersonImageService = MovieWave.Application.Services.PersonImageService;

namespace MovieWave.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(MediaItemMapping));
		services.AddAutoMapper(typeof(MediaItemTypeMapping));
		services.AddAutoMapper(typeof(StatusMapping));
		services.AddAutoMapper(typeof(CountryMapping));
		services.AddAutoMapper(typeof(UserMapping));
		services.AddAutoMapper(typeof(SeoAdditionMapping));
		services.AddAutoMapper(typeof(TagMapping));
		services.AddAutoMapper(typeof(RestrictedRatingMapping));
		services.AddAutoMapper(typeof(NotificationMapping));
		services.AddAutoMapper(typeof(ReviewMapping));
		services.AddAutoMapper(typeof(AttachmentMapping));
		services.AddAutoMapper(typeof(CommentMapping));
		services.AddAutoMapper(typeof(EpisodeMapping));
		services.AddAutoMapper(typeof(PersonMapping));
		services.AddAutoMapper(typeof(StudioMapping));
		services.AddAutoMapper(typeof(RoleMapping));
		services.AddAutoMapper(typeof(BannerMapping));
		services.AddAutoMapper(typeof(PersonImageMapping));

		InitService(services);
	}

	private static void InitService(this IServiceCollection services)
	{
		services.AddScoped<IMediaItemValidator, MediaItemValidator>();
		services.AddScoped<IValidator<CreateMediaItemDto>, CreateMediaItemValidator>();
		services.AddScoped<IValidator<UpdateMediaItemDto>, UpdateMediaItemValidator>();

		services.AddScoped<IDataSeederService, DataSeederService>();

		services.AddScoped<IBannerService, BannerService>();
		services.AddScoped<IAttachmentService, AttachmentService>();
		services.AddScoped<IMediaItemService, MediaItemService>();
		services.AddScoped<IMediaItemTypeService, MediaItemTypeService>();
		services.AddScoped<ICountryService, CountryService>();
		services.AddScoped<IStatusService, StatusService>();
		services.AddScoped<IRestrictedRatingService, RestrictedRatingService>();
		services.AddScoped<IStudioService, StudioService>();
		services.AddScoped<ISeoAdditionService, SeoAdditionService>();
		services.AddScoped<ITagService, TagService>();
		services.AddScoped<IStudioService, StudioService>();
		services.AddScoped<IPersonService, PersonService>();
		services.AddScoped<IPersonImageService, PersonImageService>();

		services.AddScoped<IStorageService, StorageService>();

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
		services.AddScoped<IRoleService, RoleService>();

	}
}
