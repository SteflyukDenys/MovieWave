using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Dto.EpisodeVoice;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;
using TMDbLib.Objects.TvShows;

namespace MovieWave.Application.Services
{
	public class EpisodeVoiceService : IEpisodeVoiceService
	{
		private readonly IBaseRepository<EpisodeVoice> _episodeVoiceRepository;
		private readonly IBaseRepository<Episode> _episodeRepository;
		private readonly IBaseRepository<Voice> _voiceRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public EpisodeVoiceService(IBaseRepository<EpisodeVoice> episodeVoiceRepository, IBaseRepository<Episode> episodeRepository, IBaseRepository<Voice> voiceRepository, IMapper mapper, ILogger logger)
		{
			_episodeVoiceRepository = episodeVoiceRepository;
			_episodeRepository = episodeRepository;
			_voiceRepository = voiceRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<BaseResult<EpisodeVoiceDto>> AddEpisodeVoiceAsync(CreateEpisodeVoiceDto dto)
		{
			var episode = await _episodeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == dto.EpisodeId);
			if (episode == null)
			{
				return new BaseResult<EpisodeVoiceDto>
				{
					ErrorMessage = ErrorMessage.EpisodeNotFound,
					ErrorCode = (int)ErrorCodes.EpisodeNotFound
				};
			}

			var voice = await _voiceRepository.GetAll().FirstOrDefaultAsync(v => v.Id == dto.VoiceId);
			if (voice == null)
			{
				return new BaseResult<EpisodeVoiceDto>
				{
					ErrorMessage = ErrorMessage.VoiceNotFound,
					ErrorCode = (int)ErrorCodes.VoiceNotFound
				};
			}

			var episodeVoice = new EpisodeVoice
			{
				EpisodeId = dto.EpisodeId,
				VoiceId = dto.VoiceId,
				VideoUrl = dto.VideoUrl
			};

			await _episodeVoiceRepository.CreateAsync(episodeVoice);
			await _episodeVoiceRepository.SaveChangesAsync();

			return new BaseResult<EpisodeVoiceDto> { Data = _mapper.Map<EpisodeVoiceDto>(episodeVoice) };
		}

		public async Task<BaseResult> DeleteEpisodeVoiceAsync(Guid episodeId, long voiceId)
		{
			var ev = await _episodeVoiceRepository.GetAll()
				.FirstOrDefaultAsync(ev => ev.EpisodeId == episodeId && ev.VoiceId == voiceId);
			if (ev == null)
			{
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.EpisodeVoiceNotFound,
					ErrorCode = (int)ErrorCodes.EpisodeVoiceNotFound
				};
			}

			_episodeVoiceRepository.Remove(ev);
			await _episodeVoiceRepository.SaveChangesAsync();

			return new BaseResult<EpisodeVoiceDto> { Data = _mapper.Map<EpisodeVoiceDto>(ev) };

		}

		public async Task<CollectionResult<EpisodeVoiceDto>> GetVoicesByEpisodeIdAsync(Guid episodeId)
		{
			var evs = await _episodeVoiceRepository.GetAll().Where(x => x.EpisodeId == episodeId).ToListAsync();
			var dtos = evs.Select(e => _mapper.Map<EpisodeVoiceDto>(e)).ToList();

			return new CollectionResult<EpisodeVoiceDto> { Data = dtos, Count = dtos.Count };
		}
	}
}
