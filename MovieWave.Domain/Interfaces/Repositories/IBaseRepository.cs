using MovieWave.Domain.Interfaces.Databases;

namespace MovieWave.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> : IStateSaveChanges
{
	IQueryable<TEntity> GetAll();

	Task<TEntity> CreateAsync(TEntity entity);

	TEntity Update(TEntity entity);

	void Remove(TEntity entity);
}
