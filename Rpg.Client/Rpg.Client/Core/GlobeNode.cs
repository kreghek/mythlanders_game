namespace Rpg.Client.Core
{
    public class GlobeNode
    {
        public Dialog? AvailableDialog { get; set; }
        public Combat? Combat { get; set; }

        public int Index { get; internal set; }

        public string Name { get; set; }
    }
}