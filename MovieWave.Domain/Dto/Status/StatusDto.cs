using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.Status;

public class StatusDto
{
	public long Id { get; set; }
	public StatusType StatusType { get; set; }
	public string? Description { get; set; }
};