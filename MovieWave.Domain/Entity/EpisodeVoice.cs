namespace MovieWave.Domain.Entity;

// For relationship Many-to-Many
public class EpisodeVoice
{
	public Guid EpisodeId { get; set; }
	public Episode Episode { get; set; }

	public Guid VoiceId { get; set; }
	public Voice Voice { get; set; }

	public string VideoUrl { get; set; }
	public TimeSpan? LastWatchedTime { get; set; }
}
