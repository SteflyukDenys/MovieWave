using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class StatusDataGenerator
{
	public static List<Status> GenerateStatuses()
	{
		return new List<Status>
		{
			new Status { Id = 1, StatusType = StatusType.Released, Description = "Фільм повністю випущений і доступний для перегляду." },
			new Status { Id = 2, StatusType = StatusType.Announced, Description = "Фільм анонсований, але дата випуску ще не визначена." },
			new Status { Id = 3, StatusType = StatusType.Ongoing, Description = "Фільм зараз у виробництві або випускається частинами." },
			new Status { Id = 4, StatusType = StatusType.Suspended, Description = "Виробництво або випуск фільма тимчасово призупинено." },
			new Status { Id = 5, StatusType = StatusType.Cancelled, Description = "Фільм був остаточно скасований і не буде випущений." }
		};
	}
}