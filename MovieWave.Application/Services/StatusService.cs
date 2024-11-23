using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Status;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class StatusService : IStatusService
	{
		private readonly IBaseRepository<Status> _statusRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public StatusService(IBaseRepository<Status> statusRepository, ILogger logger, IMapper mapper)
		{
			_statusRepository = statusRepository;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<CollectionResult<StatusDto>> GetAllAsync()
		{
			List<StatusDto> statuses;

			statuses = await _statusRepository.GetAll()
				.Select(s => _mapper.Map<StatusDto>(s))
				.ToListAsync();

			if (!statuses.Any())
			{
				_logger.Warning(ErrorMessage.StatusesNotFound);
				return new CollectionResult<StatusDto>
				{
					ErrorMessage = ErrorMessage.StatusesNotFound,
					ErrorCode = (int)ErrorCodes.StatusesNotFound
				};
			}

			return new CollectionResult<StatusDto> { Data = statuses, Count = statuses.Count };
		}

		public async Task<BaseResult<StatusDto>> GetByIdAsync(long id)
		{
			StatusDto? statusDto;

			var status = await _statusRepository.GetAll().FirstOrDefaultAsync(s => s.Id == id);

			if (status == null)
			{
				_logger.Warning($"Status {id} not found");
				return new BaseResult<StatusDto>
				{
					ErrorMessage = ErrorMessage.StatusNotFound,
					ErrorCode = (int)ErrorCodes.StatusNotFound
				};
			}

			statusDto = _mapper.Map<StatusDto>(status);

			return new BaseResult<StatusDto> { Data = statusDto };
		}

		public async Task<BaseResult<StatusDto>> UpdateAsync(UpdateStatusDto dto)
		{
			var status = await _statusRepository.GetAll().FirstOrDefaultAsync(s => s.Id == dto.Id);
			if (status == null)
			{
				return new BaseResult<StatusDto>
				{
					ErrorMessage = ErrorMessage.StatusNotFound,
					ErrorCode = (int)ErrorCodes.StatusNotFound
				};
			}

			_mapper.Map(dto, status);
			var updatedStatus = _statusRepository.Update(status);
			await _statusRepository.SaveChangesAsync();

			return new BaseResult<StatusDto> { Data = _mapper.Map<StatusDto>(updatedStatus) };
		}
	}
}
