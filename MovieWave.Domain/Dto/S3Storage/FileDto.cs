namespace MovieWave.Domain.Dto.S3Storage;

public class FileDto
{
	public string FileName { get; set; } = string.Empty;

	public Stream Content { get; set; } = null!;

	public string ContentType { get; set; } = string.Empty;
}