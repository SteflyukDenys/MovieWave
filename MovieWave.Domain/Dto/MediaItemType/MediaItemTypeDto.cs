using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.MediaItemType;

public class MediaItemTypeDto
{
	public int Id { get; set; }
	public string MediaItemName { get; set; }
	public SeoAdditionInputDto SeoAddition = new SeoAdditionInputDto();
};