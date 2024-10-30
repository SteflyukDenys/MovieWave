using System.Collections.Generic;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators
{
	public static class CountryDataGenerator
	{
		public static List<Country> GenerateCountries()
		{
			return new List<Country>
			{
				new Country
				{
					Id = 1L,
					Name = "США",
					SeoAddition = new SeoAddition
					{
						Slug = "usa",
						MetaTitle = "Фільми з США",
						Description = "Кращі фільми та серіали, створені в США",
						MetaDescription = "Всі фільми, доступні онлайн з США.",
						MetaImagePath = "https://path-to-usa-image.jpg"
					}
				},
				new Country
				{
					Id = 2L,
					Name = "Велика Британія",
					SeoAddition = new SeoAddition
					{
						Slug = "uk",
						MetaTitle = "Фільми з Великої Британії",
						Description = "Кращі фільми та серіали з Великої Британії",
						MetaDescription = "Всі серіали з Великої Британії, доступні онлайн.",
						MetaImagePath = "https://path-to-uk-image.jpg"
					}
				},
				new Country
				{
					Id = 3L,
					Name = "Франція",
					SeoAddition = new SeoAddition
					{
						Slug = "france",
						MetaTitle = "Фільми з Франції",
						Description = "Популярні французькі фільми та серіали",
						MetaDescription = "Всі французькі фільми, доступні онлайн.",
						MetaImagePath = "https://path-to-france-image.jpg"
					}
				},
				new Country
				{
					Id = 4L,
					Name = "Німеччина",
					SeoAddition = new SeoAddition
					{
						Slug = "germany",
						MetaTitle = "Фільми з Німеччини",
						Description = "Вибрані фільми та серіали з Німеччини",
						MetaDescription = "Онлайн-доступ до фільмів з Німеччини.",
						MetaImagePath = "https://path-to-germany-image.jpg"
					}
				},
				new Country
				{
					Id = 5L,
					Name = "Японія",
					SeoAddition = new SeoAddition
					{
						Slug = "japan",
						MetaTitle = "Фільми з Японії",
						Description = "Кращі японські фільми та аніме",
						MetaDescription = "Популярні аніме та фільми з Японії онлайн.",
						MetaImagePath = "https://path-to-japan-image.jpg"
					}
				},
				new Country
				{
					Id = 6L,
					Name = "Україна",
					SeoAddition = new SeoAddition
					{
						Slug = "ukraine",
						MetaTitle = "Українські фільми",
						Description = "Кращі українські фільми та серіали",
						MetaDescription = "Українські популярні фільми та серіали.",
						MetaImagePath = "https://path-to-ukraine-image.jpg"
					}
				}
			};
		}
	}
}
