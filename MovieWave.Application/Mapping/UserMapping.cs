using AutoMapper;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class UserMapping : Profile
{
	public UserMapping()
	{
		CreateMap<User, UserDto>()
			//.ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole.ToString()))
			.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null));

		CreateMap<RegisterUserDto, User>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
			.AfterMap((src, dest) => dest.Id = Guid.NewGuid());
	}
}