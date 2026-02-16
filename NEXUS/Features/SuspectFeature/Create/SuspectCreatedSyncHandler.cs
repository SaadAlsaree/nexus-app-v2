using NEXUS.Helpers;
using NEXUS.Services;

namespace NEXUS.Features.SuspectFeature.Create;

public class SuspectCreatedSyncHandler
{
    private readonly IIntelligenceEngine _intelligenceEngine;
    private readonly ILogger<SuspectCreatedSyncHandler> _logger;

    public SuspectCreatedSyncHandler(IIntelligenceEngine intelligenceEngine, ILogger<SuspectCreatedSyncHandler> logger)
    {
        _intelligenceEngine = intelligenceEngine;
        _logger = logger;
    }

    public async Task Handle(Events.SuspectCreatedEvent @event)
    {
        _logger.LogInformation($"Starting Neo4j Sync for Suspect: {@event.FullName} ({@event.Id})");

        try
        {
            // 1. المزامنة (حفظ البيانات الخام)
            // قمت بإضافة Trim() هنا لضمان نظافة البيانات عند المقارنة
            await _intelligenceEngine.SyncNodeAsync(@event.Id, "Suspect",
                new Dictionary<string, object>
                    {
                        // تنظيف الاسم الكامل
                        { "fullName", ArabicTextHelper.Normalize(@event.FullName) },
                        { "status", @event.Status.ToString() },
            
                        // تنظيف الأجزاء (مهم جداً للربط العائلي)
                        { "firstName", ArabicTextHelper.Normalize(@event.FirstName) },
                        { "fatherName", ArabicTextHelper.Normalize(@event.SecondName) },
                        { "grandFatherName", ArabicTextHelper.Normalize(@event.ThirdName) },
                        { "tribe", ArabicTextHelper.Normalize(@event.Tribe) },
                        { "motherName", ArabicTextHelper.Normalize(@event.MotherName ?? "") },
                        { "placeOfBirth", ArabicTextHelper.Normalize(@event.PlaceOfBirth ?? "") }
                    }
             );

            _logger.LogInformation("Node created. Starting Intelligence Inference...");

            await _intelligenceEngine.InferRelationshipsAsync(@event.Id);

            _logger.LogInformation("Neo4j Sync & Inference Completed Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync suspect to Neo4j.");
            throw;
        }
    }
}