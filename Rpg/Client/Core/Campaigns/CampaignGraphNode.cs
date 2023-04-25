namespace Client.Core.Campaigns;

internal sealed class CampaignGraphNode<TValueData> : ICampaignGraphNode<TValueData>
{
    public CampaignGraphNode(TValueData data)
    {
        Value = data;
    }

    public TValueData Value { get; }
}