using AutoMapper;
using MovieWave.Domain.Dto.Review;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class ReviewMapping : Profile
{
	public ReviewMapping()
	{
		CreateMap<Review, ReviewDto>().ReverseMap();
	}
}