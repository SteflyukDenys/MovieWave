namespace MovieWave.Domain.Enum;

public enum ErrorCodes
{
	// MediaItem Errors
	MediaItemsNotFound = 0,
	MediaItemNotFound = 1,
	MediaItemAlreadyExists = 2,

	// MediaItemType Errors
	MediaItemTypesNotFound = 6,
	MediaItemTypeNotFound = 7,

	// User Errors
	UserAlreadyExists = 10,
	InvalidLogin = 11,
	InvalidPassword = 12,
	UserCreationFailed = 13,
	PasswordNotEqualsPasswordConfirm = 14,
	UserNotFound = 15,
	UserAlreadyExistsThisRole = 16,
	InvalidInput = 17,

	// Role
	RoleAlreadyExists = 20,
	RoleNotFound = 21,
	RolesNotFound = 22,

	// Country Errors
	CountriesNotFound = 25,
	CountryNotFound = 26,
	CountryAlreadyExists = 27,

	// Status Errors
	StatusesNotFound = 30,
	StatusNotFound = 31,

	// RestrictedRating Errors
	RestrictedRatingsNotFound = 35,
	RestrictedRatingNotFound = 36,

	// Studio Errors
	StudiosNotFound = 37,
	StudioNotFound = 38,
	StudioAlreadyExists = 39,

	// Attachments Errors
	AttachmentNotFound = 40,

	// Banners Errors
	BannerNotFound = 45,
	BannersNotFound = 46,

	// Tags Errors
	TagsNotFound = 50,
	TagNotFound = 51,
	TagAlreadyExists = 52,

	// Person
	PersonNotFound = 55,
	PersonsNotFound = 56,
	PersonImageNotFound = 57,

	// Season
	SeasonNotFound = 60,

	// Episode
	EpisodeNotFound = 65,

	// Voice
	VoiceNotFound = 70,

	// EpisodeVoice
	EpisodeVoiceNotFound = 75,

	// Watch History
	WatchHistoryNotFound = 80,
	WantToWatchNotFound = 81,
	FavoritesNotFound = 82,

	// Comment
	ParentCommentNotFound = 85,
	CommentNotFound = 86,

	// General Errors
	InternalServerError = 90,
	ErrorSeedingDb = 95
}
