namespace Client.Core.Campaigns;

public interface ICampaignGraphNode<TValueData>
{
    public TValueData Value { get; }
}