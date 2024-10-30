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
		try
		{
			studios = await _studioRepository.GetAll()
				.Include(s => s.SeoAddition)
				.Select(s => _mapper.Map<StudioDto>(s))
				.ToListAsync();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new CollectionResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}

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

		try
		{
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
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}

		return new BaseResult<StudioDto> { Data = studioDto };
	}

	public async Task<BaseResult<StudioDto>> CreateAsync(CreateStudioDto dto)
	{
		try
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<StudioDto>> UpdateAsync(UpdateStudioDto dto)
	{
		try
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<StudioDto>> DeleteAsync(long id)
	{
		try
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}
}
