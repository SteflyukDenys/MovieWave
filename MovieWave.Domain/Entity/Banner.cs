using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Banner : BaseEntity<Guid>
{
	public Banner()
	{
		Id = Guid.NewGuid();
	}
	public string Title { get; set; }
	public string ImageUrl { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; } // When to start displaying the banner
	public DateTime? EndDate { get; set; } // When to stop displaying the banner
	public int DisplayOrder { get; set; } // Order in which banners are displayed
}