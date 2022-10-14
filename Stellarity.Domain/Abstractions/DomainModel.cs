using Stellarity.Database.Entities;

namespace Stellarity.Domain.Abstractions;

public abstract class DomainModel<TEntity>
    where TEntity : IEntity
{
    /// <summary>
    /// Base .ctor for resolving models from entities
    /// </summary>
    /// <param name="entity">Model's entity</param>
    protected DomainModel(TEntity entity)
    {
        if (entity is null) throw new NullReferenceException("Model's entity was null!");
        Entity = entity;
    }

    internal TEntity Entity { get; }
}