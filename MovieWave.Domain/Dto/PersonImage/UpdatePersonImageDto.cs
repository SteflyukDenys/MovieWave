using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.PersonImage;

public class UpdatePersonImageDto
{
	public Guid Id { get; set; }

	public Guid PersonId { get; set; }

	public PersonImageType ImageType { get; set; }

}