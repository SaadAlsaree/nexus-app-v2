using NEXUS.Services;

namespace NEXUS.Features.SuspectFeature.UpdateStatus;

public sealed class SuspectStatusChangedHandler(IIntelligenceEngine intelligenceEngine)
{
    public async Task Handle(SuspectStatusUpdatedEvent @event)
    {
        // Update the status on the Neo4j node
        var properties = new Dictionary<string, object>
        {
            { "Status", @event.NewStatus.ToString() },
            { "LastUpdated", DateTime.UtcNow }
        };

        await intelligenceEngine.SyncNodeAsync(@event.SuspectId, "Suspect", properties);
    }
}
