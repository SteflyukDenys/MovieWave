using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators
{
	public static class SeoAdditionDataGenerator
	{
		// Метод для генерації випадкових SEO-даних
		public static SeoAddition GenerateSeoAddition()
		{
			return new Faker<SeoAddition>()
				.RuleFor(sa => sa.Slug, f =>
				{
					var slug = f.Lorem.Slug();
					return slug.Length <= 30 ? slug : slug.Substring(0, 30);
				})
				.RuleFor(sa => sa.MetaTitle, f => f.Lorem.Sentence())
				.RuleFor(sa => sa.Description, f => f.Lorem.Paragraph())
				.RuleFor(sa => sa.MetaDescription, f => f.Lorem.Paragraph())
				.RuleFor(sa => sa.MetaImagePath, f => f.Image.PicsumUrl());
		}

		public static SeoAddition GenerateSpecificSeoAddition(string slug, string metaTitle, string description, string metaDescription, string metaImagePath)
		{
			return new SeoAddition
			{
				Slug = slug,
				MetaTitle = metaTitle,
				Description = description,
				MetaDescription = metaDescription,
				MetaImagePath = metaImagePath
			};
		}
	}
}