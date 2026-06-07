using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KarasKino.Infrastructure.Data;

public class EventDispatchInterceptor(IDomainEventDispatcher dispatcher) : SaveChangesInterceptor
{
  public override async ValueTask<int> SavedChangesAsync(
    SaveChangesCompletedEventData eventData,
    int result,
    CancellationToken cancellationToken = default)
  {
    var context = eventData.Context;
    if (context is not AppDbContext appDbContext)
      return await base.SavedChangesAsync(eventData, result, cancellationToken);

    var entitiesWithEvents = appDbContext.ChangeTracker
      .Entries<HasDomainEventsBase>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Any())
      .ToArray();

    await dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return await base.SavedChangesAsync(eventData, result, cancellationToken);
  }
}
