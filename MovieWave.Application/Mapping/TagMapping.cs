using AutoMapper;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class TagMapping : Profile
{
	public TagMapping()
	{
		CreateMap<Tag, TagDto>().ReverseMap();

		CreateMap<CreateTagDto, Tag>().ReverseMap();

		CreateMap<UpdateTagDto, Tag>().ReverseMap();

	}
}