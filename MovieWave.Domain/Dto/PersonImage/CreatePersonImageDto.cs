using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.PersonImage;

public class CreatePersonImageDto
{
	public Guid PersonId { get; set; }

	public PersonImageType ImageType { get; set; }

}