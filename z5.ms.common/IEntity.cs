namespace z5.ms.common
{
    /// <summary>Empty interface to indicate that model is a Domain entity with identity</summary>
    /// <remarks>
    /// Anemic models shall be used in CRUD bounding context for simplicity (behavior is in a service object).
    /// Aggregates shall be used for complex business logic (behavior is encapsulated in an Aggregate).
    /// </remarks>
    public interface IEntity
    {
        ///// <summary>Crete DB table, triggers, etc.</summary>
        //void CreateTable();
    }

    /// <summary>Interface to indicate immutable object with no identity</summary>
    public interface IValueObject
    {
    }

    /// <summary>Empty interface to indicate that object is a view model (DTO)</summary>
    public interface IViewModel
    {
    }

    /// <summary>Empty interface to indicate that entity is a Domain entity aggregate root</summary>
    /// <remarks>All operations with entity properties must be done from withing this root</remarks>
    public interface IAggregateRoot : IEntity
    {
    }

    /// <summary>Empty interface to indicate that entity is a Domain entity aggregate</summary>
    public interface IAggregate : IEntity
    {
    }

    /// <summary>SeedWork repository interface</summary>
    /// <remarks>
    /// One repository per aggregate root shall be enforced!
    /// This means that repositories won't be created for every single table.
    /// </remarks>
    // ReSharper disable once UnusedTypeParameter
    public interface IRepository<T> where T : IAggregateRoot
    {
        //The DbContext object (exposed as an IUnitOfWork object) - not relevant for our dapper micro ORM preference
        //Will use db connection factory here
        //Important! lifetime of context or db connection must be set to Scoped!
        //IUnitOfWork UnitOfWork { get; }

        //Note that using the singleton lifetime for the repository could cause you serious concurrency
        //problems when your DbContext is set to scoped(InstancePerLifetimeScope) lifetime(the default lifetime for a DBContext).
    }
}