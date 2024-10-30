using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class VoiceDataGenerator
{
	public static List<Voice> GenerateVoices()
	{
		return new List<Voice>
		{
			new Voice
			{
				Id = 1L,
				Name = "Netflix Dubbing Studio",
				Description = "Офіційне озвучування від Netflix.",
				Locale = "en-US",
				IconPath = "https://path-to-netflix-icon.jpg"
			},
			new Voice
			{
				Id = 2L,
				Name = "Ukrainian Voice Over Studio",
				Description = "Професійне українське озвучування фільмів та серіалів.",
				Locale = "uk-UA",
				IconPath = "https://path-to-ukrainian-voice-icon.jpg"
			},
			new Voice
			{
				Id = 3,
				Name = "LostFilm",
				Description = "Популярна студія озвучування для фільмів та серіалів.",
				Locale = "ru-RU",
				IconPath = "https://path-to-lostfilm-icon.jpg"
			}
		};
	}
}