using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class RoleDataGenerator
{
	public static List<Role> GenerateRoles()
	{
		return new List<Role>
		{
			new Role
			{
				Id = 1L,
				Name = "User"
			},
			new Role
			{
				Id = 2L,
				Name = "Admin"
			},
			new Role
			{
				Id = 3L,
				Name = "Moderator"
			}
		};
	}
}