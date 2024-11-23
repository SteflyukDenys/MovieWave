using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class StudioService : IStudioService
{
	private readonly IBaseRepository<Studio> _studioRepository;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public StudioService(IBaseRepository<Studio> studioRepository, ILogger logger, IMapper mapper)
	{
		_studioRepository = studioRepository;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<CollectionResult<StudioDto>> GetAllAsync()
	{
		List<StudioDto> studios;

		studios = await _studioRepository.GetAll()
			.Include(s => s.SeoAddition)
			.Select(s => _mapper.Map<StudioDto>(s))
			.ToListAsync();

		if (!studios.Any())
		{
			_logger.Warning(ErrorMessage.StudiosNotFound);
			return new CollectionResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudiosNotFound,
				ErrorCode = (int)ErrorCodes.StudiosNotFound
			};
		}

		return new CollectionResult<StudioDto> { Data = studios, Count = studios.Count };
	}

	public async Task<BaseResult<StudioDto>> GetByIdAsync(long id)
	{
		StudioDto? studioDto;

		var studio = await _studioRepository.GetAll()
			.Include(s => s.SeoAddition)
			.FirstOrDefaultAsync(s => s.Id == id);

		if (studio == null)
		{
			_logger.Warning($"Studio {id} not found");
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioNotFound,
				ErrorCode = (int)ErrorCodes.StudioNotFound
			};
		}

		studioDto = _mapper.Map<StudioDto>(studio);

		return new BaseResult<StudioDto> { Data = studioDto };
	}

	public async Task<BaseResult<StudioDto>> CreateAsync(CreateStudioDto dto)
	{
		var existingStudio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Name == dto.Name);
		if (existingStudio != null)
		{
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioAlreadyExists,
				ErrorCode = (int)ErrorCodes.StudioAlreadyExists
			};
		}

		var newStudio = _mapper.Map<Studio>(dto);
		await _studioRepository.CreateAsync(newStudio);
		await _studioRepository.SaveChangesAsync();

		return new BaseResult<StudioDto> { Data = _mapper.Map<StudioDto>(newStudio) };
	}

	public async Task<BaseResult<StudioDto>> UpdateAsync(UpdateStudioDto dto)
	{
		var studio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Id == dto.Id);
		if (studio == null)
		{
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioNotFound,
				ErrorCode = (int)ErrorCodes.StudioNotFound
			};
		}

		_mapper.Map(dto, studio);
		var updatedStudio = _studioRepository.Update(studio);
		await _studioRepository.SaveChangesAsync();

		return new BaseResult<StudioDto> { Data = _mapper.Map<StudioDto>(updatedStudio) };
	}

	public async Task<BaseResult<StudioDto>> DeleteAsync(long id)
	{
		var studio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Id == id);
		if (studio == null)
		{
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioNotFound,
				ErrorCode = (int)ErrorCodes.StudioNotFound
			};
		}

		_studioRepository.Remove(studio);
		await _studioRepository.SaveChangesAsync();

		return new BaseResult<StudioDto> { Data = _mapper.Map<StudioDto>(studio) };
	}
}
