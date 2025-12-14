using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

public class EventBridgePublisher : IEventPublisher
{
  private readonly IAmazonEventBridge _eventBridge;
  private readonly IConfiguration _config;
  private readonly ILogger<EventBridgePublisher> _logger;

  public EventBridgePublisher(
      IAmazonEventBridge eventBridge,
      IConfiguration config,
      ILogger<EventBridgePublisher> logger)
  {
    _eventBridge = eventBridge;
    _config = config;
    _logger = logger;
  }

  public async Task PublishAsync<T>(string detailType, string source, T @event, CancellationToken cancellationToken)
  {
    var request = new PutEventsRequest
    {
      Entries = new List<PutEventsRequestEntry>
            {
                new()
                {
                    EventBusName = _config["AWS:EventBusName"],
                    Source = source,
                    DetailType = detailType,
                    Detail = JsonSerializer.Serialize(@event),
                }
            }
    };

    var response = await _eventBridge.PutEventsAsync(request, cancellationToken);

    if (response.FailedEntryCount > 0)
    {
      _logger.LogError("Failed to publish event to EventBridge: {Failures}", response.Entries);
    }
    else
    {
      _logger.LogInformation("Successfully published event: {DetailType}", detailType);
    }
  }
}
