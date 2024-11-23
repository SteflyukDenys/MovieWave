﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MovieWave.Domain.Dto.MediaItem;

namespace MovieWave.Application.Validations.FluentValidations.MediaItem;


public class MediaItemSearchDtoValidator : AbstractValidator<MediaItemSearchDto>
{
    public MediaItemSearchDtoValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(x => x.SortBy).Must(value =>
                string.IsNullOrEmpty(value) ||
                value == "ReleaseDate" ||
                value == "Name")
            .WithMessage("Invalid SortBy value.");
    }
}