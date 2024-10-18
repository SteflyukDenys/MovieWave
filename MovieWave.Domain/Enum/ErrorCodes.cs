namespace MovieWave.Domain.Enum;

public enum ErrorCodes
{
	MediaItemsNotFound = 0,
	MediaItemNotFound = 1,
	MediaItemAlreadyExists = 2,

	MediaItemTypesNotFound = 6,
	MediaItemTypeNotFound = 7,

	InternalServerError = 90,
	ErrorSeedingDb = 500
}
