using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class StudioDataGenerator
{
	public static List<Studio> GenerateStudios()
	{
		return new List<Studio>
		{
			new Studio
			{
				Id = 1L,
				Name = "Warner Bros.",
				LogoPath = "https://path-to-warner-bros-logo.jpg",
				Description = "Одна з найбільших кіностудій, відома такими фільмами, як Гаррі Поттер, Діана, та ін.",
				SeoAddition = new SeoAddition
				{
					Slug = "warner-bros",
					MetaTitle = "Фільми Warner Bros.",
					Description = "Переглядайте фільми від Warner Bros. онлайн",
					MetaDescription = "Всі популярні фільми Warner Bros. в одному місці на MovieWave.",
					MetaImagePath = "https://path-to-warner-bros-image.jpg"
				}
			},
			new Studio
			{
				Id = 2L,
				Name = "Universal Pictures",
				LogoPath = "https://path-to-universal-pictures-logo.jpg",
				Description =
					"Одна з найстаріших студій Голлівуду, яка створила відомі серії фільмів, такі як Парк Юрського періоду.",
				SeoAddition = new SeoAddition
				{
					Slug = "universal-pictures",
					MetaTitle = "Фільми Universal Pictures",
					Description = "Кращі фільми від Universal Pictures онлайн на MovieWave.",
					MetaDescription =
						"Усі топ-фільми від Universal Pictures, зокрема Парк Юрського періоду та Форсаж.",
					MetaImagePath = "https://path-to-universal-image.jpg"
				}
			},
			new Studio
			{
				Id = 3L,
				Name = "Netflix",
				LogoPath = "https://path-to-netflix-logo.jpg",
				Description =
					"Один із найбільших світових стримінгових сервісів, відомий власними серіалами та фільмами.",
				SeoAddition = new SeoAddition
				{
					Slug = "netflix",
					MetaTitle = "Фільми та серіали Netflix",
					Description = "Найкращі серіали та фільми Netflix для перегляду онлайн.",
					MetaDescription =
						"Вибирайте серед серіалів та фільмів від Netflix, зокрема Відьмак, Дивні дива та ін.",
					MetaImagePath = "https://path-to-netflix-image.jpg"
				}
			},
			new Studio
			{
				Id = 4L,
				Name = "Pixar Animation Studios",
				LogoPath = "https://path-to-pixar-logo.jpg",
				Description = "Анімаційна студія, відома емоційними та технічно інноваційними мультфільмами.",
				SeoAddition = new SeoAddition
				{
					Slug = "pixar",
					MetaTitle = "Анімаційні фільми Pixar",
					Description = "Анімаційні фільми Pixar для всієї родини на MovieWave.",
					MetaDescription = "Дивіться всі анімаційні хіти від Pixar, зокрема Історія іграшок та Вперед.",
					MetaImagePath = "https://path-to-pixar-image.jpg"
				}
			},
			new Studio
			{
				Id = 5L,
				Name = "Paramount Pictures",
				LogoPath = "https://path-to-paramount-logo.jpg",
				Description = "Голлівудська кіностудія, відома численними культовими фільмами та франшизами.",
				SeoAddition = new SeoAddition
				{
					Slug = "paramount",
					MetaTitle = "Фільми Paramount Pictures",
					Description = "Всі хіти від Paramount Pictures на MovieWave.",
					MetaDescription = "Переглядайте фільми від Paramount Pictures, такі як Місія нездійсненна.",
					MetaImagePath = "https://path-to-paramount-image.jpg"
				}
			},
			new Studio
			{
				Id = 6L,
				Name = "Columbia Pictures",
				LogoPath = "https://path-to-columbia-logo.jpg",
				Description =
					"Студія Sony Pictures, відома такими фільмами, як Людина-павук, та класичними комедіями.",
				SeoAddition = new SeoAddition
				{
					Slug = "columbia",
					MetaTitle = "Фільми Columbia Pictures",
					Description = "Фільми Columbia Pictures онлайн на MovieWave.",
					MetaDescription =
						"Переглядайте популярні фільми від Columbia Pictures, зокрема Людина-павук та Джуманджі.",
					MetaImagePath = "https://path-to-columbia-image.jpg"
				}
			},
			new Studio
			{
				Id = 7L,
				Name = "Disney",
				LogoPath = "https://path-to-disney-logo.jpg",
				Description = "Світовий лідер у виробництві анімаційних та сімейних фільмів.",
				SeoAddition = new SeoAddition
				{
					Slug = "disney",
					MetaTitle = "Фільми Disney",
					Description = "Сімейні та анімаційні фільми від Disney на MovieWave.",
					MetaDescription =
						"Дивіться класичні та нові фільми Disney, включаючи Король Лев та Холодне серце.",
					MetaImagePath = "https://path-to-disney-image.jpg"
				}
			}
		};
	}
}