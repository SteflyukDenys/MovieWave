using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.DAL.Repositories;
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
	private readonly IUnitOfWork _unitOfWork;
	private readonly IStorageService _storageService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public PersonService(
		IBaseRepository<Person> personRepository,
		ILogger logger,
		IMapper mapper, IUnitOfWork unitOfWork, IStorageService storageService)
	{
		_personRepository = personRepository;
		_logger = logger;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_storageService = storageService;
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

	public async Task<CollectionResult<PersonDto>> GetPersonsByIdsAsync(List<Guid> personIds)
	{
		var persons = await _personRepository.GetAll()
			.Where(p => personIds.Contains(p.Id))
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

		return new CollectionResult<PersonDto>
		{
			Data = personDtos,
			Count = personDtos.Count
		};
	}


	public async Task<CollectionResult<PersonDto>> GetAllPersonsAsync()
	{
		var persons = await _personRepository.GetAll()
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
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var person = await _personRepository.GetAll()
				.Include(p => p.Images)
				.FirstOrDefaultAsync(p => p.Id == personId);

			if (person == null)
			{
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.PersonNotFound,
					ErrorCode = (int)ErrorCodes.PersonNotFound
				};
			}

			foreach (var image in person.Images)
			{
				if (!string.IsNullOrEmpty(image.ImagePath))
				{
					var deleteResult = await _storageService.DeleteFileAsync(image.ImagePath);
					if (!deleteResult.IsSuccess)
					{
						_logger.Warning("Не вдалося видалити файл {ImagePath}: {ErrorMessage}", image.ImagePath, deleteResult.ErrorMessage);
					}
				}
			}

			_personRepository.Remove(person);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			return new BaseResult();
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Помилка при видаленні Person: {Message}", ex.Message);

			return new BaseResult
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<Dictionary<string, Guid>> GetOrCreatePersonsByNamesAsync(List<string> names)
	{
		if (names == null || !names.Any())
			return new Dictionary<string, Guid>();

		var persons = await _personRepository.GetAll()
			.Where(p => names.Contains((p.FirstName + " " + p.LastName).Trim())
						|| names.Contains(p.FirstName))
			.ToListAsync();

		var result = new Dictionary<string, Guid>();

		foreach (var p in persons)
		{
			var fullName = (p.FirstName + " " + p.LastName).Trim();
			if (string.IsNullOrWhiteSpace(p.LastName))
				fullName = p.FirstName;
			if (!result.ContainsKey(fullName))
				result[fullName] = p.Id;
		}

		var missing = names.Where(n => !result.ContainsKey(n)).ToList();

		foreach (var m in missing)
		{
			var parts = m.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			string firstName = parts[0];
			string lastName = parts.Length > 1 ? parts[1] : "";

			var newPerson = new Person
			{
				Id = Guid.NewGuid(),
				FirstName = firstName,
				LastName = lastName
			};

			await _personRepository.CreateAsync(newPerson);
			await _personRepository.SaveChangesAsync();

			var fullName = (newPerson.FirstName + " " + newPerson.LastName).Trim();
			if (string.IsNullOrWhiteSpace(newPerson.LastName))
				fullName = newPerson.FirstName;
			result[fullName] = newPerson.Id;
		}

		return result;
	}

}