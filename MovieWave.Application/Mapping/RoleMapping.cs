using AutoMapper;
using MovieWave.Domain.Dto.Role;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class RoleMapping : Profile
{
	public RoleMapping()
	{
		CreateMap<Role, RoleDto>().ReverseMap();
	}
}