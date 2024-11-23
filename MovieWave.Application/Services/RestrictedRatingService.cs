using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class RestrictedRatingService : IRestrictedRatingService
	{
		private readonly IBaseRepository<RestrictedRating> _restrictedRatingRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public RestrictedRatingService(IBaseRepository<RestrictedRating> restrictedRatingRepository, ILogger logger, IMapper mapper)
		{
			_restrictedRatingRepository = restrictedRatingRepository;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<CollectionResult<RestrictedRatingDto>> GetAllAsync()
		{
			List<RestrictedRatingDto> ratings;

			ratings = await _restrictedRatingRepository.GetAll()
				.Select(r => _mapper.Map<RestrictedRatingDto>(r))
				.ToListAsync();

			if (!ratings.Any())
			{
				_logger.Warning(ErrorMessage.RestrictedRatingsNotFound);
				return new CollectionResult<RestrictedRatingDto>
				{
					ErrorMessage = ErrorMessage.RestrictedRatingsNotFound,
					ErrorCode = (int)ErrorCodes.RestrictedRatingsNotFound
				};
			}

			return new CollectionResult<RestrictedRatingDto> { Data = ratings, Count = ratings.Count };
		}

		public async Task<BaseResult<RestrictedRatingDto>> GetByIdAsync(long id)
		{
			RestrictedRatingDto? ratingDto;

			var rating = await _restrictedRatingRepository.GetAll()
				.FirstOrDefaultAsync(r => r.Id == id);

			if (rating == null)
			{
				_logger.Warning($"RestrictedRating {id} not found");
				return new BaseResult<RestrictedRatingDto>
				{
					ErrorMessage = ErrorMessage.RestrictedRatingNotFound,
					ErrorCode = (int)ErrorCodes.RestrictedRatingNotFound
				};
			}

			ratingDto = _mapper.Map<RestrictedRatingDto>(rating);

			return new BaseResult<RestrictedRatingDto> { Data = ratingDto };
		}

		public async Task<BaseResult<RestrictedRatingDto>> UpdateAsync(UpdateRestrictedRatingDto dto)
		{
			var rating = await _restrictedRatingRepository.GetAll().FirstOrDefaultAsync(r => r.Id == dto.Id);
			if (rating == null)
			{
				return new BaseResult<RestrictedRatingDto>
				{
					ErrorMessage = ErrorMessage.RestrictedRatingNotFound,
					ErrorCode = (int)ErrorCodes.RestrictedRatingNotFound
				};
			}

			_mapper.Map(dto, rating);
			var updatedRating = _restrictedRatingRepository.Update(rating);
			await _restrictedRatingRepository.SaveChangesAsync();

			return new BaseResult<RestrictedRatingDto> { Data = _mapper.Map<RestrictedRatingDto>(updatedRating) };
		}
	}
}
