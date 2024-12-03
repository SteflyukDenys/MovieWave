using MovieWave.Domain.Dto.S3Storage;

namespace MovieWave.API.UploadFileRequest;

public static class FileRequest
{
    public static FileDto ConvertToFileDto(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return new FileDto
        {
            FileName = file.FileName,
            Content = new MemoryStream(memoryStream.ToArray()),
            ContentType = file.ContentType
        };
    }
}