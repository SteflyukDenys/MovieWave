namespace MovieWave.Domain.Dto.User;

public record UserDto
{
	public Guid Id { get; init; }
	public string Login { get; init; }
	public string Email { get; init; }
	public string UserRole { get; init; }
	public string? AvatarPath { get; init; }
	public string? BackdropPath { get; init; }
	public string? Gender { get; init; }
	public string? Description { get; init; }
	public DateTime? Birthday { get; init; }
	public DateTime? LastSeenAt { get; init; }
}