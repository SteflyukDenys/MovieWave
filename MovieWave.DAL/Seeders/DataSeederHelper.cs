using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieWave.Domain.Entity;
using MovieWave.DAL.Seeders.DataGenerators;
using MovieWave.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MovieWave.DAL.Seeders
{
	public class DataSeederHelper
	{
		private readonly IBaseRepository<User> _userRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IBaseRepository<Comment> _commentRepository;
		private readonly IBaseRepository<Status> _statusRepository;
		private readonly IBaseRepository<MediaItemType> _mediaItemTypeRepository;
		private readonly IBaseRepository<RestrictedRating> _restrictedRatingRepository;
		private readonly IBaseRepository<Attachment> _attachmentRepository;
		private readonly IBaseRepository<Country> _countryRepository;
		private readonly IBaseRepository<Studio> _studioRepository;
		private readonly IBaseRepository<Tag> _tagRepository;
		private readonly IBaseRepository<Season> _seasonRepository;
		private readonly IBaseRepository<Episode> _episodeRepository;
		private readonly IBaseRepository<Review> _reviewRepository;
		private readonly IBaseRepository<Notification> _notificationRepository;
		private readonly IBaseRepository<UserSubscription> _userSubscriptionRepository;
		private readonly IBaseRepository<SubscriptionPlan> _subscriptionPlanRepository;
		private readonly IBaseRepository<Person> _personRepository;
		private readonly IBaseRepository<Voice> _voiceRepository;
		private readonly IBaseRepository<EpisodeVoice> _episodeVoiceRepository;

		public DataSeederHelper(
			IBaseRepository<User> userRepository,
			IBaseRepository<MediaItem> mediaItemRepository,
			IBaseRepository<Comment> commentRepository,
			IBaseRepository<Status> statusRepository,
			IBaseRepository<MediaItemType> mediaItemTypeRepository,
			IBaseRepository<RestrictedRating> restrictedRatingRepository,
			IBaseRepository<Attachment> attachmentRepository,
			IBaseRepository<Country> countryRepository,
			IBaseRepository<Studio> studioRepository,
			IBaseRepository<Tag> tagRepository,
			IBaseRepository<Season> seasonRepository,
			IBaseRepository<Episode> episodeRepository,
			IBaseRepository<Review> reviewRepository,
			IBaseRepository<Notification> notificationRepository,
			IBaseRepository<UserSubscription> userSubscriptionRepository,
			IBaseRepository<SubscriptionPlan> subscriptionPlanRepository,
			IBaseRepository<Person> personRepository,
			IBaseRepository<Voice> voiceRepository,
			IBaseRepository<EpisodeVoice> episodeVoiceRepository
			)
		{
			_userRepository = userRepository;
			_mediaItemRepository = mediaItemRepository;
			_commentRepository = commentRepository;
			_statusRepository = statusRepository;
			_mediaItemTypeRepository = mediaItemTypeRepository;
			_restrictedRatingRepository = restrictedRatingRepository;
			_attachmentRepository = attachmentRepository;
			_countryRepository = countryRepository;
			_studioRepository = studioRepository;
			_tagRepository = tagRepository;
			_seasonRepository = seasonRepository;
			_episodeRepository = episodeRepository;
			_reviewRepository = reviewRepository;
			_notificationRepository = notificationRepository;
			_userSubscriptionRepository = userSubscriptionRepository;
			_subscriptionPlanRepository = subscriptionPlanRepository;
			_personRepository = personRepository;
			_voiceRepository = voiceRepository;
			_episodeVoiceRepository = episodeVoiceRepository;
		}

		public async Task SeedAsync()
		{
			await SeedUsersAsync();
			await SeedStatusesAsync();
			await SeedMediaItemTypesAsync();
			await SeedRestrictedRatingsAsync();
			await SeedSubscriptionPlansAsync();
			await SeedMediaItemsAsync();
			await SeedSeasonsAsync();
			await SeedVoicesAsync();
			await SeedEpisodesAsync();
			await SeedCommentsAsync();
			await SeedNotificationsAsync();
			await SeedReviewsAsync();
			await SeedUserSubscriptionsAsync();
			await SeedAttachmentsAsync();
			await SeedCountriesAsync();
			await SeedStudiosAsync();
			await SeedTagsAsync();
			await SeedPeopleAsync();
			await SeedEpisodeVoicesAsync();
		}

		private async Task SeedUsersAsync()
		{
			var users = UserDataGenerator.GenerateUsers(10);
			foreach (var user in users)
			{
				await _userRepository.CreateAsync(user);
			}
			await _userRepository.SaveChangesAsync();
		}

		private async Task SeedStatusesAsync()
		{
			var statuses = StatusDataGenerator.GenerateStatuses(5);
			foreach (var status in statuses)
			{
				await _statusRepository.CreateAsync(status);
			}
			await _statusRepository.SaveChangesAsync();
		}

		private async Task SeedMediaItemTypesAsync()
		{
			var existingTypes = _mediaItemTypeRepository.GetAll().Select(t => t.MediaItemName).ToList();

			var mediaItemTypes = MediaItemTypeDataGenerator.GenerateMediaItemTypes()
				.Where(t => !existingTypes.Contains(t.MediaItemName))
				.ToList();

			foreach (var type in mediaItemTypes)
			{
				await _mediaItemTypeRepository.CreateAsync(type);
			}

			await _mediaItemTypeRepository.SaveChangesAsync();
		}

		private async Task SeedRestrictedRatingsAsync()
		{
			var restrictedRatings = RestrictedRatingDataGenerator.GenerateRestrictedRatings(5);
			foreach (var rating in restrictedRatings)
			{
				await _restrictedRatingRepository.CreateAsync(rating);
			}
			await _restrictedRatingRepository.SaveChangesAsync();
		}

		private async Task SeedSubscriptionPlansAsync()
		{
			var subscriptionPlans = SubscriptionPlanDataGenerator.GenerateSubscriptionPlans(3);
			foreach (var plan in subscriptionPlans)
			{
				await _subscriptionPlanRepository.CreateAsync(plan);
			}
			await _subscriptionPlanRepository.SaveChangesAsync();
		}

		private async Task SeedMediaItemsAsync()
		{
			var statuses = _statusRepository.GetAll().ToList();
			var mediaItemTypes = _mediaItemTypeRepository.GetAll().ToList();
			var restrictedRatings = _restrictedRatingRepository.GetAll().ToList();

			if (!statuses.Any() || !mediaItemTypes.Any() || !restrictedRatings.Any())
			{
				throw new InvalidOperationException("One or more required lists for media item generation are empty.");
			}

			var mediaItems = MediaItemDataGenerator.GenerateMediaItems(20, statuses, mediaItemTypes, restrictedRatings);
			foreach (var item in mediaItems)
			{
				await _mediaItemRepository.CreateAsync(item);
			}

			await _mediaItemRepository.SaveChangesAsync();
		}

		private async Task SeedSeasonsAsync()
		{
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var seasons = SeasonDataGenerator.GenerateSeasons(15, mediaItems);
			foreach (var season in seasons)
			{
				await _seasonRepository.CreateAsync(season);
			}
			await _seasonRepository.SaveChangesAsync();
		}

		private async Task SeedVoicesAsync()
		{
			var voices = VoiceDataGenerator.GenerateVoices(10);
			foreach (var voice in voices)
			{
				await _voiceRepository.CreateAsync(voice);
			}
			await _voiceRepository.SaveChangesAsync();
		}

		private async Task SeedEpisodesAsync()
		{
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var seasons = _seasonRepository.GetAll().ToList();
			var episodes = EpisodeDataGenerator.GenerateEpisodes(30, mediaItems, seasons);
			foreach (var episode in episodes)
			{
				await _episodeRepository.CreateAsync(episode);
			}
			await _episodeRepository.SaveChangesAsync();
		}

		private async Task SeedCommentsAsync()
		{
			var episodes = _episodeRepository.GetAll().ToList();
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var userIds = _userRepository.GetAll().Select(u => u.Id).ToList();
			var comments = CommentDataGenerator.GenerateComments(20, episodes, mediaItems, userIds, null);

			foreach (var comment in comments)
			{
				await _commentRepository.CreateAsync(comment);
			}

			await _commentRepository.SaveChangesAsync();
		}

		private async Task SeedNotificationsAsync()
		{
			var users = _userRepository.GetAll().ToList();
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var episodes = _episodeRepository.GetAll().ToList();
			var notifications = NotificationDataGenerator.GenerateNotifications(20, users, mediaItems, episodes);

			foreach (var notification in notifications)
			{
				await _notificationRepository.CreateAsync(notification);
			}

			await _notificationRepository.SaveChangesAsync();
		}

		private async Task SeedReviewsAsync()
		{
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var users = _userRepository.GetAll().ToList();
			var reviews = ReviewDataGenerator.GenerateReviews(30, mediaItems, users);

			foreach (var review in reviews)
			{
				await _reviewRepository.CreateAsync(review);
			}

			await _reviewRepository.SaveChangesAsync();
		}

		private async Task SeedUserSubscriptionsAsync()
		{
			var users = _userRepository.GetAll().ToList();
			var subscriptionPlans = _subscriptionPlanRepository.GetAll().ToList();
			var userSubscriptions = UserSubscriptionDataGenerator.GenerateUserSubscriptions(10, users, subscriptionPlans);

			foreach (var subscription in userSubscriptions)
			{
				await _userSubscriptionRepository.CreateAsync(subscription);
			}

			await _userSubscriptionRepository.SaveChangesAsync();
		}

		private async Task SeedAttachmentsAsync()
		{
			var mediaItems = _mediaItemRepository.GetAll().ToList();
			var attachments = AttachmentDataGenerator.GenerateAttachments(20, mediaItems);

			foreach (var attachment in attachments)
			{
				await _attachmentRepository.CreateAsync(attachment);
			}

			await _attachmentRepository.SaveChangesAsync();
		}

		private async Task SeedCountriesAsync()
		{
			var countries = CountryDataGenerator.GenerateCountries(10);
			foreach (var country in countries)
			{
				await _countryRepository.CreateAsync(country);
			}
			await _countryRepository.SaveChangesAsync();
		}

		private async Task SeedStudiosAsync()
		{
			var studios = StudioDataGenerator.GenerateStudios(10);
			foreach (var studio in studios)
			{
				await _studioRepository.CreateAsync(studio);
			}
			await _studioRepository.SaveChangesAsync();
		}

		private async Task SeedTagsAsync()
		{
			var tags = TagDataGenerator.GenerateTags(10, 5);

			foreach (var tag in tags)
			{
				await _tagRepository.CreateAsync(tag);
			}

			await _tagRepository.SaveChangesAsync();
		}

		private async Task SeedPeopleAsync()
		{
			var people = PersonDataGenerator.GeneratePeople(20);
			foreach (var person in people)
			{
				await _personRepository.CreateAsync(person);
			}
			await _personRepository.SaveChangesAsync();
		}

		private async Task SeedEpisodeVoicesAsync()
		{
			var episodes = _episodeRepository.GetAll().ToList();
			var voices = _voiceRepository.GetAll().ToList();

			if (!episodes.Any() || !voices.Any())
			{
				throw new InvalidOperationException("Episodes or Voices are missing for EpisodeVoice generation.");
			}

			var episodeVoices = EpisodeVoiceDataGenerator.GenerateEpisodeVoices(episodes, voices);

			foreach (var episodeVoice in episodeVoices)
			{
				var exists = await _episodeVoiceRepository.GetAll()
					.AnyAsync(ev => ev.EpisodeId == episodeVoice.EpisodeId && ev.VoiceId == episodeVoice.VoiceId);

				if (!exists)
				{
					await _episodeVoiceRepository.CreateAsync(episodeVoice);
				}
			}

			await _episodeVoiceRepository.SaveChangesAsync();
		}


	}
}
