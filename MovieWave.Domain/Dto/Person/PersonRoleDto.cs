using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.Person;

public class PersonRoleDto
{
	public Guid PersonId { get; set; }

	public int Role { get; set; }
}