namespace Rpg.Client.Core
{
    internal sealed class DialogOption
    {
        public IOptionAftermath Aftermath { get; init; }
        public bool IsEnd { get; init; }
        public DialogNode Next { get; init; }
        public string Text { get; init; }
    }
}