using KarasKino.Core.Base;

namespace KarasKino.Core.Interfaces;

public interface IRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity;
