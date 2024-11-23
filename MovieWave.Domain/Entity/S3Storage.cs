namespace MovieWave.Domain.Entity;

public class S3Storage
{
	public string Name { get; set; }

	public MemoryStream InputStream { get; set; }

	public string BucketName { get; set; }
}