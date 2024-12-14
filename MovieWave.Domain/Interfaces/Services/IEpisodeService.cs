using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IEpisodeService
{
	Task<BaseResult<EpisodeDto>> CreateEpisodeAsync(CreateEpisodeDto dto);

	Task<BaseResult<EpisodeDto>> UpdateEpisodeAsync(UpdateEpisodeDto dto);

	Task<BaseResult> DeleteEpisodeAsync(Guid episodeId);

	Task<BaseResult<EpisodeDto>> GetEpisodeByIdAsync(Guid episodeId);

	Task<CollectionResult<EpisodeDto>> GetEpisodesByMediaItemIdAsync(Guid mediaItemId);
}