using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Dto.User;

public class RateMediaItemDto
{
	[Required]
	public Guid MediaItemId { get; set; }

	[Range(1, 10)]
	public int Rating { get; set; }
}