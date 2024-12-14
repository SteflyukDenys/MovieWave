using AutoMapper;
using MovieWave.Domain.Dto.Review;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class ReviewMapping : Profile
{
	public ReviewMapping()
	{
		CreateMap<Review, ReviewDto>()
			.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
			.ReverseMap();
	}
}