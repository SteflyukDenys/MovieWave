using Microsoft.EntityFrameworkCore;

namespace MovieWave.Domain.AbstractEntity;

public static class PaginationHelper
{
	public static async Task<(List<T>, int)> PaginateAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
	{
		if (pageNumber <= 0) pageNumber = 1;
		if (pageSize <= 0) pageSize = 10;

		var totalItems = await query.CountAsync();
		var items = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return (items, totalItems);
	}
}
