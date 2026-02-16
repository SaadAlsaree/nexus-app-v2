namespace NEXUS.Features.SuspectFeature.GetNetwork;

public record NetworkGraphDto(List<NetworkNode> Nodes, List<NetworkLink> Links);
public record NetworkNode(string Id, string Label, string Type, string Group);
public record NetworkLink(string Source, string Target, string Label, string Type);