using System.Collections.Generic;

namespace Client.Core.Campaigns;

public interface ICampaignGraph<TValueData>
{
    IReadOnlyCollection<ICampaignGraphNode<TValueData>> GetAllNodes();
    IReadOnlyCollection<ICampaignGraphNode<TValueData>> GetNext(ICampaignGraphNode<TValueData> node);
    void AddNode(ICampaignGraphNode<TValueData> node);
    void ConnectNodes(ICampaignGraphNode<TValueData> sourceNode, ICampaignGraphNode<TValueData> targetNode);
}