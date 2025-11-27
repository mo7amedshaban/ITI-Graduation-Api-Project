using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Common;

public abstract class Entity
{
    // Initialize the domain events list correctly
    private readonly List<DomainEvent> _domainEvents = new();

    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }

    // Make Id settable (protected) so Entity Framework can map it and constructors can assign it
    // Allow public set so higher-level code can set GUIDs prior to saving (helps when creating children with FK values)
    public Guid Id { get; set; }

    [NotMapped] public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}