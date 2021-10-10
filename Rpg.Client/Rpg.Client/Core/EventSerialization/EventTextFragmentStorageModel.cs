namespace Rpg.Client.Core.EventSerialization
{
    internal sealed record EventTextFragmentStorageModel
    {
        public string Speaker { get; set; }
        public string Text { get; set; }
    }
}