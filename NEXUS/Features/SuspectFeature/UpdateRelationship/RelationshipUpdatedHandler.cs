using NEXUS.Services;

namespace NEXUS.Features.SuspectFeature.UpdateRelationship;

public sealed class RelationshipUpdatedHandler(IIntelligenceEngine intelligenceEngine)
{
    public async Task Handle(RelationshipUpdatedEvent @event)
    {
        // Update the link in the intelligence engine
        await intelligenceEngine.CreateLinkAsync(@event.SuspectId, @event.RelatedPersonId, @event.Relationship.ToString());
    }
}
