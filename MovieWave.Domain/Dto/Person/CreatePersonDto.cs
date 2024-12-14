using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.Person;

public class CreatePersonDto
{
	public string FirstName { get; set; }
	public string LastName { get; set; }

	public DateTime? BirthDate { get; set; }
	public DateTime? DeathDate { get; set; }

	public string? Biography { get; set; }
}
