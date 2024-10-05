using FluentValidation;
using MovieWave.Domain.Dto.MediaItem;

namespace MovieWave.Application.Validations.FluentValidations.MediaItem;

public class UpdateMediaItemValidator : AbstractValidator<UpdateMediaItemDto>
{
	public UpdateMediaItemValidator()
	{
		RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
	}
}

