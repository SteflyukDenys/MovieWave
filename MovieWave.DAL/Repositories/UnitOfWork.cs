using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;

namespace MovieWave.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _context;

	public UnitOfWork(AppDbContext context, IBaseRepository<User> users, IBaseRepository<Role> roles, IBaseRepository<UserRole> userRoles)
	{
		_context = context;
		Users = users;
		Roles = roles;
		UserRoles = userRoles;
	}

	public async Task<IDbContextTransaction> BeginTransactionAsync()
	{
		return await _context.Database.BeginTransactionAsync();
	}

	public async Task<int> SaveChangesAsync()
	{
		return await _context.SaveChangesAsync();
	}

	public IBaseRepository<User> Users { get; set; }

	public IBaseRepository<Role> Roles { get; set; }

	public IBaseRepository<UserRole> UserRoles { get; set; }

}