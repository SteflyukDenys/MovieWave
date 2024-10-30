﻿using AutoMapper;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class PersonMapping : Profile
{
	public PersonMapping()
	{
		CreateMap<Person, PersonDto>().ReverseMap();

		//CreateMap<CreatePersonDto, Person>()
		//	.AfterMap((src, dest) => dest.Id = Guid.NewGuid())
		//	.ReverseMap();
	}
}