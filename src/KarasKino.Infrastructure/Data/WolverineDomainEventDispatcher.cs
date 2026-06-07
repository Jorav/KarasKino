using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace KarasKino.Infrastructure.Data;

public class WolverineDomainEventDispatcher(
  IMessageBus bus,
  ILogger<WolverineDomainEventDispatcher> logger) : IDomainEventDispatcher
{
  public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents)
  {
    foreach (var entity in entitiesWithEvents)
    {
      if (entity is not IHasDomainEvents hasDomainEvents)
      {
        logger.LogError(
          "Entity of type {EntityType} does not inherit from {BaseType}. Unable to clear domain events.",
          entity.GetType().Name,
          nameof(IHasDomainEvents));
        continue;
      }

      var events = hasDomainEvents.DomainEvents.ToArray();
      hasDomainEvents.ClearDomainEvents();

      foreach (var domainEvent in events)
      {
        await bus.PublishAsync(domainEvent);
      }
    }
  }
}
