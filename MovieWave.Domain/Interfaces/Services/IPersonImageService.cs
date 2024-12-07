using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IPersonImageService
{
	Task<BaseResult<PersonImageDto>> UploadPersonImageAsync(CreatePersonImageDto dto, FileDto uploadImage);

	Task<BaseResult<PersonImageDto>> GetPersonImageByIdAsync(Guid personImageId);

	Task<CollectionResult<PersonImageDto>> GetImagesByPersonIdAsync(Guid personId);

	Task<BaseResult<PersonImageDto>> UpdatePersonImageAsync(UpdatePersonImageDto dto, FileDto newImage);

	Task<BaseResult> DeletePersonImageAsync(Guid personImageId);
}