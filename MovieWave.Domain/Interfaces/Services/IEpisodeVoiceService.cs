using MovieWave.Domain.Dto.EpisodeVoice;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IEpisodeVoiceService
{
	Task<BaseResult<EpisodeVoiceDto>> AddEpisodeVoiceAsync(CreateEpisodeVoiceDto dto);

	Task<BaseResult> DeleteEpisodeVoiceAsync(Guid episodeId, long voiceId);

	Task<CollectionResult<EpisodeVoiceDto>> GetVoicesByEpisodeIdAsync(Guid episodeId);
}