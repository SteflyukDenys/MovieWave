using MovieWave.Application.Resources;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Validations;
using MovieWave.Domain.Result;

namespace MovieWave.Application.Validations.FluentValidations.MediaItem;

public class MediaItemValidator : IMediaItemValidator
{
    public BaseResult ValidateOnNull(Domain.Entity.MediaItem model)
    {
        if (model == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.MediaItemNotFound,
                ErrorCode = (int)ErrorCodes.MediaItemNotFound
            };
        }
        return new BaseResult();
    }

    public BaseResult CreateValidator(Domain.Entity.MediaItem mediatem)
    {
        if (mediatem != null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.MediaItemAlreadyExists,
                ErrorCode = (int)ErrorCodes.MediaItemAlreadyExists
            };
        }
        return new BaseResult();
    }

}
