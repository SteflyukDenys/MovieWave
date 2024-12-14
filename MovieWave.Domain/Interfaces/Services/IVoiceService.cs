using MovieWave.Domain.Dto.Voice;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IVoiceService
{
	Task<BaseResult<VoiceDto>> CreateVoiceAsync(CreateVoiceDto dto);

	Task<BaseResult<VoiceDto>> UpdateVoiceAsync(UpdateVoiceDto dto);

	Task<BaseResult> DeleteVoiceAsync(long voiceId);

	Task<BaseResult<VoiceDto>> GetVoiceByIdAsync(long voiceId);

	Task<CollectionResult<VoiceDto>> GetAllVoicesAsync();
}