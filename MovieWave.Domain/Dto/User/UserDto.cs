namespace MovieWave.Domain.Dto.User;

public record UserDto
{
	public Guid Id { get; set; }

	public string Login { get; set; }

	public string Email { get; set; }

	public string PasswordHash { get; set; }

	public List<string> UserRoles { get; set; }

	public string? AvatarPath { get; set; }

	public string? Gender { get; set; }

	public string? Description { get; set; }

	public DateTime? Birthday { get; set; }

	public DateTime? LastSeenAt { get; set; }
}