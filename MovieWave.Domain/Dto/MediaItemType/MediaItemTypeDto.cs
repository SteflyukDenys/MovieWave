using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.MediaItemType;

public record MediaItemTypeDto(int Id, string MediaItemName, SeoAdditionDto SeoAddition);