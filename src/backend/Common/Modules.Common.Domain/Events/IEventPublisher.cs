namespace Modules.Common.Domain.Events;

/// <summary>
/// Interface for publishing events
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to all registered handlers
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish</typeparam>
    /// <param name="event">The event instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
}
