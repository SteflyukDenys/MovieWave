using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class AttachmentService : IAttachmentService
	{
		private readonly IStorageService _storageService;
		private readonly IBaseRepository<Attachment> _attachmentRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public AttachmentService(IStorageService storageService, IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IBaseRepository<Attachment> attachmentRepository)
		{
			_storageService = storageService;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_attachmentRepository = attachmentRepository;
		}

		public async Task<BaseResult<AttachmentDto>> UploadAttachmentAsync(CreateAttachmentDto dto, FileDto uploadFile)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				if (uploadFile == null || uploadFile.Content.Length == 0)
				{
					return new BaseResult<AttachmentDto>
					{
						ErrorMessage = ErrorMessage.InvalidFile,
						ErrorCode = 400
					};
				}

				var attachment = _mapper.Map<Attachment>(dto);
				// For S3
				var attachmentType = Enum.GetName(typeof(AttachmentType), dto.AttachmentType);
				var folder = $"mediaItems/{dto.MediaItemId}/attachments/{attachmentType}";

				var uploadAttachmentFile = await _storageService.UploadFileAsync(uploadFile, folder);

				if (!uploadAttachmentFile.IsSuccess)
				{
					return new BaseResult<AttachmentDto>
					{
						ErrorMessage = uploadAttachmentFile.ErrorMessage,
						ErrorCode = uploadAttachmentFile.ErrorCode
					};
				}

				attachment.AttachmentUrl = uploadAttachmentFile.Data;

				await _attachmentRepository.CreateAsync(attachment);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				var resultDto = _mapper.Map<AttachmentDto>(attachment);

				resultDto.AttachmentUrl = _storageService.GenerateFileUrl(resultDto.AttachmentUrl);

				return new BaseResult<AttachmentDto> { Data = resultDto };
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Помилка при створенні Attachment: {Message}", ex.Message);

				if (transaction != null && transaction.GetDbTransaction().Connection != null)
				{
					await transaction.RollbackAsync();
				}

				return new BaseResult<AttachmentDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}

		public async Task<BaseResult<AttachmentDto>> GetAttachmentByIdAsync(Guid attachmentId)
		{
			var attachment = await _unitOfWork.Attachments.GetAll()
				.FirstOrDefaultAsync(a => a.Id == attachmentId);

			if (attachment == null)
			{
				_logger.Warning("Attachment з ID {AttachmentId} не знайдено.", attachmentId);
				return new BaseResult<AttachmentDto>
				{
					ErrorMessage = ErrorMessage.AttachmentNotFound,
					ErrorCode = (int)ErrorCodes.AttachmentNotFound
				};
			}

			var attachmentDto = _mapper.Map<AttachmentDto>(attachment);
			attachmentDto.AttachmentUrl = _storageService.GenerateFileUrl(attachment.AttachmentUrl);

			return new BaseResult<AttachmentDto> { Data = attachmentDto };
		}

		public async Task<CollectionResult<AttachmentDto>> GetAttachmentsByMediaItemIdAsync(Guid mediaItemId)
		{
			var attachments = await _unitOfWork.Attachments.GetAll()
				.Where(a => a.MediaItemId == mediaItemId)
				.ToListAsync();

			var attachmentDtos = attachments.Select(a =>
			{
				var dto = _mapper.Map<AttachmentDto>(a);
				dto.AttachmentUrl = _storageService.GenerateFileUrl(a.AttachmentUrl);
				return dto;
			}).ToList();

			return new CollectionResult<AttachmentDto>
			{
				Data = attachmentDtos,
				Count = attachmentDtos.Count
			};
		}

		public async Task<BaseResult<AttachmentDto>> UpdateAttachmentAsync(UpdateAttachmentDto dto, FileDto newAttachment)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				var attachment = await _attachmentRepository.GetAll()
					.FirstOrDefaultAsync(x => x.Id == dto.Id);

				if (attachment == null)
				{
					return new BaseResult<AttachmentDto>()
					{
						ErrorMessage = ErrorMessage.AttachmentNotFound,
						ErrorCode = (int)ErrorCodes.AttachmentNotFound
					};
				}

				_mapper.Map(dto, attachment);

				if (newAttachment != null)
				{
					if (!string.IsNullOrEmpty(attachment.AttachmentUrl))
					{
						var deleteResult = await _storageService.DeleteFileAsync(attachment.AttachmentUrl);
						if (!deleteResult.IsSuccess)
						{
							_logger.Warning("Не вдалося видалити старий файл: {ErrorMessage}", deleteResult.ErrorMessage);
						}
					}

					var attachmentType = Enum.GetName(typeof(AttachmentType), dto.AttachmentType);
					var folder = $"mediaItems/{dto.MediaItemId}/attachments/{attachmentType}";

					var uploadAttachmentFile = await _storageService.UploadFileAsync(newAttachment, folder);
					if (!uploadAttachmentFile.IsSuccess)
					{
						return new BaseResult<AttachmentDto>
						{
							ErrorMessage = uploadAttachmentFile.ErrorMessage,
							ErrorCode = uploadAttachmentFile.ErrorCode
						};
					}

					attachment.AttachmentUrl = uploadAttachmentFile.Data;
				}


				_attachmentRepository.Update(attachment);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				var resultDto = _mapper.Map<AttachmentDto>(attachment);

				if (!string.IsNullOrEmpty(resultDto.AttachmentUrl))
				{
					resultDto.AttachmentUrl = _storageService.GenerateFileUrl(resultDto.AttachmentUrl);
				}

				return new BaseResult<AttachmentDto> { Data = resultDto };
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error("Помилка при оновленні Attachment: {Message}", ex.Message);
				return new BaseResult<AttachmentDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}

		public async Task<BaseResult> DeleteAttachmentAsync(Guid attachmentId)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				var attachment = await _unitOfWork.Attachments.GetAll()
					.FirstOrDefaultAsync(a => a.Id == attachmentId);

				if (attachment == null)
				{
					_logger.Warning("Attachment з ID {AttachmentId} не знайдено.", attachmentId);
					return new BaseResult
					{
						ErrorMessage = ErrorMessage.AttachmentNotFound,
						ErrorCode = (int)ErrorCodes.AttachmentNotFound
					};
				}

				var deleteResult = await _storageService.DeleteFileAsync(attachment.AttachmentUrl);

				if (!deleteResult.IsSuccess)
				{
					_logger.Error("Не вдалося видалити файл зі сховища: {ErrorMessage}", deleteResult.ErrorMessage);
					await transaction.RollbackAsync();
					return new BaseResult
					{
						ErrorMessage = deleteResult.ErrorMessage,
						ErrorCode = deleteResult.ErrorCode
					};
				}

				_unitOfWork.Attachments.Remove(attachment);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				return new BaseResult<AttachmentDto> { Data = _mapper.Map<AttachmentDto>(attachment) };
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error("Помилка при видаленні Attachment: {Message}", ex.Message);
				return new BaseResult<AttachmentDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}
	}
}
