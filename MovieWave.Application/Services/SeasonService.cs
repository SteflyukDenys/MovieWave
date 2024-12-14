using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class SeasonService : ISeasonService
	{
		private readonly IBaseRepository<Season> _seasonRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public SeasonService(IBaseRepository<Season> seasonRepository, IBaseRepository<MediaItem> mediaItemRepository, IMapper mapper, ILogger logger)
		{
			_seasonRepository = seasonRepository;
			_mediaItemRepository = mediaItemRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<BaseResult<SeasonDto>> CreateSeasonAsync(CreateSeasonDto dto)
		{
			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(m => m.Id == dto.MediaItemId);
			if (mediaItem == null)
			{
				return new BaseResult<SeasonDto>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			var season = new Season
			{
				Id = Guid.NewGuid(),
				MediaItemId = dto.MediaItemId,
				Name = dto.Name
			};

			await _seasonRepository.CreateAsync(season);
			await _seasonRepository.SaveChangesAsync();

			mediaItem.MediaItemTypeId = 2;
			_mediaItemRepository.Update(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			var seasonDto = _mapper.Map<SeasonDto>(season);
			return new BaseResult<SeasonDto> { Data = seasonDto };
		}

		public async Task<BaseResult<SeasonDto>> UpdateSeasonAsync(UpdateSeasonDto dto)
		{
			var season = await _seasonRepository.GetAll().FirstOrDefaultAsync(s => s.Id == dto.Id);
			if (season == null)
			{
				return new BaseResult<SeasonDto>
				{
					ErrorMessage = ErrorMessage.SeasonNotFound,
					ErrorCode = (int)ErrorCodes.SeasonNotFound
				};
			}

			season.Name = dto.Name;
			_seasonRepository.Update(season);
			await _seasonRepository.SaveChangesAsync();

			return new BaseResult<SeasonDto> { Data = _mapper.Map<SeasonDto>(season) };
		}

		public async Task<BaseResult> DeleteSeasonAsync(Guid seasonId)
		{
			var season = await _seasonRepository.GetAll().FirstOrDefaultAsync(s => s.Id == seasonId);
			if (season == null)
			{
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.SeasonNotFound,
					ErrorCode = (int)ErrorCodes.SeasonNotFound
				};
			}

			_seasonRepository.Remove(season);
			await _seasonRepository.SaveChangesAsync();

			return new BaseResult<SeasonDto> { Data = _mapper.Map<SeasonDto>(season) };
		}

		public async Task<BaseResult<SeasonDto>> GetSeasonByIdAsync(Guid seasonId)
		{
			var season = await _seasonRepository.GetAll().FirstOrDefaultAsync(s => s.Id == seasonId);
			if (season == null)
			{
				return new BaseResult<SeasonDto>
				{
					ErrorMessage = ErrorMessage.SeasonNotFound,
					ErrorCode = (int)ErrorCodes.SeasonNotFound
				};
			}

			return new BaseResult<SeasonDto> { Data = _mapper.Map<SeasonDto>(season) };
		}
	}
}
