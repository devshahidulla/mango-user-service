public interface IEventPublisher
{
  Task PublishAsync<T>(string detailType, string source, T @event, CancellationToken cancellationToken);
}
