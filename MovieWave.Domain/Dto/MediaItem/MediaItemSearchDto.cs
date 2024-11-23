namespace MovieWave.Domain.Dto.MediaItem;

public record MediaItemSearchDto
{
		public string? Query { get; set; }
		public List<Guid>? TagIds { get; set; }
		public int? StatusId { get; set; }
		public int? MediaTypeId { get; set; }
		public string? SortBy { get; set; } // "ReleaseDate", "Name"...
		public bool SortDescending { get; set; } = true;
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
}