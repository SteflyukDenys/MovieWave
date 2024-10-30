using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class TagDataGenerator
{
	public static List<Tag> GenerateTags()
	{
		return new List<Tag>
		{
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Комедія",
				Description = "Фільми для гарного настрою з веселими сюжетами та героями.",
				IsGenre = true,
				SeoAddition = new SeoAddition
				{
					Slug = "comedy",
					MetaTitle = "Комедії онлайн | Дивитися комедійні фільми на MovieWave",
					Description = "Насолоджуйтесь комедійними фільмами та серіалами на MovieWave.",
					MetaDescription = "Переглядайте найкращі комедії онлайн. Зібрання комедій для гарного настрою.",
					MetaImagePath = "https://path-to-comedy-image.jpg"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Бойовик",
				Description = "Фільми з динамічними сценами та захопливими сюжетами.",
				IsGenre = true,
				SeoAddition = new SeoAddition
				{
					Slug = "action",
					MetaTitle = "Бойовики онлайн | Дивитися фільми бойовики на MovieWave",
					Description = "Захопливі бойовики та екшен фільми онлайн.",
					MetaDescription = "Найкращі бойовики з улюбленими героями та динамічними сценами.",
					MetaImagePath = "https://path-to-action-image.jpg"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Драма",
				Description = "Емоційні фільми, що розповідають глибокі історії.",
				IsGenre = true,
				SeoAddition = new SeoAddition
				{
					Slug = "drama",
					MetaTitle = "Драми онлайн | Дивитися драматичні фільми на MovieWave",
					Description = "Переглядайте драматичні фільми онлайн на MovieWave.",
					MetaDescription = "Колекція драм для поціновувачів глибоких історій та емоцій.",
					MetaImagePath = "https://path-to-drama-image.jpg"
				}
			},

			// Tags
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Новинки",
				Description = "Фільми та серіали, що вийшли на екран зовсім недавно.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "new-releases",
					MetaTitle = "Новинки кіно | Дивитися останні новинки на MovieWave",
					Description = "Останні новинки кіно та серіалів на MovieWave.",
					MetaDescription = "Дивіться новинки кіно онлайн. Найсвіжіші прем'єри та нові фільми.",
					MetaImagePath = "https://path-to-new-releases-image.jpg"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Популярне",
				Description = "Фільми та серіали, які завоювали любов глядачів.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "popular",
					MetaTitle = "Популярні фільми та серіали на MovieWave",
					Description = "Найпопулярніші фільми та серіали, що підкорили серця глядачів.",
					MetaDescription = "Переглядайте популярні фільми та серіали на MovieWave.",
					MetaImagePath = "https://path-to-popular-image.jpg"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Сімейне",
				Description = "Фільми для перегляду всією родиною.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "family",
					MetaTitle = "Сімейні фільми | Дивитися фільми для всієї родини",
					Description = "Сімейні фільми, що підійдуть для глядачів будь-якого віку.",
					MetaDescription = "Дивіться сімейні фільми разом із близькими на MovieWave.",
					MetaImagePath = "https://path-to-family-image.jpg"
				}
			}
			//new Tag
			//{
			//	Id = Guid.NewGuid(),
			//	Name = "Романтична комедія",
			//	Description = "Комедійні фільми та серіали з романтичними сюжетами.",
			//	IsGenre = true,
			//	ParentId = /* ID of "Комедія" */,
			//	SeoAddition = new SeoAddition
			//	{
			//		Slug = "romantic-comedy",
			//		MetaTitle = "Романтична комедія | Дивитися романтичні комедії",
			//		Description = "Романтичні комедії для любителів гумору та романтики.",
			//		MetaDescription = "Дивіться романтичні комедії на MovieWave.",
			//		MetaImagePath = "https://path-to-romantic-comedy-image.jpg"
			//	}
			//}
		};
	}
}