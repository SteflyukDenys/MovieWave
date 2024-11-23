using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Role : NamedEntity<long>
{
    public List<User> Users { get; set; }
}