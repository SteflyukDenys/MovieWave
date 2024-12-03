namespace MovieWave.Domain.Dto.Banner;

public class CreateBannerDto
{
	public string Title { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public int DisplayOrder { get; set; }
}