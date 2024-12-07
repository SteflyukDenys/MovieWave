using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface PersonImageService
{
	Task<BaseResult<PersonImageDto>> UploadPersonImageAsync(CreatePersonImageDto dto, FileDto uploadImage);

	Task<BaseResult<PersonImageDto>> GetPersonByIdAsync(Guid personImageId);

	Task<BaseResult<PersonImageDto>> UpdatePersonImageAsync(UpdatePersonImageDto dto, FileDto newImage);

	Task<CollectionResult<PersonImageDto>> GetImageByPersonIdAsync(Guid personId);

	Task<PersonImageDto> DeletePersonImageAsync(Guid personImageId);
}