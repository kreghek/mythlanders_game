namespace Client.Core.EventSerialization;

internal sealed record EventNodeStorageModel
{
    public EventTextFragmentStorageModel[] Fragments { get; set; }
}