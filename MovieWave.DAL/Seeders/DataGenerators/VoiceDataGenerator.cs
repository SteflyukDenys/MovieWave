using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators
{
	public static class VoiceDataGenerator
	{
		public static List<Voice> GenerateVoices(int count)
		{
			return new Faker<Voice>()
				.RuleFor(v => v.Id, f => f.Random.Guid())
				.RuleFor(v => v.Name, f => f.Name.FirstName())
				.RuleFor(v => v.Locale, f => f.Locale)
				.RuleFor(v => v.Description, f => f.Lorem.Sentence())
				.RuleFor(v => v.IconPath, f => f.Image.PicsumUrl())
				.Generate(count);
		}
	}
}
