using Stellarity.Database.Entities;

namespace Stellarity.Domain.Abstractions;

public abstract class DomainModel<TEntity>
    where TEntity : IEntity
{
    /// <summary>
    /// Base .ctor for resolving models from entities
    /// </summary>
    /// <param name="entity">Model's entity</param>
    protected DomainModel(TEntity entity) => Entity = entity;

    internal TEntity Entity { get; }
}