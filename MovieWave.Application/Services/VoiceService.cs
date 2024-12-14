using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Dto.Voice;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;
using TMDbLib.Objects.TvShows;

namespace MovieWave.Application.Services
{
	public class VoiceService : IVoiceService
	{
		private readonly IBaseRepository<Voice> _voiceRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public VoiceService(IBaseRepository<Voice> voiceRepository, IMapper mapper, ILogger logger)
		{
			_voiceRepository = voiceRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<BaseResult<VoiceDto>> CreateVoiceAsync(CreateVoiceDto dto)
		{
			var voice = new Voice
			{
				Name = dto.Name,
				Locale = dto.Locale,
				Description = dto.Description,
				IconPath = dto.IconPath
			};

			await _voiceRepository.CreateAsync(voice);
			await _voiceRepository.SaveChangesAsync();

			return new BaseResult<VoiceDto> { Data = _mapper.Map<VoiceDto>(voice) };
		}

		public async Task<BaseResult<VoiceDto>> UpdateVoiceAsync(UpdateVoiceDto dto)
		{
			var voice = await _voiceRepository.GetAll().FirstOrDefaultAsync(v => v.Id == dto.Id);
			if (voice == null)
			{
				return new BaseResult<VoiceDto>
				{
					ErrorMessage = ErrorMessage.VoiceNotFound,
					ErrorCode = (int)ErrorCodes.VoiceNotFound
				};
			}

			voice.Name = dto.Name;
			voice.Locale = dto.Locale;
			voice.Description = dto.Description;
			voice.IconPath = dto.IconPath;

			_voiceRepository.Update(voice);
			await _voiceRepository.SaveChangesAsync();

			return new BaseResult<VoiceDto> { Data = _mapper.Map<VoiceDto>(voice) };
		}

		public async Task<BaseResult> DeleteVoiceAsync(long voiceId)
		{
			var voice = await _voiceRepository.GetAll().FirstOrDefaultAsync(v => v.Id == voiceId);
			if (voice == null)
			{
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.VoiceNotFound,
					ErrorCode = (int)ErrorCodes.VoiceNotFound
				};
			}

			_voiceRepository.Remove(voice);
			await _voiceRepository.SaveChangesAsync();

			return new BaseResult<VoiceDto> { Data = _mapper.Map<VoiceDto>(voice) };
		}

		public async Task<BaseResult<VoiceDto>> GetVoiceByIdAsync(long voiceId)
		{
			var voice = await _voiceRepository.GetAll().FirstOrDefaultAsync(v => v.Id == voiceId);
			if (voice == null)
			{
				return new BaseResult<VoiceDto>
				{
					ErrorMessage = ErrorMessage.VoiceNotFound,
					ErrorCode = (int)ErrorCodes.VoiceNotFound
				};
			}

			return new BaseResult<VoiceDto> { Data = _mapper.Map<VoiceDto>(voice) };
		}

		public async Task<CollectionResult<VoiceDto>> GetAllVoicesAsync()
		{
			var voices = await _voiceRepository.GetAll().ToListAsync();
			var dtos = voices.Select(v => _mapper.Map<VoiceDto>(v)).ToList();
			return new CollectionResult<VoiceDto>
			{
				Data = dtos,
				Count = dtos.Count
			};
		}
	}
}
