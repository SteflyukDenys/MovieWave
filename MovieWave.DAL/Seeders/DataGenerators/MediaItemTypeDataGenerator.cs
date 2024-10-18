using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators
{
	public static class MediaItemTypeDataGenerator
	{
		public static List<MediaItemType> GenerateMediaItemTypes()
		{
			return new List<MediaItemType>
			{
				new MediaItemType
				{
					Id = 1,
					MediaItemName = MediaItemName.Film,
					SeoAddition = SeoAdditionDataGenerator.GenerateSpecificSeoAddition(
						"film",
						"Фільми онлайн | Дивитися всі фільми на MovieWave",
						"Всі фільми, доступні онлайн на MovieWave.",
						"Переглядайте всі нові фільми онлайн на MovieWave. Обирайте серед безлічі жанрів, акторів та режисерів.",
						"https://path-to-film-category-image.jpg"
					)
				},
				new MediaItemType
				{
					Id = 2,
					MediaItemName = MediaItemName.Series,
					SeoAddition = SeoAdditionDataGenerator.GenerateSpecificSeoAddition(
						"series",
						"Серіали онлайн | Дивитися всі серіали на MovieWave",
						"Всі серіали, доступні онлайн на MovieWave.",
						"Дивіться нові серіали онлайн. Вибирайте серед популярних серіалів різних жанрів.",
						"https://path-to-series-category-image.jpg"
					)
				}
			};
		}
	}
}