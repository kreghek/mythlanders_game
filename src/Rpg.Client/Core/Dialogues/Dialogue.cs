namespace Rpg.Client.Core.Dialogues
{
    internal sealed class Dialogue
    {
        public Dialogue(EventNode root, EventPosition combatPosition)
        {
            Root = root;
            CombatPosition = combatPosition;
        }

        public EventPosition CombatPosition { get; }

        public EventNode Root { get; }
    }
}