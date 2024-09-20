namespace MovieWave.Domain.AbstractEntity;

/// <summary>
/// Base abstract class for entities with an identifier.
/// TKey represents the data type of the identifier.
/// </summary>
/// <typeparam name="TKey">The data type of the identifier (Guid or int)</typeparam>
public abstract class BaseEntity<TKey>
{
	public TKey Id { get; set; }
}
