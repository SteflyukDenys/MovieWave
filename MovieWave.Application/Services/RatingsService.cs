using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class RatingsService : IRatingsService
	{
		private readonly IBaseRepository<Review> _reviewRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public RatingsService(
			IBaseRepository<Review> reviewRepository,
			IBaseRepository<MediaItem> mediaItemRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			ILogger logger)
		{
			_reviewRepository = reviewRepository;
			_mediaItemRepository = mediaItemRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<BaseResult<bool>> RateMediaItemAsync(Guid userId, RateMediaItemDto dto)
		{
			_logger.Information("User {UserId} оцінює MediaItem {MediaItemId} з Rating={Rating}", userId, dto.MediaItemId, dto.Rating);

			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(mi => mi.Id == dto.MediaItemId);
			if (mediaItem == null)
			{
				_logger.Warning("MediaItem з ID={MediaItemId} не знайдено", dto.MediaItemId);
				return new BaseResult<bool>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			var existingReview = await _reviewRepository.GetAll()
				.FirstOrDefaultAsync(r => r.MediaItemId == dto.MediaItemId && r.UserId == userId);

			if (existingReview != null)
			{
				_logger.Information("Знайдено існуючий Review для MediaItem {MediaItemId} та User {UserId}. Оновлюємо Rating.", dto.MediaItemId, userId);
				existingReview.Rating = dto.Rating;
				existingReview.UpdatedAt = DateTime.UtcNow;
				_reviewRepository.Update(existingReview);
			}
			else
			{
				_logger.Information("Створюємо новий Review для MediaItem {MediaItemId} та User {UserId}.", dto.MediaItemId, userId);
				var review = new Review
				{
					MediaItemId = dto.MediaItemId,
					UserId = userId,
					Rating = dto.Rating,
					Text = null
				};
				await _reviewRepository.CreateAsync(review);
			}

			await _unitOfWork.SaveChangesAsync();
			_logger.Information("Rating успішно збережено для MediaItem {MediaItemId} та User {UserId}", dto.MediaItemId, userId);
			return new BaseResult<bool>
			{
				Data = true
			};
		}


		public async Task<BaseResult<double>> GetAverageRatingAsync(Guid mediaItemId)
		{
			_logger.Information("Отримання середньої оцінки для MediaItem {MediaItemId}", mediaItemId);

			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(mi => mi.Id == mediaItemId);
			if (mediaItem == null)
			{
				_logger.Warning("MediaItem з ID={MediaItemId} не знайдено", mediaItemId);
				return new BaseResult<double>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			var reviews = _reviewRepository.GetAll().Where(r => r.MediaItemId == mediaItemId);
			var reviewCount = await reviews.CountAsync();
			if (reviewCount == 0)
			{
				_logger.Information("MediaItem {MediaItemId} не має оцінок. Середня оцінка буде 0.0", mediaItemId);
				return new BaseResult<double>
				{
					Data = 0.0
				};
			}

			var averageRating = await reviews.AverageAsync(r => (double?)r.Rating) ?? 0.0;
			_logger.Information("Середня оцінка для MediaItem {MediaItemId} = {AverageRating}", mediaItemId, averageRating);

			return new BaseResult<double>
			{
				Data = averageRating
			};
		}

	}
}
