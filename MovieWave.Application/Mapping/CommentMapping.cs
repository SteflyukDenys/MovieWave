using AutoMapper;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class CommentMapping : Profile
{
	public CommentMapping()
	{
		CreateMap<Comment, CommentDto>()
			.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
			.ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));

		CreateMap<AddCommentDto, Comment>()
			.ForMember(dest => dest.CommentableId, opt => opt.MapFrom(src => src.MediaItemId))
			.ForMember(dest => dest.CommentableType, opt => opt.MapFrom(src => "MediaItem"));
	}
}