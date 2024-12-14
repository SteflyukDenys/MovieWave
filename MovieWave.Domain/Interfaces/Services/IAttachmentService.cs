using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IAttachmentService
{
	Task<BaseResult<AttachmentDto>> UploadAttachmentAsync(CreateAttachmentDto dto, FileDto uploadFile);

	Task<BaseResult<AttachmentDto>> GetAttachmentByIdAsync(Guid attachmentId);

	Task<CollectionResult<AttachmentDto>> GetAttachmentsByMediaItemIdAsync(Guid mediaItemId);

	Task<BaseResult<AttachmentDto>> UpdateAttachmentAsync(UpdateAttachmentDto dto, FileDto newAttachment);

	Task<BaseResult> DeleteAttachmentAsync(Guid attachmentId);

	Task<CollectionResult<AttachmentDto>> AddAttachmentsAsync(List<AttachmentDto> attachments);
}