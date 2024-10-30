using AutoMapper;
using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class RestrictedRatingMapping : Profile
{
	public RestrictedRatingMapping()
	{
		CreateMap<RestrictedRating, RestrictedRatingDto>().ReverseMap();

		CreateMap<UpdateRestrictedRatingDto, RestrictedRating>().ReverseMap();

	}
}