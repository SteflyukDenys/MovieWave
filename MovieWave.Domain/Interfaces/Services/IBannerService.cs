using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services
{
	public interface IBannerService
	{
		Task<CollectionResult<BannerDto>> GetActiveBannersAsync();

		Task<BaseResult<BannerDto>> GetBannerByIdAsync(Guid bannerId);

		Task<BaseResult<BannerDto>> CreateBannerAsync(CreateBannerDto dto, FileDto imageUrl);

		Task<BaseResult<BannerDto>> UpdateBannerAsync(UpdateBannerDto dto, FileDto newImageUrl);

		Task<BaseResult> DeleteBannerAsync(Guid bannerId);
	}
}