using AutoMapper;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class UserMapping : Profile
{
	public UserMapping()
	{
		CreateMap<User, UserDto>()
			.ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()))
			.ForMember(dest => dest.AvatarPath, opt => opt.MapFrom(src => src.AvatarPath))
			.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
			.ForMember(dest => dest.LastSeenAt, opt => opt.MapFrom(src => src.LastSeenAt))
			.ReverseMap();

		CreateMap<RegisterUserDto, User>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
			.ReverseMap();

		CreateMap<UpdateUserDto, User>().ReverseMap();
	}
}