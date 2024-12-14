using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using MovieCredits = TMDbLib.Objects.Movies.Credits;
using TvCredits = TMDbLib.Objects.TvShows.Credits;

namespace MovieWave.Application.Services
{
	public class TmdbMovieImportService : ITmdbMovieImportService
	{
		private readonly IMediaItemService _mediaItemService;
		private readonly ITagService _tagService;
		private readonly ICountryService _countryService;
		private readonly IStudioService _studioService;
		private readonly IPersonService _personService;
		private readonly IAttachmentService _attachmentService;
		private readonly ISeasonService _seasonService;
		private readonly IEpisodeService _episodeService;
		private readonly IPersonImageService _personImageService;

		private TMDbClient _client;

		public TmdbMovieImportService(
			IMediaItemService mediaItemService,
			ITagService tagService,
			ICountryService countryService,
			IStudioService studioService,
			IPersonService personService,
			IAttachmentService attachmentService,
			ISeasonService seasonService,
			IEpisodeService episodeService,
			IPersonImageService personImageService
		)
		{
			_mediaItemService = mediaItemService;
			_tagService = tagService;
			_countryService = countryService;
			_studioService = studioService;
			_personService = personService;
			_attachmentService = attachmentService;
			_seasonService = seasonService;
			_episodeService = episodeService;
			_personImageService = personImageService;

			_client = new TMDbClient("");
			_client.DefaultLanguage = "uk-UA";
		}

		public async Task ImportDataAsync()
		{
			await ImportMoviesAsync(300);
			await ImportSeriesAsync(50);
		}

		private async Task ImportMoviesAsync(int count)
		{
			int itemsPerPage = 20;
			int pagesNeeded = (int)Math.Ceiling(count / (double)itemsPerPage);

			var allMovies = new List<SearchMovie>();
			for (int i = 1; i <= pagesNeeded; i++)
			{
				var pageResult = await _client.GetMoviePopularListAsync(page: i);
				allMovies.AddRange(pageResult.Results);
			}

			var topMovies = allMovies.Take(count).ToList();

			foreach (var movieShort in topMovies)
			{
				var movieDetails = await _client.GetMovieAsync(
					movieShort.Id,
					MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Images | MovieMethods.ReleaseDates
				);

				if (movieDetails == null)
				{
					Console.WriteLine($"No details for movie ID={movieShort.Id}");
					continue;
				}

				long restrictedRatingId = await GetRestrictedRatingForMovie(movieShort.Id, movieDetails);
				int statusId = MapStatus(movieDetails.Status);
				int mediaItemTypeId = 1; // Film
				DateTime? releaseDate = movieDetails.ReleaseDate;

				var genreNames = movieDetails.Genres.Select(g => g.Name).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
				var tagIds = await _tagService.GetTagsByNamesAsync(genreNames);

				var countryNames = movieDetails.ProductionCountries
					.Select(c => c.Name)
					.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList();
				var countryIds = await _countryService.GetCountriesByNamesAsync(countryNames);

				var studioNames = movieDetails.ProductionCompanies
					.Select(pc => pc.Name)
					.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList();
				var studioIds = await _studioService.GetStudioByNamesAsync(studioNames);

				// Person
				var directors = movieDetails.Credits.Crew.Where(c => c.Job == "Director").ToList();
				var actors = movieDetails.Credits.Cast.Take(5).ToList();
				var allPersonNames = directors.Select(d => d.Name).Concat(actors.Select(a => a.Name)).Distinct().ToList();
				var personMap = await _personService.GetOrCreatePersonsByNamesAsync(allPersonNames); // Повертає мапу

				var personRoles = new List<PersonRoleDto>();
				foreach (var dir in directors)
				{
					if (personMap.TryGetValue(dir.Name, out var pId))
					{
						personRoles.Add(new PersonRoleDto()
						{
							PersonId = pId,
							Role = (int)PersonRole.Director
						});
					}
				}
				foreach (var act in actors)
				{
					if (personMap.TryGetValue(act.Name, out var pId))
					{
						personRoles.Add(new PersonRoleDto()
						{
							PersonId = pId,
							Role = (int)PersonRole.Actor
						});
					}
				}
				// Перевірка дублікату
				var nameYear = movieDetails.Title + (releaseDate?.Year.ToString() ?? "");
				if (await IsMediaItemDuplicateAsync(nameYear))
				{
					Console.WriteLine($"Media '{movieDetails.Title}' duplicate. Skipped.");
					continue;
				}

				var baseSlugTitle = movieDetails.OriginalTitle;
				if (string.IsNullOrEmpty(baseSlugTitle))
					baseSlugTitle = movieDetails.Title ?? "no-title";

				var slug = GenerateSlug(baseSlugTitle);
				slug = await MakeSlugUniqueAsync(slug);

				var createDto = new CreateMediaItemDto
				{
					Name = movieDetails.Title,
					OriginalName = movieDetails.OriginalTitle,
					Description = movieDetails.Overview,
					Duration = movieDetails.Runtime ?? 0,
					ImdbScore = movieDetails.VoteAverage,
					MediaItemTypeId = mediaItemTypeId,
					StatusId = statusId,
					RestrictedRatingId = restrictedRatingId,
					FirstAirDate = releaseDate?.ToUniversalTime(),
					LastAirDate = releaseDate?.ToUniversalTime(),
					SeoAddition = new SeoAdditionInputDto
					{
						Slug = slug,
						MetaTitle = movieDetails.OriginalTitle,
						Description = movieDetails.Overview,
						MetaDescription = movieDetails.Overview
					},
					TagIds = tagIds,
					CountryIds = countryIds,
					StudioIds = studioIds,
					PersonRoles = personRoles
				};

				var createResult = await _mediaItemService.CreateMediaItemAsync(createDto);
				if (!createResult.IsSuccess)
				{
					Console.WriteLine($"Failed create movie: {movieDetails.Title}. Error: {createResult.ErrorMessage}");
					continue;
				}

				var mediaItemId = createResult.Data.Id;
				var attachments = new List<AttachmentDto>();

				// Poster & Backdrop
				if (!string.IsNullOrEmpty(movieDetails.PosterPath))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Thumbnail,
						AttachmentUrl = "https://image.tmdb.org/t/p/original" + movieDetails.PosterPath
					});
				}
				if (!string.IsNullOrEmpty(movieDetails.BackdropPath))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Poster,
						AttachmentUrl = "https://image.tmdb.org/t/p/w500" + movieDetails.BackdropPath
					});
				}
				if (movieDetails.Images?.Backdrops != null)
				{
					// Scenes
					foreach (var backdrop in movieDetails.Images.Backdrops)
					{
						attachments.Add(new AttachmentDto
						{
							Id = Guid.NewGuid(),
							MediaItemId = mediaItemId,
							AttachmentType = AttachmentType.Scene,
							AttachmentUrl = "https://image.tmdb.org/t/p/original" + backdrop.FilePath
						});
					}
				}

				// Trailer
				var trailerUrl = GetTrailerUrl(movieDetails.Videos);
				if (!string.IsNullOrEmpty(trailerUrl))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Trailer,
						AttachmentUrl = trailerUrl
					});
				}

				await _attachmentService.AddAttachmentsAsync(attachments);

				// Додамо зображення для персон
				//await AddPersonImagesAsync(personImageCommands);

				Console.WriteLine($"Movie '{movieDetails.Title}' added.");
			}
		}

		private async Task ImportSeriesAsync(int count)
		{
			int itemsPerPage = 20;
			int pagesNeeded = (int)Math.Ceiling(count / (double)itemsPerPage);

			var allSeries = new List<SearchTv>();
			for (int i = 1; i <= pagesNeeded; i++)
			{
				var pageResult = await _client.GetTvShowPopularAsync(i);
				allSeries.AddRange(pageResult.Results);
			}

			var topSeries = allSeries.Take(count).ToList();

			foreach (var seriesShort in topSeries)
			{
				var seriesDetails = await _client.GetTvShowAsync(
					seriesShort.Id,
					TvShowMethods.Credits | TvShowMethods.Videos | TvShowMethods.Images
				);
				if (seriesDetails == null)
				{
					Console.WriteLine($"No details for series ID={seriesShort.Id}");
					continue;
				}

				int mediaItemTypeId = 2; // Series
				int statusId = MapStatusTv(seriesDetails.Status);

				DateTime? firstAirDate = seriesDetails.FirstAirDate;
				DateTime? lastAirDate = seriesDetails.LastAirDate;

				// For series, default PG
				long restrictedRatingId = 2;

				var genreNames = seriesDetails.Genres.Select(g => g.Name).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
				var tagIds = await _tagService.GetTagsByNamesAsync(genreNames);

				var countryCodes = seriesDetails.OriginCountry?.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList() ?? new List<string>();
				var mappedCountries = countryCodes.Select(MapCountryCodeToName).ToList();
				var countryIds = await _countryService.GetCountriesByNamesAsync(mappedCountries);

				var studioNames = seriesDetails.ProductionCompanies.Select(pc => pc.Name).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList();
				var studioIds = await _studioService.GetStudioByNamesAsync(studioNames);

				var personMap = new Dictionary<string, Guid>();
				var (personRoles, personImageCommands) = await GetPersonRolesAndImagesForSeries(seriesDetails.Credits, personMap);

				int duration = (seriesDetails.EpisodeRunTime != null && seriesDetails.EpisodeRunTime.Any())
					? seriesDetails.EpisodeRunTime.First()
					: 0;

				// Персони
				var directors = seriesDetails.Credits.Crew.Where(c => c.Job == "Director").ToList();
				var actors = seriesDetails.Credits.Cast.Take(5).ToList();
				var allPersonNames = directors.Select(d => d.Name).Concat(actors.Select(a => a.Name)).Distinct().ToList();
				var personMaps = await _personService.GetOrCreatePersonsByNamesAsync(allPersonNames);

				var personsRoles = new List<PersonRoleDto>();
				foreach (var dir in directors)
				{
					if (personMaps.TryGetValue(dir.Name, out var pId))
					{
						personsRoles.Add(new PersonRoleDto()
						{
							PersonId = pId,
							Role = (int)PersonRole.Director
						});
					}
				}
				foreach (var act in actors)
				{
					if (personMaps.TryGetValue(act.Name, out var pId))
					{
						personsRoles.Add(new PersonRoleDto()
						{
							PersonId = pId,
							Role = (int)PersonRole.Actor
						});
					}
				}

				// Duplicate check
				var nameYear = seriesDetails.Name + (firstAirDate?.Year.ToString() ?? "");
				if (await IsMediaItemDuplicateAsync(nameYear))
				{
					Console.WriteLine($"Series '{seriesDetails.Name}' duplicate. Skipped.");
					continue;
				}

				var baseSlugTitle = seriesDetails.OriginalName;
				if (string.IsNullOrEmpty(baseSlugTitle))
					baseSlugTitle = seriesDetails.Name ?? "no-title";

				var slug = GenerateSlug(baseSlugTitle);
				slug = await MakeSlugUniqueAsync(slug);

				var createDto = new CreateMediaItemDto
				{
					Name = seriesDetails.Name,
					OriginalName = seriesDetails.OriginalName,
					Description = seriesDetails.Overview,
					Duration = duration,
					ImdbScore = seriesDetails.VoteAverage,
					MediaItemTypeId = mediaItemTypeId,
					StatusId = statusId,
					RestrictedRatingId = restrictedRatingId,
					FirstAirDate = firstAirDate?.ToUniversalTime(),
					LastAirDate = lastAirDate?.ToUniversalTime(),
					SeoAddition = new SeoAdditionInputDto
					{
						Slug = slug,
						MetaTitle = seriesDetails.OriginalName,
						Description = seriesDetails.Overview,
						MetaDescription = seriesDetails.Overview
					},
					TagIds = tagIds,
					CountryIds = countryIds,
					StudioIds = studioIds,
					PersonRoles = personRoles
				};

				var createResult = await _mediaItemService.CreateMediaItemAsync(createDto);
				if (!createResult.IsSuccess)
				{
					Console.WriteLine($"Failed to create series: {seriesDetails.Name}. Error: {createResult.ErrorMessage}");
					continue;
				}

				var mediaItemId = createResult.Data.Id;
				var attachments = new List<AttachmentDto>();

				// Poster, Scenes, Trailer
				if (!string.IsNullOrEmpty(seriesDetails.PosterPath))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Thumbnail,
						AttachmentUrl = "https://image.tmdb.org/t/p/original" + seriesDetails.PosterPath
					});
				}
				if (!string.IsNullOrEmpty(seriesDetails.BackdropPath))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Poster,
						AttachmentUrl = "https://image.tmdb.org/t/p/w500" + seriesDetails.BackdropPath
					});
				}
				if (seriesDetails.Images?.Backdrops != null)
				{
					foreach (var bd in seriesDetails.Images.Backdrops)
					{
						attachments.Add(new AttachmentDto
						{
							Id = Guid.NewGuid(),
							MediaItemId = mediaItemId,
							AttachmentType = AttachmentType.Scene,
							AttachmentUrl = "https://image.tmdb.org/t/p/original" + bd.FilePath
						});
					}
				}
				var trailerUrl = GetTrailerUrl(seriesDetails.Videos);
				if (!string.IsNullOrEmpty(trailerUrl))
				{
					attachments.Add(new AttachmentDto
					{
						Id = Guid.NewGuid(),
						MediaItemId = mediaItemId,
						AttachmentType = AttachmentType.Trailer,
						AttachmentUrl = trailerUrl
					});
				}

				await _attachmentService.AddAttachmentsAsync(attachments);

				// Person images
				await AddPersonImagesAsync(personImageCommands);

				// Seasons & Episodes
				if (seriesDetails.NumberOfSeasons > 0)
				{
					for (int sn = 1; sn <= seriesDetails.NumberOfSeasons; sn++)
					{
						var seasonDetails = await _client.GetTvSeasonAsync(seriesShort.Id, sn, TvSeasonMethods.Videos | TvSeasonMethods.Images);
						if (seasonDetails == null) continue;

						// Create season
						var seasonCreate = new CreateSeasonDto
						{
							MediaItemId = mediaItemId,
							Name = seasonDetails.Name ?? $"Сезон {sn}"
						};
						var seasonRes = await _seasonService.CreateSeasonAsync(seasonCreate);
						if (!seasonRes.IsSuccess) continue;
						var seasonId = seasonRes.Data.Id;

						// Episodes
						if (seasonDetails.Episodes != null)
						{
							foreach (var ep in seasonDetails.Episodes)
							{
								var epImages = ep.StillPath;
								var epSlugBase = ep.Name ?? $"episode-{ep.EpisodeNumber}";
								var epSlug = GenerateSlug(epSlugBase);
								epSlug = await MakeSlugUniqueAsync(epSlug);

								// Create episode
								var epCreate = new CreateEpisodeDto
								{
									MediaItemId = mediaItemId,
									SeasonId = seasonId,
									Name = ep.Name ?? $"Епізод {ep.EpisodeNumber}",
									Description = ep.Overview,
									Duration = ep.Runtime,
									AirDate = ep.AirDate?.ToUniversalTime(),
									IsFiller = false,
									ImagePath = epImages,
									SeoAddition = new SeoAdditionInputDto
									{
										Slug = epSlug,
										MetaTitle = ep.Name,
										Description = ep.Overview,
										MetaDescription = ep.Overview
									}
								};
								var epRes = await _episodeService.CreateEpisodeAsync(epCreate);
								if (!epRes.IsSuccess) continue;
								var episodeId = epRes.Data.Id;

								// Add episode scenes (from still_path)
								var epAttachments = new List<AttachmentDto>();
								if (!string.IsNullOrEmpty(ep.StillPath))
								{
									epAttachments.Add(new AttachmentDto
									{
										Id = Guid.NewGuid(),
										MediaItemId = mediaItemId,
										AttachmentType = AttachmentType.Scene,
										AttachmentUrl = "https://image.tmdb.org/t/p/original" + ep.StillPath
									});
								}

								if (epAttachments.Any())
									await _attachmentService.AddAttachmentsAsync(epAttachments);
							}
						}
					}
				}

				Console.WriteLine($"Series '{seriesDetails.Name}' added.");
			}
		}

		private async Task<string> MakeSlugUniqueAsync(string slug)
		{
			string originalSlug = slug;
			int counter = 1;

			while (await SlugExistsAsync(slug))
			{
				counter++;
				slug = $"{originalSlug}-{counter}";
			}

			return slug;
		}

		private async Task<bool> SlugExistsAsync(string slug)
		{
			return await Task.Run(() => _mediaItemService.CheckSlugExists(slug));
		}

		private async Task<long> GetRestrictedRatingForMovie(int movieId, Movie movieDetails)
		{
			if (movieDetails.ReleaseDates?.Results == null) return 1;
			var usRelease = movieDetails.ReleaseDates.Results.FirstOrDefault(r => r.Iso_3166_1 == "US");
			if (usRelease?.ReleaseDates == null || !usRelease.ReleaseDates.Any()) return 1;

			var cert = usRelease.ReleaseDates.FirstOrDefault()?.Certification;
			if (string.IsNullOrWhiteSpace(cert)) return 1;

			cert = cert.ToUpper().Trim();

			switch (cert)
			{
				case "G": return 1;
				case "PG": return 2;
				case "PG-13": return 3;
				case "R": return 4;
				case "NC-17": return 5;
				default:
					return 1;
			}
		}

		private int MapStatus(string tmdbStatus)
		{
			switch (tmdbStatus)
			{
				case "Released":
					return 1;
				case "Planned":
					return 2;
				case "In Production":
				case "Post Production":
					return 3;
				case "Canceled":
					return 5;
				default:
					return 1;
			}
		}

		private int MapStatusTv(string tmdbStatus)
		{
			switch (tmdbStatus)
			{
				case "Ended":
					return 1;
				case "Planned":
					return 2;
				case "Returning Series":
				case "In Production":
				case "Pilot":
					return 3;
				case "Canceled":
					return 5;
				default:
					return 1;
			}
		}

		private string GenerateSlug(string title)
		{
			if (string.IsNullOrWhiteSpace(title)) return "no-title";

			var slug = title.ToLower();
			var charsToRemove = new[] { "'", ":", ",", ".", "!", "?", "(", ")", "\"" };
			foreach (var c in charsToRemove) slug = slug.Replace(c, "");
			slug = slug.Replace(" ", "-");
			while (slug.Contains("--"))
				slug = slug.Replace("--", "-");
			slug = slug.Trim('-');

			return string.IsNullOrEmpty(slug) ? "no-title" : slug;
		}

		private string MapCountryCodeToName(string code)
		{
			switch (code.ToUpper())
			{
				case "US": return "США";
				case "GB": return "Велика Британія";
				case "FR": return "Франція";
				case "DE": return "Німеччина";
				case "JP": return "Японія";
				case "UA": return "Україна";
				case "CA": return "Канада";
				case "AU": return "Австралія";
				// Додайте інші за потреби
				default: return "США";
			}
		}

		private async Task<bool> IsMediaItemDuplicateAsync(string nameYearKey)
		{
			var searchDto = new MediaItemSearchDto
			{
				Query = nameYearKey,
				SortDescending = false
			};
			var result = await _mediaItemService.SearchMediaItemsAsync(searchDto, 1, 10);
			return result.Data.Any(mi => (mi.Name + (mi.FirstAirDate?.ToString() ?? "")) == nameYearKey);
		}

		private string GetTrailerUrl(ResultContainer<Video> videos)
		{
			if (videos?.Results == null) return null;
			// шукаємо офіційний трейлер на YouTube
			var trailer = videos.Results.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
			if (trailer == null) return null;
			return "https://www.youtube.com/watch?v=" + trailer.Key;
		}

		private async Task<(List<PersonRoleDto> personRoles, List<(Guid PersonId, string ImagePath)> personImageCommands)>
	GetPersonRolesAndImagesForMovie(MovieCredits credits, Dictionary<string, Guid> personMap)
		{
			var personRoles = new List<PersonRoleDto>();
			var personImageCommands = new List<(Guid PersonId, string ImagePath)>();

			// Process Cast for Actors
			foreach (var castMember in credits.Cast.Take(10)) // Limit to top 10 actors
			{
				if (!personMap.TryGetValue(castMember.Name, out var personId))
					continue;

				personRoles.Add(new PersonRoleDto
				{
					PersonId = personId,
					Role = (int)PersonRole.Actor
				});

				if (!string.IsNullOrEmpty(castMember.ProfilePath))
				{
					personImageCommands.Add((personId, castMember.ProfilePath));
				}
			}

			// Process Crew for Directors, Producers, and Writers
			foreach (var crewMember in credits.Crew)
			{
				if (!personMap.TryGetValue(crewMember.Name, out var personId))
					continue;

				switch (crewMember.Job)
				{
					case "Director":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Director
						});
						break;

					case "Producer":
					case "Executive Producer":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Producer
						});
						break;

					case "Writer":
					case "Screenplay":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Writer
						});
						break;
				}

				if (!string.IsNullOrEmpty(crewMember.ProfilePath))
				{
					personImageCommands.Add((personId, crewMember.ProfilePath));
				}
			}

			personRoles = personRoles.Distinct().ToList();

			return (personRoles, personImageCommands);
		}

		private async Task<(List<PersonRoleDto> personRoles, List<(Guid PersonId, string ImagePath)> personImageCommands)>
			GetPersonRolesAndImagesForSeries(TvCredits credits, Dictionary<string, Guid> personMap)
		{
			var personRoles = new List<PersonRoleDto>();
			var personImageCommands = new List<(Guid PersonId, string ImagePath)>();

			// Process Cast for Actors
			foreach (var castMember in credits.Cast.Take(10)) // Limit to top 10 actors
			{
				if (!personMap.TryGetValue(castMember.Name, out var personId))
					continue;

				personRoles.Add(new PersonRoleDto
				{
					PersonId = personId,
					Role = (int)PersonRole.Actor
				});

				if (!string.IsNullOrEmpty(castMember.ProfilePath))
				{
					personImageCommands.Add((personId, castMember.ProfilePath));
				}
			}

			// Process Crew for Directors, Producers, and Writers
			foreach (var crewMember in credits.Crew)
			{
				if (!personMap.TryGetValue(crewMember.Name, out var personId))
					continue;

				switch (crewMember.Job)
				{
					case "Director":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Director
						});
						break;

					case "Producer":
					case "Executive Producer":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Producer
						});
						break;

					case "Writer":
					case "Screenplay":
						personRoles.Add(new PersonRoleDto
						{
							PersonId = personId,
							Role = (int)PersonRole.Writer
						});
						break;
				}

				if (!string.IsNullOrEmpty(crewMember.ProfilePath))
				{
					personImageCommands.Add((personId, crewMember.ProfilePath));
				}
			}

			personRoles = personRoles.Distinct().ToList();

			return (personRoles, personImageCommands);
		}


		private async Task AddPersonImagesAsync(List<(Guid PersonId, string ImagePath)> personImageCommands)
		{
			foreach (var cmd in personImageCommands)
			{
				if (string.IsNullOrEmpty(cmd.ImagePath)) continue;

				var fileDto = new MovieWave.Domain.Dto.S3Storage.FileDto
				{
					FileName = Guid.NewGuid().ToString() + ".jpg",
					Content = await DownloadImageStreamAsync("https://image.tmdb.org/t/p/original" + cmd.ImagePath),
					ContentType = "image/jpeg"
				};

				var createImageDto = new MovieWave.Domain.Dto.PersonImage.CreatePersonImageDto
				{
					PersonId = cmd.PersonId,
					ImageType = MovieWave.Domain.Enum.PersonImageType.AvatarPath
				};

				await _personImageService.UploadPersonImageAsync(createImageDto, fileDto);
			}
		}

		private async Task<Stream> DownloadImageStreamAsync(string url)
		{
			var httpClient = new System.Net.Http.HttpClient();
			var response = await httpClient.GetAsync(url);

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStreamAsync();
		}
	}
}