namespace PlotConverter
{
    internal sealed record EventDto
    {
        public EventNodeDto AfterCombatNode { get; set; }
        public string Aftermath { get; set; }

        public EventNodeDto BeforeCombatNode { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Sid { get; set; }
    }
}