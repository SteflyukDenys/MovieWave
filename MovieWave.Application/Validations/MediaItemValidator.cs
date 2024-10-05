using MovieWave.Application.Resources;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Validations;
using MovieWave.Domain.Result;

namespace MovieWave.Application.Validations;

public class MediaItemValidator : IMediaItemValidator
{
	public BaseResult ValidateOnNull(MediaItem model)
	{
		if(model == null)
		{
			return new BaseResult()
			{
				ErrorMessage = ErrorMessage.MediaItemNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemNotFound
			};
		}
		return new BaseResult();
	}
	public BaseResult CreateValidator(MediaItem mediatem)
	{
		if(mediatem != null) 
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
