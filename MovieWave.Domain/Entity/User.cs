using Microsoft.AspNetCore.Identity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces;

namespace MovieWave.Domain.Entity;

public class User : IdentityUser<Guid>, IAuditable
{
	public User()
	{
		Id = Guid.NewGuid();
	}
	public UserRole UserRole { get; set; }

	public string Login { get; set; }

	public string? AvatarPath { get; set; }
	public string? BackdropPath { get; set; }
	public Gender? Gender { get; set; }
	public string? Description { get; set; }
	public DateTime? Birthday { get; set; }
	public DateTime? LastSeenAt { get; set; }

	public UserToken UserToken { get; set; }

	public DateTime CreatedAt { get; set; }
	public long CreatedBy { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public long? UpdatedBy { get; set; }

	// One-to-Many
	public ICollection<UserMediaItemList> UserMediaItemLists { get; set; }
	public ICollection<Comment> Comments { get; set; }
	public ICollection<UserSubscription> UserSubscriptions { get; set; }
	public ICollection<Notification> Notifications { get; set; }
	public ICollection<Review> Reviews { get; set; }
}
