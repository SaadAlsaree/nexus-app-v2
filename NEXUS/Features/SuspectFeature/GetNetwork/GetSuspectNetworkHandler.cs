using Neo4j.Driver;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.GetNetwork;

public class GetSuspectNetworkHandler
{
    private readonly IDriver _driver;

    public GetSuspectNetworkHandler(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<Response<NetworkGraphDto>> Handle(GetSuspectNetworkQuery query)
    {
        // استخدام OPTIONAL MATCH يضمن عودة المتهم حتى لو كان وحيداً
        var cypherQuery = @"
        MATCH (center:Suspect {id: $id})
        OPTIONAL MATCH (center)-[r]-(target)
        RETURN center, r, target";

        using var session = _driver.AsyncSession();
        var cursor = await session.RunAsync(cypherQuery, new { id = query.SuspectId.ToString() });

        var nodesDict = new Dictionary<string, NetworkNode>();
        var links = new List<NetworkLink>();

        while (await cursor.FetchAsync())
        {
            // 1. المتهم الرئيسي دائماً موجود
            var center = cursor.Current["center"].As<INode>();
            AddNode(nodesDict, center, "1"); // Group 1 للمركز

            // 2. نفحص هل يوجد علاقة أم أن القيم Null؟
            // في حال استخدام OPTIONAL MATCH، إذا لم توجد علاقة ستكون r و target بـ null
            var relObj = cursor.Current["r"];
            var targetObj = cursor.Current["target"];

            if (relObj != null && targetObj != null)
            {
                var rel = relObj.As<IRelationship>();
                var target = targetObj.As<INode>();

                // نضيف الهدف والرابط فقط إذا وُجدا
                AddNode(nodesDict, target, "2"); // Group 2 للأطراف

                links.Add(new NetworkLink(
                    Source: GetProp(center, "id"),
                    Target: GetProp(target, "id"),
                    Label: rel.Type,
                    Type: rel.Properties.ContainsKey("type") ? rel.Properties["type"].As<string>() : "Direct"
                ));
            }
        }

        return Response<NetworkGraphDto>.Success(new NetworkGraphDto(nodesDict.Values.ToList(), links));
    }

    // تحديث بسيط لدالة الإضافة لتستقبل المجموعة
    private void AddNode(Dictionary<string, NetworkNode> dict, INode node, string group)
    {
        var id = GetProp(node, "id");
        if (!dict.ContainsKey(id))
        {
            var label = node.Properties.ContainsKey("fullName") ? node.Properties["fullName"].As<string>() : "Unknown";
            var type = node.Labels.FirstOrDefault() ?? "Unknown";

            dict[id] = new NetworkNode(id, label, type, group);
        }
    }

    private string GetProp(INode node, string key) =>
        node.Properties.ContainsKey(key) ? node.Properties[key].As<string>() : string.Empty;
}