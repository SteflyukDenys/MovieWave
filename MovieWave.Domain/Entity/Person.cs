using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Person : AuditableEntity<Guid>
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime? BirthDate { get; set; }
	public DateTime? DeathDate { get; set; }
	public string? ImagePath { get; set; }
	public string? Biography { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }

	public ICollection<MediaItemPerson> MoviePeople { get; set; }
}
