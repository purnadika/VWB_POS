namespace POS.Domain.Common;

/// <summary>
/// Base class for aggregate roots in Domain-Driven Design.
/// Aggregate roots are entities that are accessed directly and own related entities.
/// They act as transaction boundaries and are responsible for maintaining invariants.
/// </summary>
public abstract class AggregateRoot
{
    // Derived classes should implement their own identity
    // (e.g., Guid Id, int Id, or composite keys)
}
