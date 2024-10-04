using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto;

public record UserDto(
	Guid Id,
	UserRole UserRole,
	string Login,
	string Email,
	string? AvatarPath,
	string? BackdropPath,
	Gender? Gender,
	string? Description,
	string? Birthday, //DateTime
	string? LastSeenAt //DateTime
);
