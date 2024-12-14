using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class Person : AuditableEntity<Guid>
{
	public Person()
	{
		Id = Guid.NewGuid();
	}

	public string FirstName { get; set; }
	public string LastName { get; set; }

	public string? Biography { get; set; }

	public DateTime? BirthDate { get; set; }
	public DateTime? DeathDate { get; set; }

	public ICollection<PersonImage> Images { get; set; } = new List<PersonImage>();

	public ICollection<MediaItemPerson> MediaItemPeople { get; set; } = new List<MediaItemPerson>();
}
