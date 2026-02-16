using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using NEXUS.Data;
using NEXUS.Data.Enums;
using NEXUS.Dtos;

namespace NEXUS.Services;

public class IntelligenceEngine : IIntelligenceEngine
{
    private readonly IDriver _neo4jDriver;
    private readonly AppDbContext _sqlContext;
    private readonly ILogger<IntelligenceEngine> _logger;

    // تم إضافة "Event" للقائمة المسموحة
    private readonly HashSet<string> _allowedLabels = new()
    {
        "Suspect", "Phone", "Location", "Weapon", "Vehicle", "Case", "Organization", "Event"
    };

    public IntelligenceEngine(
        IDriver neo4jDriver,
        AppDbContext sqlContext,
        ILogger<IntelligenceEngine> logger)
    {
        _neo4jDriver = neo4jDriver;
        _sqlContext = sqlContext;
        _logger = logger;
    }

    // =================================================================
    // 1. عمليات الغراف الأساسية (Core Graph Operations)
    // =================================================================

    public async Task SyncNodeAsync(Guid entityId, string nodeType, Dictionary<string, object> properties)
    {
        if (!_allowedLabels.Contains(nodeType)) throw new ArgumentException($"Invalid node type: {nodeType}");

        properties["id"] = entityId.ToString();
        properties["last_updated"] = DateTime.UtcNow.ToString("O");

        var query = $@"MERGE (n:{nodeType} {{id: $id}}) SET n += $props";

        using var session = _neo4jDriver.AsyncSession();
        await session.RunAsync(query, new { id = entityId.ToString(), props = properties });
    }

    // [جديد] دالة لربط التفاصيل الاستراتيجية (قضية، موقع، دورات)
    public async Task SyncSuspectDetailsAsync(Guid suspectId, SuspectDetailsDto details)
    {
        using var session = _neo4jDriver.AsyncSession();

        // 1. ربط بالقضية (Case)
        if (!string.IsNullOrEmpty(details.CaseNumber))
        {
            var caseQuery = @"
                MATCH (s:Suspect {id: $id})
                MERGE (c:Case {number: $caseNo})
                MERGE (s)-[:INVOLVED_IN]->(c)";
            await session.RunAsync(caseQuery, new { id = suspectId.ToString(), caseNo = details.CaseNumber });
        }

        // 2. ربط بالموقع (Location - Governorate)
        if (!string.IsNullOrEmpty(details.Governorate))
        {
            var locQuery = @"
                MATCH (s:Suspect {id: $id})
                MERGE (l:Location {name: $gov, type: 'Governorate'})
                MERGE (s)-[:LIVES_IN]->(l)";
            await session.RunAsync(locQuery, new { id = suspectId.ToString(), gov = details.Governorate });
        }

        // 3. ربط بالدورات والعمليات (Events)
        if (details.HistoryEvents != null && details.HistoryEvents.Any())
        {
            foreach (var evt in details.HistoryEvents)
            {
                var eventQuery = @"
                    MATCH (s:Suspect {id: $id})
                    MERGE (e:Event {name: $evtName, type: $evtType})
                    MERGE (s)-[:PARTICIPATED_IN]->(e)";

                await session.RunAsync(eventQuery, new
                {
                    id = suspectId.ToString(),
                    evtName = evt.Name,
                    evtType = evt.Type // 'Course' or 'Operation'
                });
            }
        }
    }

    public async Task CreateLinkAsync(Guid fromId, Guid toId, string relationshipType, Dictionary<string, object>? props = null)
    {
        var query = $@"
                MATCH (a {{id: $fromId}}), (b {{id: $toId}})
                MERGE (a)-[r:{relationshipType}]->(b)
                SET r += $props, r.created_at = datetime()";

        using var session = _neo4jDriver.AsyncSession();
        await session.RunAsync(query, new { fromId = fromId.ToString(), toId = toId.ToString(), props = props ?? new() });
    }

    public async Task DeleteNodeAsync(Guid entityId)
    {
        var query = "MATCH (n {id: $id}) DETACH DELETE n";
        try
        {
            using var session = _neo4jDriver.AsyncSession();
            await session.RunAsync(query, new { id = entityId.ToString() });
            _logger.LogInformation($"Deleted Node: {entityId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete node {entityId}");
            throw;
        }
    }

    // =================================================================
    // 2. عمليات الاستدلال الذكي (AI Inference)
    // =================================================================

    public async Task InferRelationshipsAsync(Guid suspectId)
    {
        var idParam = new { id = suspectId.ToString() };

        var siblingQuery = @"
            MATCH (new:Suspect {id: $id})
            MATCH (existing:Suspect)
            WHERE new.id <> existing.id
              AND new.fatherName = existing.fatherName
              AND new.grandFatherName = existing.grandFatherName
              AND new.tribe = existing.tribe
            MERGE (new)-[:POTENTIAL_SIBLING {confidence: 'High'}]-(existing)";

        var localityQuery = @"
            MATCH (new:Suspect {id: $id})
            MATCH (existing:Suspect)
            WHERE new.id <> existing.id
              AND new.placeOfBirth <> '' AND existing.placeOfBirth <> ''
              AND new.placeOfBirth = existing.placeOfBirth
              AND new.tribe = existing.tribe
            MERGE (new)-[:SAME_ORIGIN {location: new.placeOfBirth}]-(existing)";

        var maternalQuery = @"
            MATCH (new:Suspect {id: $id})
            MATCH (existing:Suspect)
            WHERE new.id <> existing.id
              AND new.motherName <> '' AND existing.motherName <> ''
              AND new.motherName = existing.motherName
              AND new.fatherName <> existing.fatherName
            MERGE (new)-[:MATERNAL_SIBLING {mother: new.motherName}]-(existing)";

        var isChildQuery = @"
            MATCH (child:Suspect {id: $id})
            MATCH (father:Suspect)
            WHERE child.id <> father.id
              AND child.fatherName = father.firstName
              AND child.grandFatherName = father.fatherName
              AND child.tribe = father.tribe
            MERGE (father)-[:PARENT_OF {confidence: 'High', type: 'Inferred'}]->(child)";

        var isParentQuery = @"
            MATCH (father:Suspect {id: $id})
            MATCH (child:Suspect)
            WHERE father.id <> child.id
              AND child.fatherName = father.firstName
              AND child.grandFatherName = father.fatherName
              AND child.tribe = father.tribe
            MERGE (father)-[:PARENT_OF {confidence: 'High', type: 'Inferred'}]->(child)";

        using var session = _neo4jDriver.AsyncSession();

        try
        {
            await Task.WhenAll(
                session.RunAsync(siblingQuery, idParam),
                session.RunAsync(localityQuery, idParam),
                session.RunAsync(maternalQuery, idParam),
                session.RunAsync(isChildQuery, idParam),
                session.RunAsync(isParentQuery, idParam)
            );

            _logger.LogInformation($"Inference completed for Suspect {suspectId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running inference queries");
            throw;
        }
    }

    // =================================================================
    // 3. عمليات التحليل والاستعلام (Advanced Analysis & Search)
    // =================================================================

    // [جديد] البحث عن المتهمين حسب القضية
    public async Task<List<SuspectDto>> GetSuspectsByCaseAsync(string caseNumber)
    {
        var query = @"
            MATCH (c:Case {number: $caseNumber})<-[:INVOLVED_IN]-(s:Suspect)
            RETURN s.id, s.fullName, s.status, s.riskLevel";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { caseNumber });
        return await MapCursorToSuspects(cursor);
    }

    // [جديد] البحث عن المتهمين حسب الولاية/المحافظة
    public async Task<List<SuspectDto>> GetSuspectsByLocationAsync(string governorate)
    {
        var query = @"
            MATCH (l:Location {name: $gov})<-[:LIVES_IN]-(s:Suspect)
            RETURN s.id, s.fullName, s.status, s.riskLevel";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { gov = governorate });
        return await MapCursorToSuspects(cursor);
    }

    // [جديد] البحث عن رفقاء المسار (دورات، عمليات مشتركة)
    public async Task<List<SuspectDto>> GetCohortsAsync(string eventName, string eventType)
    {
        var query = @"
            MATCH (e:Event {name: $name, type: $type})<-[:PARTICIPATED_IN]-(s:Suspect)
            RETURN s.id, s.fullName, s.status, s.riskLevel";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { name = eventName, type = eventType });
        return await MapCursorToSuspects(cursor);
    }

    public async Task<AnalysisReportDto> AnalyzeNewEntryAsync(string value, string type)
    {
        var watchlistMatch = await _sqlContext.EntityWatchlists
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Keyword == value && w.IsActive);

        if (watchlistMatch != null)
        {
            return new AnalysisReportDto(
                IsFlagged: true,
                Message: $"تطابق مع قائمة المراقبة: {watchlistMatch.Reason}",
                AlertLevel: watchlistMatch.AlertLevel,
                Source: "Watchlist",
                Recommendation: "يرجى التحقق الفوري وتجميد النشاط."
            );
        }

        var query = @"
            MATCH (e {value: $val})<-[]-(s:Suspect)
            WHERE s.currentStatus IN ['Sentenced', 'Fugitive'] OR s.riskLevel >= 4
            RETURN s.fullName, s.currentStatus
            LIMIT 1";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { val = value });

        if (await cursor.FetchAsync())
        {
            var name = cursor.Current["s.fullName"].As<string>();
            var status = cursor.Current["s.currentStatus"].As<string>();

            return new AnalysisReportDto(
                IsFlagged: true,
                Message: $"هذه المعلومة مرتبطة بالمتهم الخطر ({name}) وحالته ({status})",
                AlertLevel: AlertLevel.High,
                Source: "Graph Intelligence",
                Recommendation: "يجب التحقيق في طبيعة العلاقة فوراً."
            );
        }

        return new AnalysisReportDto(false, "No threats found", AlertLevel.Low, "None", "Safe");
    }

    public async Task<List<SuspectDto>> FindWhoMentionedEntityAsync(string entityValue, Guid excludeSuspectId)
    {
        var query = @"
            MATCH (target {value: $val})<-[r]-(other:Suspect)
            WHERE other.id <> $excludeId
            RETURN other.id, other.fullName, other.currentStatus, other.riskLevel";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { val = entityValue, excludeId = excludeSuspectId.ToString() });

        // استخدام دالة المساعدة الجديدة
        return await MapCursorToSuspects(cursor);
    }

    public async Task<List<PathResultDto>> FindBridgesAsync(Guid entityA, Guid entityB)
    {
        var query = @"
            MATCH (start {id: $id1}), (end {id: $id2})
            MATCH path = allShortestPaths((start)-[*..6]-(end))
            RETURN [n in nodes(path) | COALESCE(n.fullName, n.value, labels(n)[0])] as nodeNames,
                   length(path) as len";

        using var session = _neo4jDriver.AsyncSession();
        var cursor = await session.RunAsync(query, new { id1 = entityA.ToString(), id2 = entityB.ToString() });

        var paths = new List<PathResultDto>();

        while (await cursor.FetchAsync())
        {
            var names = cursor.Current["nodeNames"].As<List<string>>();
            var length = cursor.Current["len"].As<int>();

            paths.Add(new PathResultDto(
                PathDescription: string.Join(" -> ", names),
                Length: length,
                Nodes: names
            ));
        }

        return paths;
    }

    // =================================================================
    // 4. دوال مساعدة (Helper Methods)
    // =================================================================

    private async Task<List<SuspectDto>> MapCursorToSuspects(IResultCursor cursor)
    {
        var result = new List<SuspectDto>();
        while (await cursor.FetchAsync())
        {
            // ملاحظة: تأكد من أن الـ DTO الخاص بك يدعم هذه الحقول
            // قمت باستخدام قيم افتراضية للحقول غير المطلوبة في البحث السريع
            result.Add(new SuspectDto(
                Id: Guid.Parse(cursor.Current.ContainsKey("s.id") ? cursor.Current["s.id"].As<string>() : cursor.Current["other.id"].As<string>()),
                FullName: cursor.Current.ContainsKey("s.fullName") ? cursor.Current["s.fullName"].As<string>() : cursor.Current["other.fullName"].As<string>(),
                Kunya: null,
                Status: cursor.Current.ContainsKey("s.status") ? cursor.Current["s.status"].As<string>() : cursor.Current.ContainsKey("other.currentStatus") ? cursor.Current["other.currentStatus"].As<string>() : "Unknown",
                CurrentLocation: "Unknown",
                MainCaseNumber: null,
                RiskLevel: cursor.Current.ContainsKey("s.riskLevel") ? cursor.Current["s.riskLevel"].As<int>() : cursor.Current.ContainsKey("other.riskLevel") ? cursor.Current["other.riskLevel"].As<int>() : 0
            ));
        }
        return result;
    }


    public async Task LinkSuspectToCaseAsync(Guid suspectId, string caseNumber, string role, string caseStatus)
    {
        // المنطق:
        // 1. نجد المتهم (MATCH)
        // 2. نجد القضية أو ننشئها إذا لم تكن موجودة (MERGE Case)
        // 3. ننشئ العلاقة ونضع فيها صفة المتهم (MERGE Relationship)

        var query = @"
        MATCH (s:Suspect {id: $id})
        MERGE (c:Case {number: $caseNum})
        ON CREATE SET c.status = $status, c.created_at = datetime()
        ON MATCH SET c.status = $status // تحديث حالة القضية لو كانت موجودة
        
        MERGE (s)-[r:INVOLVED_IN]->(c)
        SET r.role = $role,            // تخزين الدور داخل العلاقة نفسها
            r.linked_at = datetime()";

        using var session = _neo4jDriver.AsyncSession();

        await session.RunAsync(query, new
        {
            id = suspectId.ToString(),
            caseNum = caseNumber,
            role = role,
            status = caseStatus
        });

        _logger.LogInformation($"Linked Suspect {suspectId} to Case {caseNumber} as {role}");
    }
}