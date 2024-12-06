﻿using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IPersonService
{
	Task<BaseResult<PersonDto>> CreatePersonAsync(CreatePersonDto dto);

	Task<BaseResult<PersonDto>> UpdatePersonAsync(UpdatePersonDto dto);

	Task<BaseResult<PersonDto>> GetPersonByIdAsync(Guid personId);

	Task<CollectionResult<PersonDto>> GetAllPersonsAsync();

	Task<BaseResult> DeletePersonAsync(Guid personId);
}