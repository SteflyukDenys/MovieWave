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
	UserNotFound = 11,
	UserAlreadyExists = 12,
	InvalidLogin = 13,
	InvalidPassword = 14,
	UserCreationFailed = 15,
	PasswordNotEqualsPasswordConfirm = 16,

	// Country Errors
	CountriesNotFound = 20,
	CountryNotFound = 21,
	CountryAlreadyExists = 22,

	// Status Errors
	StatusesNotFound = 25,
	StatusNotFound = 26,

	// General Errors
	InternalServerError = 90,
	ErrorSeedingDb = 500
}
