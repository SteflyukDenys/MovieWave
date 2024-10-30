using AutoMapper;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class AttachmentMapping : Profile
{
	public AttachmentMapping()
	{
		CreateMap<Attachment, AttachmentDto>().ReverseMap();
	}
}