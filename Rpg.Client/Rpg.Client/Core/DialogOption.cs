namespace Rpg.Client.Core
{
    internal sealed class DialogOption
    {
        public string Text { get; init; }
        public DialogNode Next { get; init; }
        public bool IsEnd { get; init; }
        public IOptionAftermath Aftermath { get; init; }
    }
}
