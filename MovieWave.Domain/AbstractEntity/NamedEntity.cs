namespace MovieWave.Domain.AbstractEntity;

/// <inheritdoc/>
public abstract class NamedEntity<TKey> : BaseEntity<TKey>
{
	public string Name { get; set; }
}
