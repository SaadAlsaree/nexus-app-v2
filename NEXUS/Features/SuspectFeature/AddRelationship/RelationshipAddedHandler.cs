using NEXUS.Services;

namespace NEXUS.Features.SuspectFeature.AddRelationship;

public sealed class RelationshipAddedHandler(IIntelligenceEngine intelligenceEngine)
{
    public async Task Handle(RelationshipAddedEvent @event)
    {
        // 1. Ensure the referenced person exists as a node (light node if not a suspect)
        // We'll create a "Person" node for the relation.
        // In a real system, we'd check if this person is already a Suspect to merge nodes.
        // For now, simpler logic:

        var relatedPersonProps = new Dictionary<string, object>
        {
            { "Id", @event.RelatedPersonId },
            { "Type", "RelatedPerson" } 
            // We might want to fetch names here, but for now we just link IDs
        };

        // Create/Update the related person node
        await intelligenceEngine.SyncNodeAsync(@event.RelatedPersonId, "RelatedPerson", relatedPersonProps);

        // 2. Create the link
        await intelligenceEngine.CreateLinkAsync(@event.SuspectId, @event.RelatedPersonId, @event.Relationship.ToString());
    }
}
