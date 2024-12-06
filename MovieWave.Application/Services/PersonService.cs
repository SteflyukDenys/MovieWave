using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class PersonService : IPersonService
{
	private readonly IBaseRepository<Person> _personRepository;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public PersonService(
		IBaseRepository<Person> personRepository,
		ILogger logger,
		IMapper mapper)
	{
		_personRepository = personRepository;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<BaseResult<PersonDto>> CreatePersonAsync(CreatePersonDto dto)
	{
		var person = _mapper.Map<Person>(dto);

		await _personRepository.CreateAsync(person);
		await _personRepository.SaveChangesAsync();

		var personDto = _mapper.Map<PersonDto>(person);
		return new BaseResult<PersonDto> { Data = personDto };
	}

	public async Task<BaseResult<PersonDto>> UpdatePersonAsync(UpdatePersonDto dto)
	{
			var person = await _personRepository.GetAll()
				.Include(p => p.SeoAddition)
				.FirstOrDefaultAsync(p => p.Id == dto.Id);

			if (person == null)
			{
				return new BaseResult<PersonDto>
				{
					ErrorMessage = ErrorMessage.PersonNotFound,
					ErrorCode = (int)ErrorCodes.PersonNotFound
				};
			}

			_mapper.Map(dto, person);

			_personRepository.Update(person);
			await _personRepository.SaveChangesAsync();

			var personDto = _mapper.Map<PersonDto>(person);
			return new BaseResult<PersonDto> { Data = personDto };
	}

	public async Task<BaseResult<PersonDto>> GetPersonByIdAsync(Guid personId)
	{
		var person = await _personRepository.GetAll()
			.Include(p => p.SeoAddition)
			.FirstOrDefaultAsync(p => p.Id == personId);

		if (person == null)
		{
			return new BaseResult<PersonDto>
			{
				ErrorMessage = ErrorMessage.PersonNotFound,
				ErrorCode = (int)ErrorCodes.PersonNotFound
			};
		}

		var personDto = _mapper.Map<PersonDto>(person);
		return new BaseResult<PersonDto> { Data = personDto };
	}

	public async Task<CollectionResult<PersonDto>> GetAllPersonsAsync()
	{
		var persons = await _personRepository.GetAll()
			.Include(p => p.SeoAddition)
			.ToListAsync();

		if (!persons.Any())
		{
			return new CollectionResult<PersonDto>
			{
				ErrorMessage = ErrorMessage.PersonsNotFound,
				ErrorCode = (int)ErrorCodes.PersonsNotFound
			};
		}

		var personDtos = _mapper.Map<List<PersonDto>>(persons);
		return new CollectionResult<PersonDto> { Data = personDtos, Count = personDtos.Count };
	}

	public async Task<BaseResult> DeletePersonAsync(Guid personId)
	{
		var person = await _personRepository.GetAll()
			.Include(p => p.SeoAddition)
			.FirstOrDefaultAsync(p => p.Id == personId);

		if (person == null)
		{
			return new BaseResult
			{
				ErrorMessage = ErrorMessage.PersonNotFound,
				ErrorCode = (int)ErrorCodes.PersonNotFound
			};
		}

		_personRepository.Remove(person);
		await _personRepository.SaveChangesAsync();

		return new BaseResult();
	}
}
