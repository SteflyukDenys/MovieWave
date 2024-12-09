﻿using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Country;

public record CountryDto(long Id, string Name, SeoAdditionDto SeoAddition);