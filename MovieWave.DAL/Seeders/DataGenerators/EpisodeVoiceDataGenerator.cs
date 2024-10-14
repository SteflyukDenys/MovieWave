using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class EpisodeVoiceDataGenerator
{
	public static List<EpisodeVoice> GenerateEpisodeVoices(List<Episode> episodes, List<Voice> voices)
	{
		var episodeVoices = new List<EpisodeVoice>();
		var faker = new Faker();

		var availableCombinations = episodes
			.SelectMany(e => voices, (episode, voice) => new { EpisodeId = episode.Id, VoiceId = voice.Id })
			.ToList();

		foreach (var combination in availableCombinations)
		{
			if (faker.Random.Bool(0.5f))
			{
				episodeVoices.Add(new EpisodeVoice
				{
					EpisodeId = combination.EpisodeId,
					VoiceId = combination.VoiceId,
					VideoUrl = faker.Internet.Url()
				});
			}
		}

		return episodeVoices;
	}
}