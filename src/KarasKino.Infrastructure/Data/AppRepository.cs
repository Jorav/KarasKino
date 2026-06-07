using Ardalis.Specification;

namespace KarasKino.Infrastructure.Data;

public class AppRepository<T>(AppDbContext db)
  : RepositoryBase<T>(db), IRepositoryBase<T> where T : class;
