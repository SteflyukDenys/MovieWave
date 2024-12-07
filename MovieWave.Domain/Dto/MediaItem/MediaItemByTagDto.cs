using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Dto.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWave.Domain.Dto.MediaItem;

public class MediaItemByTagDto
{
	public string? Thumbnail { get; set; }

	public string Name { get; set; }

	public int? ReleaseYear { get; set; }

	public List<TagNameDto> Tags { get; set; }

	public SeoAdditionInputDto SeoAddition { get; set; }
}