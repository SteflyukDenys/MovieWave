using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class MediaItemTypeService : IMediaItemTypeService
	{
		private readonly IBaseRepository<MediaItemType> _mediaItemTypeRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public MediaItemTypeService(IBaseRepository<MediaItemType> mediaItemTypeRepository, IMapper mapper, ILogger logger)
		{
			_mediaItemTypeRepository = mediaItemTypeRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<CollectionResult<MediaItemTypeDto>> GetAllAsync()
		{
			List<MediaItemTypeDto> mediaItemTypes;

			var entities = await _mediaItemTypeRepository.GetAll().ToListAsync();

			mediaItemTypes = entities.Select(entity => _mapper.Map<MediaItemTypeDto>(entity)).ToList();

			if (!mediaItemTypes.Any())
			{
				_logger.Warning("No media item types found.");
				return new CollectionResult<MediaItemTypeDto>()
				{
					ErrorMessage = ErrorMessage.MediaItemTypesNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemTypesNotFound
				};
			}

			return new CollectionResult<MediaItemTypeDto>()
			{
				Data = mediaItemTypes,
				Count = mediaItemTypes.Count
			};
		}

		public async Task<BaseResult<MediaItemTypeDto>> GetByIdAsync(int id)
		{
			MediaItemTypeDto? mediaItemType;

			var entity = await _mediaItemTypeRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

			if (entity == null)
			{
				_logger.Warning($"MediaItemType with ID {id} not found.");
				return new BaseResult<MediaItemTypeDto>()
				{
					ErrorMessage = ErrorMessage.MediaItemTypeNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemTypeNotFound
				};
			}

			mediaItemType = _mapper.Map<MediaItemTypeDto>(entity);

			return new BaseResult<MediaItemTypeDto>()
			{
				Data = mediaItemType
			};
		}
	}
}
