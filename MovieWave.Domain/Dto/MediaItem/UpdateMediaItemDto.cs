using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.MediaItem;

public class UpdateMediaItemDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string? OriginalName { get; set; }
	public string? Description	{ get; set; }
	public int? Duration { get; set; }
	public int? EpisodesCount { get; set; }
	public decimal? ImdbScore { get; set; }

	public int MediaItemTypeId { get; set; }
	public long? StatusId { get; set; }

	public long? RestrictedRatingId { get; set; }

	public DateTime? FirstAirDate { get; set; }

	public DateTime? LastAirDate { get; set; }
	public DateTime? PublishedAt { get; set; }
	public SeoAdditionInputDto SeoAddition { get; set; }

	public List<Guid> TagIds { get; set; }
	public List<long> CountryIds { get; set; }
	public List<long> StudioIds { get; set; }
	public List<PersonRoleDto>? PersonRoles { get; set; }
};