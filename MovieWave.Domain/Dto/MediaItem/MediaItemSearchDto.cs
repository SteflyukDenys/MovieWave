namespace MovieWave.Domain.Dto.MediaItem;

public record MediaItemSearchDto
{
	public string? Query { get; set; }

	public List<long>? TagIds { get; set; }

	public int? StatusId { get; set; }

	public int? MediaTypeId { get; set; }

	public int? Year { get; set; }

	public List<long>? CountryIds { get; set; }

	public List<long>? StudioIds { get; set; }

	public string? SortBy { get; set; } // "ReleaseDate", "Name"...
	public bool SortDescending { get; set; } = true;
}