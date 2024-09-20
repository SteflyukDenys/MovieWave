using MovieWave.Domain.Interfaces;

namespace MovieWave.Domain.AbstractEntity;

/// <inheritdoc/>
public abstract class AuditableEntity<TKey> : BaseEntity<TKey>, IAuditable
{
	public DateTime CreatedAt { get; set; }
	public long CreatedBy { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public long? UpdatedBy { get; set; }
}
