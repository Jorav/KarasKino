namespace KarasKino.Core.Base;

public abstract class Entity : IAggregateRoot
{
  public Guid Id { get; private set; } = Guid.NewGuid();
}
