using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Repositories;

namespace MovieWave.Domain.Interfaces.Databases;

public interface IUnitOfWork : IStateSaveChanges
{
    Task<IDbContextTransaction> BeginTransactionAsync();

	IBaseRepository<User> Users { get; set; }

	IBaseRepository<Role> Roles { get; set; }

	IBaseRepository<UserRole> UserRoles { get; set; }

	IBaseRepository<MediaItem> MediaItems { get; set; }

	IBaseRepository<Attachment> Attachments { get; set; }

	IBaseRepository<Banner> Banners { get; set; }

	IBaseRepository<Person> Persons { get; set; }

	IBaseRepository<PersonImage> PersonImages { get; set; }


	IBaseRepository<Tag> Tags { get; set; }
}