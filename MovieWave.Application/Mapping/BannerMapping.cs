using AutoMapper;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class BannerMapping : Profile
{
	public BannerMapping()
	{
		CreateMap<Banner, BannerDto>().ReverseMap();
		CreateMap<CreateBannerDto, Banner>().ReverseMap();
		CreateMap<UpdateBannerDto, Banner>().ReverseMap();
	}
}