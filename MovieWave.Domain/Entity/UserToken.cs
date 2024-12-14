using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class UserToken : BaseEntity<Guid>
{
	public UserToken()
	{
		Id = Guid.NewGuid();
	}
	public string RefreshToken { get; set; }

	public DateTime RefreshTokenExpireTime { get; set; }

	public Guid UserId { get; set; } // Foreign Key
	public User User { get; set; }
}