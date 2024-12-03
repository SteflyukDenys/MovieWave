using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IStorageService
{
	Task<BaseResult<string>> UploadFileAsync(FileDto fileDto, string folder);

	Task<BaseResult> DeleteFileAsync(string key);

	Task<BaseResult<Stream>> GetFileAsync(string key);

	Task<CollectionResult<S3ObjectDto>> ListFilesAsync(string? prefix = null);

	string GenerateFileUrl(string key);
}
