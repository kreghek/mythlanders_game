namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public GlobeNode(string name)
        {
            Name = name;
        }

        public Event? AvailableDialog { get; set; }

        public CombatSequence? CombatSequence { get; set; }

        public int Index { get; internal set; }

        public string Name { get; }
    }
}