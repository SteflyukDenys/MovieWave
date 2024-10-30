using AutoMapper;
using MovieWave.Domain.Dto.Notification;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class NotificationMapping : Profile
{
	public NotificationMapping()
	{
		CreateMap<Notification, NotificationDto>().ReverseMap();
	}
}