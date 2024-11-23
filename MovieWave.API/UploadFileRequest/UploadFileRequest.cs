namespace MovieWave.API.UploadFileRequest;

public class UploadFileRequest
{
	public IFormFile File { get; set; }

	public string Folder { get; set; }
}