using AutoMapper;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class PersonMapping : Profile
{
	public PersonMapping()
	{
		CreateMap<Person, PersonDto>().ReverseMap();

		CreateMap<CreatePersonDto, Person>().ReverseMap();

		CreateMap<UpdatePersonDto, Person>().ReverseMap();

		CreateMap<MediaItemPerson, PersonRolesDto>()
			.ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
			.ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.PersonRole.ToString()));
	}
}