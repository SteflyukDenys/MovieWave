using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class RestrictedRatingDataGenerator
{
	public static List<RestrictedRating> GenerateRestrictedRatings()
	{
		return new List<RestrictedRating>
		{
			new RestrictedRating
			{
				Id = 1,
				Name = "General Audience",
				Slug = "g",
				Value = 0,
				Hint = "Дозволено для всіх вікових категорій"
			},
			new RestrictedRating
			{
				Id = 2,
				Name = "Parental Guidance",
				Slug = "pg",
				Value = 10,
				Hint = "Рекомендований перегляд разом з батьками"
			},
			new RestrictedRating
			{
				Id = 3,
				Name = "Parents Strongly Cautioned",
				Slug = "pg-13",
				Value = 13,
				Hint = "Не рекомендовано для дітей до 13 років без супроводу дорослих"
			},
			new RestrictedRating
			{
				Id = 4,
				Name = "Restricted",
				Slug = "r",
				Value = 16,
				Hint = "Не рекомендовано для осіб молодших за 16 років"
			},
			new RestrictedRating
			{
				Id = 5,
				Name = "Adults Only",
				Slug = "nc-17",
				Value = 18,
				Hint = "Тільки для дорослих (18+)"
			}
		};
	}
}