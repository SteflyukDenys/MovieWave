using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.PersonImage;

public class PersonImageDto
{
	public Guid Id { get; set; }

	public Guid PersonId { get; set; }

	public PersonImageType ImageType { get; set; }

	public string ImagePath { get; set; }
}