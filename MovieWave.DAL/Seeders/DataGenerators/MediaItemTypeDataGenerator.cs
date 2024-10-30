using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

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
				SeoAddition = new SeoAddition
				{
					Slug = "film",
					MetaTitle = "Фільми онлайн | Дивитися всі фільми на MovieWave",
					Description = "Всі фільми, доступні онлайн на MovieWave.",
					MetaDescription = "Переглядайте всі нові фільми онлайн на MovieWave. Обирайте серед безлічі жанрів, акторів та режисерів.",
					MetaImagePath = "https://path-to-film-category-image.jpg"
				}
			},
			new MediaItemType
			{
				Id = 2,
				MediaItemName = MediaItemName.Series,
				SeoAddition = new SeoAddition
				{
					Slug = "series",
					MetaTitle = "Серіали онлайн | Дивитися всі серіали на MovieWave",
					Description = "Всі серіали, доступні онлайн на MovieWave.",
					MetaDescription = "Дивіться нові серіали онлайн. Вибирайте серед популярних серіалів різних жанрів.",
					MetaImagePath = "https://path-to-series-category-image.jpg"
				}
			}
		};
	}
}