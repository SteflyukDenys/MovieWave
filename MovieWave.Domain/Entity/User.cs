using Microsoft.AspNetCore.Identity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity
{
	public class User : IdentityUser<Guid>
	{
		public UserRole UserRole { get; set; }

		public string Login { get; set; }

		public string? AvatarPath { get; set; }
		public string? BackdropPath { get; set; }
		public Gender? Gender { get; set; }
		public string? Description { get; set; }
		public DateTime? Birthday { get; set; }
		public DateTime? LastSeenAt { get; set; }

		// One-to-Many
		public ICollection<UserMediaItemList> UserMediaItemLists { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<UserSubscription> UserSubscriptions { get; set; }
		public ICollection<Notification> Notifications { get; set; }
		public ICollection<Review> Reviews { get; set; }
	}
}
