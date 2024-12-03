using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Result;
using MovieWave.Domain.Interfaces.Services;

public class SeoAdditionService : ISeoAdditionService
{
	private readonly IStorageService _storageService;

	public SeoAdditionService(IStorageService storageService)
	{
		_storageService = storageService;
	}

	//public async Task<BaseResult<string>> UploadSeoMetaImageAsync( fileDto)
	//{
	//	var result = await _storageService.UploadFileAsync(fileDto, "seo-images");
	//	return result.IsSuccess
	//		? new BaseResult<string> { Data = result.Data }
	//		: new BaseResult<string> { ErrorMessage = result.ErrorMessage, ErrorCode = result.ErrorCode };
	//}

	public async Task<BaseResult> DeleteSeoMetaImageAsync(string imagePath)
	{
		return await _storageService.DeleteFileAsync(imagePath);
	}
}