using System.Collections.Generic;
using System.Linq;

namespace Client.Core.Campaigns;

internal sealed class CampaignGraph<TValueData>: ICampaignGraph<TValueData>
{
    private readonly IDictionary<ICampaignGraphNode<TValueData>, IList<ICampaignGraphNode<TValueData>>> _dict;

    public CampaignGraph()
    {
        _dict = new Dictionary<ICampaignGraphNode<TValueData>, IList<ICampaignGraphNode<TValueData>>>();
    }

    public void AddNode(ICampaignGraphNode<TValueData> node)
    {
        var next = new List<ICampaignGraphNode<TValueData>>();
        _dict[node] = next;
    }

    public void ConnectNodes(ICampaignGraphNode<TValueData> sourceNode, ICampaignGraphNode<TValueData> targetNode)
    {
        if (!_dict.TryGetValue(sourceNode, out var next))
        {
            next = new List<ICampaignGraphNode<TValueData>>();
            _dict[sourceNode] = next;
        }
        
        next.Add(targetNode);
    }

    public IReadOnlyCollection<ICampaignGraphNode<TValueData>> GetAllNodes()
    {
        return _dict.Keys.ToArray();
    }

    public IReadOnlyCollection<ICampaignGraphNode<TValueData>> GetNext(ICampaignGraphNode<TValueData> node)
    {
        return _dict[node].ToArray();
    }
}