using NEXUS.Services;

namespace NEXUS.Features.SuspectFeature.DeleteRelationship;

public sealed class RelationshipDeletedHandler(IIntelligenceEngine intelligenceEngine)
{
    public async Task Handle(RelationshipDeletedEvent @event)
    {
        // Delete the related person node from the intelligence engine
        // This will also remove the link (DETACH DELETE)
        await intelligenceEngine.DeleteNodeAsync(@event.RelatedPersonId);
    }
}
