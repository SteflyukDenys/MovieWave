using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class EpisodeService : IEpisodeService
	{
		private readonly IBaseRepository<Episode> _episodeRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public EpisodeService(IBaseRepository<Episode> episodeRepository, IBaseRepository<MediaItem> mediaItemRepository, IMapper mapper, ILogger logger)
		{
			_episodeRepository = episodeRepository;
			_mediaItemRepository = mediaItemRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<BaseResult<EpisodeDto>> CreateEpisodeAsync(CreateEpisodeDto dto)
		{
			var mediaItem = await _mediaItemRepository.GetAll()
				.Include(m => m.Episodes)
				.Include(m => m.Seasons)
				.FirstOrDefaultAsync(m => m.Id == dto.MediaItemId);

			if (mediaItem == null)
			{
				return new BaseResult<EpisodeDto>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			var episode = new Episode
			{
				Id = Guid.NewGuid(),
				MediaItemId = dto.MediaItemId,
				SeasonId = dto.SeasonId ?? Guid.Empty,
				Name = dto.Name,
				Description = dto.Description,
				Duration = dto.Duration,
				AirDate = dto.AirDate,
				IsFiller = dto.IsFiller,
				ImagePath = dto.ImagePath
			};

			await _episodeRepository.CreateAsync(episode);
			await _episodeRepository.SaveChangesAsync();

			if (mediaItem.Seasons.Any() || (mediaItem.Episodes.Count() > 1))
			{
				mediaItem.MediaItemTypeId = 2; // Series
			}
			else
			{
				mediaItem.MediaItemTypeId = 1; // Film
			}

			_mediaItemRepository.Update(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			var episodeDto = _mapper.Map<EpisodeDto>(episode);
			return new BaseResult<EpisodeDto> { Data = episodeDto };
		}

		public async Task<BaseResult<EpisodeDto>> UpdateEpisodeAsync(UpdateEpisodeDto dto)
		{
			var episode = await _episodeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == dto.Id);
			if (episode == null)
			{
				return new BaseResult<EpisodeDto>
				{
					ErrorMessage = ErrorMessage.EpisodeNotFound,
					ErrorCode = (int)ErrorCodes.EpisodeNotFound
				};
			}

			episode.Name = dto.Name;
			episode.Description = dto.Description;
			episode.Duration = dto.Duration;
			episode.AirDate = dto.AirDate;
			episode.IsFiller = dto.IsFiller;
			episode.ImagePath = dto.ImagePath;

			_episodeRepository.Update(episode);
			await _episodeRepository.SaveChangesAsync();

			return new BaseResult<EpisodeDto> { Data = _mapper.Map<EpisodeDto>(episode) };
		}

		public async Task<BaseResult> DeleteEpisodeAsync(Guid episodeId)
		{
			var episode = await _episodeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == episodeId);
			if (episode == null)
			{
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.EpisodeNotFound,
					ErrorCode = (int)ErrorCodes.EpisodeNotFound
				};
			}

			_episodeRepository.Remove(episode);
			await _episodeRepository.SaveChangesAsync();

			return new BaseResult<EpisodeDto> { Data = _mapper.Map<EpisodeDto>(episode) };
		}

		public async Task<BaseResult<EpisodeDto>> GetEpisodeByIdAsync(Guid episodeId)
		{
			var episode = await _episodeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == episodeId);
			if (episode == null)
			{
				return new BaseResult<EpisodeDto>
				{
					ErrorMessage = ErrorMessage.EpisodeNotFound,
					ErrorCode = (int)ErrorCodes.EpisodeNotFound
				};
			}

			return new BaseResult<EpisodeDto> { Data = _mapper.Map<EpisodeDto>(episode) };
		}

		public async Task<CollectionResult<EpisodeDto>> GetEpisodesByMediaItemIdAsync(Guid mediaItemId)
		{
			var episodes = await _episodeRepository.GetAll().Where(e => e.MediaItemId == mediaItemId).ToListAsync();
			var dtos = episodes.Select(e => _mapper.Map<EpisodeDto>(e)).ToList();

			return new CollectionResult<EpisodeDto>
			{
				Data = dtos,
				Count = dtos.Count
			};
		}
	}
}
