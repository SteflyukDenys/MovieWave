using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.Application.Services;

public class StorageService : IStorageService
{
	private readonly IAmazonS3 _s3Client;
	private readonly string _bucketName;

	public StorageService(IAmazonS3 s3Client, IConfiguration configuration)
	{
		_s3Client = s3Client;
		_bucketName = configuration["AWS:BucketName"];
	}

	public async Task<BaseResult<string>> UploadFileAsync(FileDto fileDto, string folder)
	{
		var key = $"{folder}/{fileDto.FileName}";

		var request = new PutObjectRequest
		{
			BucketName = _bucketName,
			Key = key,
			InputStream = fileDto.Content,
			ContentType = fileDto.ContentType
		};

		await _s3Client.PutObjectAsync(request);

		return new BaseResult<string> { Data = key };
	}

	public async Task<BaseResult> DeleteFileAsync(string key)
	{

		var request = new DeleteObjectRequest
		{
			BucketName = _bucketName,
			Key = key
		};

		await _s3Client.DeleteObjectAsync(request);

		return new BaseResult();

	}

	public async Task<BaseResult<Stream>> GetFileAsync(string key)
	{

		var request = new GetObjectRequest
		{
			BucketName = _bucketName,
			Key = key
		};

		var response = await _s3Client.GetObjectAsync(request);

		return new BaseResult<Stream> { Data = response.ResponseStream };

	}

	public async Task<CollectionResult<S3ObjectDto>> ListFilesAsync(string? prefix = null)
	{
		var request = new ListObjectsV2Request
		{
			BucketName = _bucketName,
			Prefix = prefix
		};

		var response = await _s3Client.ListObjectsV2Async(request);

		var files = response.S3Objects.Select(o => new S3ObjectDto
		{
			Key = o.Key,
			LastModified = o.LastModified,
			Size = o.Size,
			Url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
			{
				BucketName = _bucketName,
				Key = o.Key,
				Expires = DateTime.UtcNow.AddMinutes(5)
			})
		}).ToList();

		return new CollectionResult<S3ObjectDto> { Data = files, Count = files.Count };
	}
};
