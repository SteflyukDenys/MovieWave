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
                Name = "Комедії",
                Description = "Фільми для гарного настрою з веселими сюжетами та героями.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "comedies",
                    MetaTitle = "Комедії онлайн | Дивитися комедійні фільми на MovieWave",
                    Description = "Насолоджуйтесь комедійними фільмами та серіалами на MovieWave.",
                    MetaDescription = "Переглядайте найкращі комедії онлайн. Зібрання комедій для гарного настрою.",
                    MetaImagePath = "tags/comedies.png"
				}
            },
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Бойовики",
                Description = "Фільми з динамічними сценами та захопливими сюжетами.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "action-movies",
                    MetaTitle = "Бойовики онлайн | Дивитися фільми бойовики на MovieWave",
                    Description = "Захопливі бойовики та екшен фільми онлайн.",
                    MetaDescription = "Найкращі бойовики з улюбленими героями та динамічними сценами.",
                    MetaImagePath = "tags/action-movies.png"
				}
            },
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Детективи",
                Description = "Захопливі історії з інтригуючими розслідуваннями.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "detective-movies",
                    MetaTitle = "Детективи онлайн | Дивитися детективні фільми на MovieWave",
                    Description = "Переглядайте найкращі детективні фільми онлайн.",
                    MetaDescription = "Захопливі розслідування та загадкові сюжети для справжніх любителів детективів.",
                    MetaImagePath = "tags/detective-movies.png"
				}
            },
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Мелодрами",
                Description = "Романтичні фільми з емоційними сюжетами.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "melodramas",
                    MetaTitle = "Мелодрами онлайн | Дивитися романтичні фільми на MovieWave",
                    Description = "Романтичні мелодрами для поціновувачів чуттєвих історій.",
                    MetaDescription = "Найкращі мелодрами з глибокими сюжетами та емоціями.",
                    MetaImagePath = "tags/melodramas.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Трилери",
                Description = "Фільми з напруженими сюжетами, які тримають у напрузі до кінця.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "thrillers",
                    MetaTitle = "Трилери онлайн | Дивитися захопливі фільми на MovieWave",
                    Description = "Напружені трилери, що тримають глядачів у напрузі.",
                    MetaDescription = "Найкращі трилери для любителів адреналінових сюжетів.",
                    MetaImagePath = "tags/thrillers.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Ужаси",
                Description = "Фільми для тих, хто любить полоскотати нерви.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "horror",
                    MetaTitle = "Фільми жахів онлайн | Дивитися страшні фільми на MovieWave",
                    Description = "Найкращі ужаси для справжніх фанатів адреналіну.",
                    MetaDescription = "Страшні фільми з жахливими сюжетами та непередбачуваними моментами.",
                    MetaImagePath = "tags/horror.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Мюзикли",
                Description = "Фільми з яскравими музичними номерами та історіями.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "musicals",
                    MetaTitle = "Мюзикли онлайн | Дивитися музичні фільми на MovieWave",
                    Description = "Захопливі мюзикли з яскравими виступами та піснями.",
                    MetaDescription = "Дивіться музичні фільми онлайн для справжніх поціновувачів музики.",
                    MetaImagePath = "tags/musicals.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Пригоди",
                Description = "Захопливі фільми про подорожі та незвичайні пригоди.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "adventures",
                    MetaTitle = "Пригоди онлайн | Дивитися пригодницькі фільми на MovieWave",
                    Description = "Найкращі пригодницькі фільми для справжніх любителів подорожей.",
                    MetaDescription = "Фільми, які розкривають захопливі пригоди та незабутні події.",
                    MetaImagePath = "tags/adventures.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Спортивні",
                Description = "Фільми про спорт, досягнення та перемоги.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "sports",
                    MetaTitle = "Спортивні фільми | Дивитися фільми про спорт на MovieWave",
                    Description = "Захопливі спортивні історії та досягнення на кіноекрані.",
                    MetaDescription = "Дивіться спортивні фільми для натхнення та мотивації.",
                    MetaImagePath = "tags/sports.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Фантастика",
                Description = "Фільми з незвичайними світами та фантастичними сюжетами.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "sci-fi",
                    MetaTitle = "Фантастика онлайн | Дивитися фантастичні фільми на MovieWave",
                    Description = "Незвичайні світи, фантастичні сюжети та події.",
                    MetaDescription = "Дивіться найкращу фантастику онлайн та поринайте у незвичайні світи.",
                    MetaImagePath = "tags/sci-fi.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Кримінал",
                Description = "Фільми з кримінальними сюжетами та розслідуваннями.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "crime",
                    MetaTitle = "Кримінал онлайн | Дивитися кримінальні фільми на MovieWave",
                    Description = "Кримінальні драми та розслідування для справжніх любителів жанру.",
                    MetaDescription = "Найкращі кримінальні фільми з захопливими сюжетами.",
                    MetaImagePath = "tags/crime.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Драми",
                Description = "Емоційні фільми, що розповідають глибокі історії.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "dramas",
                    MetaTitle = "Драми онлайн | Дивитися драматичні фільми на MovieWave",
                    Description = "Переглядайте драматичні фільми онлайн на MovieWave.",
                    MetaDescription = "Колекція драм для поціновувачів глибоких історій та емоцій.",
                    MetaImagePath = "tags/dramas.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Короткометражні",
                Description = "Фільми, що передають історії за короткий час.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "short-films",
                    MetaTitle = "Короткометражні фільми | Дивитися короткі історії на MovieWave",
                    Description = "Фільми, які за короткий час відкривають великий зміст.",
                    MetaDescription = "Дивіться короткометражні фільми з великими ідеями.",
                    MetaImagePath = "tags/short-films.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Біографія",
                Description = "Фільми про життя видатних людей.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "biography",
                    MetaTitle = "Біографічні фільми | Дивитися на MovieWave",
                    Description = "Історії про видатних людей та їх досягнення.",
                    MetaDescription = "Переглядайте біографії, які надихають та розповідають історії успіху.",
                    MetaImagePath = "tags/biography.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Воєнні",
                Description = "Фільми про війни, героїзм та боротьбу за свободу.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "war-movies",
                    MetaTitle = "Воєнні фільми | Дивитися фільми про війну на MovieWave",
                    Description = "Фільми про героїчні битви та подвиги.",
                    MetaDescription = "Переглядайте найкращі воєнні фільми, що розповідають історії героїзму.",
                    MetaImagePath = "tags/war-movies.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Історичні",
                Description = "Фільми, що розповідають про значущі події минулого.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "historical",
                    MetaTitle = "Історичні фільми | Дивитися історичні події на MovieWave",
                    Description = "Подорож у минуле через найкращі історичні фільми.",
                    MetaDescription = "Дивіться історичні фільми та дізнавайтесь більше про події минулого.",
                    MetaImagePath = "tags/historical.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Документальні",
                Description = "Фільми, що відкривають правдиві історії та факти.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "documentary",
                    MetaTitle = "Документальні фільми | Дивитися пізнавальні історії на MovieWave",
                    Description = "Відкривайте правдиві історії через документальні фільми.",
                    MetaDescription = "Дивіться документальні фільми для розширення своїх знань.",
                    MetaImagePath = "tags/documentary.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "18+",
                Description = "Фільми лише для дорослої аудиторії.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "adult",
                    MetaTitle = "Фільми 18+ | Дивитися лише для дорослих",
                    Description = "Кіно для дорослої аудиторії.",
                    MetaDescription = "Переглядайте контент лише для глядачів старших 18 років.",
                    MetaImagePath = "tags/adult.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Сімейні",
                Description = "Фільми для перегляду всією родиною.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "family-movies",
                    MetaTitle = "Сімейні фільми | Дивитися для всієї родини",
                    Description = "Фільми, які підходять для перегляду в колі сім'ї.",
                    MetaDescription = "Дивіться фільми для всієї родини на MovieWave.",
                    MetaImagePath = "tags/family-movies.png"
				}
			},
            new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Фентезі",
                Description = "Фільми з чарівними світами та казковими персонажами.",
                IsGenre = true,
                SeoAddition = new SeoAddition
                {
                    Slug = "fantasy",
                    MetaTitle = "Фентезі онлайн | Дивитися фантастичні історії на MovieWave",
                    Description = "Чарівні світи та казкові пригоди у жанрі фентезі.",
                    MetaDescription = "Найкращі фентезі фільми для любителів чарівних пригод.",
                    MetaImagePath = "tags/fantasy.png"
				}
			},

			// Tags
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Прем'єри",
				Description = "Фільми та серіали, що вийшли на екран зовсім недавно.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "new-releases",
					MetaTitle = "Новинки кіно | Дивитися останні новинки на MovieWave",
					Description = "Останні новинки кіно та серіалів на MovieWave.",
					MetaDescription = "Дивіться новинки кіно онлайн. Найсвіжіші прем'єри та нові фільми.",
                    MetaImagePath = "tags/new-releases.png"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Топ 10",
				Description = "Фільми та серіали, що входять в топ 10.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "top-10",
					MetaTitle = "Новинки кіно | Дивитися кращі фільми на MovieWave",
					Description = "Кращі кіно та серіали на MovieWave.",
					MetaDescription = "Дивіться краші фільми онлайн. Найсвіжіші прем'єри та нові фільми.",
                    MetaImagePath = "tags/top-10.png"
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
                    MetaImagePath = "tags/popular.png"
				}
			},
			new Tag
			{
				Id = Guid.NewGuid(),
				Name = "Українське кіно",
				Description = "Найкращі фільми створені в Україні.",
				IsGenre = false,
				SeoAddition = new SeoAddition
				{
					Slug = "ua-movies",
					MetaTitle = "Українські фільми | Дивитися українські фільми на MovieWave",
					Description = "Українські фільми, що підійдуть для українців.",
					MetaDescription = "Дивіться українські фільми разом із близькими на MovieWave.",
                    MetaImagePath = "tags/ua-movies.png"
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