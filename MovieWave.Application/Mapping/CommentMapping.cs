using AutoMapper;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class CommentMapping : Profile
{
	public CommentMapping()
	{
		CreateMap<Comment, CommentDto>().ReverseMap();

		//CreateMap<CreateCommentDto, Comment>()
		//	.AfterMap((src, dest) => dest.Id = Guid.NewGuid())
		//	.ReverseMap();
	}
}