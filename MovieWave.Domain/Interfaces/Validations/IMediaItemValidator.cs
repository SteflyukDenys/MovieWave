using MovieWave.Domain.Entity;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Validations;

public interface IMediaItemValidator : IBaseValidator<MediaItem>
{
	BaseResult CreateValidator(MediaItem mediatem);
}
