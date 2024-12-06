using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class PersonImage : AuditableEntity<Guid>
{
	public PersonImage()
	{
		Id = Guid.NewGuid();
	}

	public Guid PersonId { get; set; }
	public Person Person { get; set; }

	public PersonImageType ImageType { get; set; }
	public string ImagePath { get; set; }
}