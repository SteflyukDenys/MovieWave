using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class BannerService : IBannerService
	{
		private readonly IBaseRepository<Banner> _bannerRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStorageService _storageService;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public BannerService(
			ILogger logger,
			IMapper mapper,
			IBaseRepository<Banner> bannerRepository,
			IStorageService storageService, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_mapper = mapper;
			_bannerRepository = bannerRepository;
			_storageService = storageService;
			_unitOfWork = unitOfWork;
		}

		public async Task<CollectionResult<BannerDto>> GetActiveBannersAsync()
		{
			var currentDate = DateTime.UtcNow;
			var banners = await _bannerRepository.GetAll()
				.Where(b => (!b.StartDate.HasValue || b.StartDate <= currentDate) &&
							(!b.EndDate.HasValue || b.EndDate >= currentDate))
				.OrderBy(b => b.DisplayOrder)
				.ToListAsync();

			if (!banners.Any())
			{
				_logger.Warning(ErrorMessage.BannersNotFound);
				return new CollectionResult<BannerDto>
				{
					ErrorMessage = ErrorMessage.BannersNotFound,
					ErrorCode = (int)ErrorCodes.BannersNotFound
				};
			}

			var bannerDtos = _mapper.Map<List<BannerDto>>(banners);

			foreach (var bannerDto in bannerDtos)
			{
				bannerDto.ImageUrl = _storageService.GenerateFileUrl(bannerDto.ImageUrl);
			}

			return new CollectionResult<BannerDto>
			{
				Data = bannerDtos,
				Count = bannerDtos.Count
			};
		}

		public async Task<BaseResult<BannerDto>> GetBannerByIdAsync(Guid bannerId)
		{
			var banner = await _bannerRepository.GetAll()
				.FirstOrDefaultAsync(b => b.Id == bannerId);

			if (banner == null)
			{
				_logger.Warning("Банер з ID {BannerId} не знайдено.", bannerId);
				return new BaseResult<BannerDto>
				{
					ErrorMessage = ErrorMessage.BannerNotFound,
					ErrorCode = (int)ErrorCodes.BannerNotFound
				};
			}

			var bannerDto = _mapper.Map<BannerDto>(banner);
			bannerDto.ImageUrl = _storageService.GenerateFileUrl(banner.ImageUrl);

			return new BaseResult<BannerDto> { Data = bannerDto };
		}

		public async Task<BaseResult<BannerDto>> CreateBannerAsync(CreateBannerDto dto, FileDto imageUrl)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				if (imageUrl == null || imageUrl.Content.Length == 0)
				{
					return new BaseResult<BannerDto>
					{
						ErrorMessage = ErrorMessage.InvalidFile,
						ErrorCode = 400
					};
				}

				var banner = _mapper.Map<Banner>(dto);
				// For S3
				var folder = $"banners";

				var uploadBannerImage = await _storageService.UploadFileAsync(imageUrl, folder);

				if (!uploadBannerImage.IsSuccess)
				{
					return new BaseResult<BannerDto>
					{
						ErrorMessage = uploadBannerImage.ErrorMessage,
						ErrorCode = uploadBannerImage.ErrorCode
					};
				}

				banner.ImageUrl = uploadBannerImage.Data;

				await _bannerRepository.CreateAsync(banner);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				var resultDto = _mapper.Map<BannerDto>(banner);

				resultDto.ImageUrl = _storageService.GenerateFileUrl(resultDto.ImageUrl);

				return new BaseResult<BannerDto> { Data = resultDto };
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error(ex, "Помилка при створенні Banner: {Message}", ex.Message);

				return new BaseResult<BannerDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}

		public async Task<BaseResult<BannerDto>> UpdateBannerAsync(UpdateBannerDto dto, FileDto newImageUrl)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				var banner = await _bannerRepository.GetAll()
					.FirstOrDefaultAsync(x => x.Id == dto.Id);

				if (banner == null)
				{
					return new BaseResult<BannerDto>()
					{
						ErrorMessage = ErrorMessage.BannerNotFound,
						ErrorCode = (int)ErrorCodes.BannerNotFound
					};
				}

				_mapper.Map(dto, banner);

				if (newImageUrl != null)
				{
					if (!string.IsNullOrEmpty(banner.ImageUrl))
					{
						var deleteResult = await _storageService.DeleteFileAsync(banner.ImageUrl);
						if (!deleteResult.IsSuccess)
						{
							_logger.Warning("Не вдалося видалити старий файл: {ErrorMessage}", deleteResult.ErrorMessage);
						}
					}

					var folder = $"banners";

					var uploadBannerImage = await _storageService.UploadFileAsync(newImageUrl, folder);
					if (!uploadBannerImage.IsSuccess)
					{
						return new BaseResult<BannerDto>
						{
							ErrorMessage = uploadBannerImage.ErrorMessage,
							ErrorCode = uploadBannerImage.ErrorCode
						};
					}

					banner.ImageUrl = uploadBannerImage.Data;
				}


				_bannerRepository.Update(banner);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				var resultDto = _mapper.Map<BannerDto>(banner);

				if (!string.IsNullOrEmpty(resultDto.ImageUrl))
				{
					resultDto.ImageUrl = _storageService.GenerateFileUrl(resultDto.ImageUrl);
				}

				return new BaseResult<BannerDto> { Data = resultDto };
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error("Помилка при оновленні Banner: {Message}", ex.Message);
				return new BaseResult<BannerDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}

		public async Task<BaseResult> DeleteBannerAsync(Guid bannerId)
		{
			var banner = await _bannerRepository.GetAll()
				.FirstOrDefaultAsync(b => b.Id == bannerId);

			if (banner == null)
			{
				_logger.Warning("Банер з ID {BannerId} не знайдено.", bannerId);
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.BannerNotFound,
					ErrorCode = (int)ErrorCodes.BannerNotFound
				};
			}

			var deleteResult = await _storageService.DeleteFileAsync(banner.ImageUrl);
			if (!deleteResult.IsSuccess)
			{
				_logger.Error("Не вдалося видалити файл зі сховища: {ErrorMessage}", deleteResult.ErrorMessage);
			}

			_bannerRepository.Remove(banner);
			await _bannerRepository.SaveChangesAsync();

			return new BaseResult();
		}
	}
}
