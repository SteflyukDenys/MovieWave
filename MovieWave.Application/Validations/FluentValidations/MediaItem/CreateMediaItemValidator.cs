using FluentValidation;
using MovieWave.Domain.Dto.MediaItem;

namespace MovieWave.Application.Validations.FluentValidations.MediaItem;

public class CreateMediaItemValidator : AbstractValidator<CreateMediaItemDto>
{
    public CreateMediaItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}
