using KarasKino.Core.Base;

namespace KarasKino.Infrastructure.Data;

internal sealed class Repository<TEntity>(AppDbContext db)
  : RepositoryBase<TEntity>(db), Core.Interfaces.IRepository<TEntity>
  where TEntity : Entity;
