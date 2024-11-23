namespace MovieWave.Domain.Dto.S3Storage;

public class S3ObjectDto
{
	public string Key { get; set; }

	public string Url { get; set; }

	public DateTime LastModified { get; set; }

	public long Size { get; set; }
}