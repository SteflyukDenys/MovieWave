using AutoMapper;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class MediaItemMapping : Profile
{
    public MediaItemMapping()
    {
        CreateMap<MediaItem, MediaItemDto>().ReverseMap();
    }
}
