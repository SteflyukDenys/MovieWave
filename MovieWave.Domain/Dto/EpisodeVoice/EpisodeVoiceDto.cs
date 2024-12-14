namespace MovieWave.Domain.Dto.EpisodeVoice;

public class EpisodeVoiceDto
{
	public Guid EpisodeId { get; set; }

	public long VoiceId { get; set; }

	public string VideoUrl { get; set; }
}