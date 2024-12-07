using AutoMapper;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class PersonImageMapping : Profile
{
	public PersonImageMapping()
	{
		CreateMap<PersonImage, PersonImageDto>().ReverseMap();

		CreateMap<CreatePersonImageDto, PersonImage>().ReverseMap();

		CreateMap<UpdatePersonImageDto, PersonImage>().ReverseMap();
	}
}