namespace MovieWave.Domain.Entity;

public class UserRole
{
	public Guid UserId { get; set; }
	public User User { get; set; }

	public long RoleId { get; set; }
	public Role Role { get; set; }
}