using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Common.Domain.Events;

namespace Modules.Common.Application;

/// <summary>
/// Implementation of IEventPublisher that resolves all handlers for an event and executes them in parallel
/// </summary>
public class EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger)
    : IEventPublisher
{
	/// <summary>
	/// Publishes an event of the specified type to all registered event handlers asynchronously.
	/// </summary>
	/// <typeparam name="TEvent">The type of the event to be published, which must implement <see cref="IEvent"/>.</typeparam>
	/// <param name="event">The event instance to be published.</param>
	/// <param name="cancellationToken">A cancellation token to observe during the operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="AggregateException">
	/// Thrown when one or more handlers throw exceptions during execution. All exceptions
	/// are encapsulated within the <see cref="AggregateException"/>.
	/// </exception>
	/// <exception cref="Exception">Thrown if an error occurs during the publish operation.</exception>
	public async Task PublishAsync<TEvent>(
		TEvent @event,
		CancellationToken cancellationToken)
        where TEvent : IEvent
    {
        var eventType = @event.GetType();
        logger.LogDebug("Publishing event {EventType}", eventType.Name);

        try
        {
            // Resolve all handlers for this event type
            var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>().ToArray();

            if (handlers.Length == 0)
            {
                logger.LogDebug("No handlers registered for event {EventType}", eventType.Name);
                return;
            }

            logger.LogDebug("Found {HandlerCount} handlers for event {EventType}", handlers.Length, eventType.Name);

            // Execute all handlers and collect results
            var handlerTasks = handlers
                .Select(handler => ExecuteHandlerAsync(handler, @event, cancellationToken))
                .ToList();

            await Task.WhenAll(handlerTasks);

            // Check for exceptions
            var exceptions = handlerTasks
                .Select(t => t.Exception)
                .Where(ex => ex != null)
                .ToList();

            if (exceptions.Count > 0)
            {
                logger.LogError("One or more handlers threw exceptions while processing event {EventType}", eventType.Name);
                throw new AggregateException($"One or more handlers threw exceptions while processing event {eventType.Name}", exceptions!);
            }

            logger.LogDebug("Successfully published event {EventType}", eventType.Name);
        }
        catch (AggregateException)
        {
            // Let the aggregate exception propagate as is
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing event {EventType}", eventType.Name);
            throw;
        }
    }

    private async Task<Exception?> ExecuteHandlerAsync<TEvent>(
        IEventHandler<TEvent> handler,
        TEvent @event,
        CancellationToken cancellationToken) where TEvent : IEvent
    {
        try
        {
            await handler.HandleAsync(@event, cancellationToken);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling event {EventType} with handler {HandlerType}",
                @event.GetType().Name, handler.GetType().Name);
            return ex;
        }
    }
}
