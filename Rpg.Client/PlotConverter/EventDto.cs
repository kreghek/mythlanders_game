namespace PlotConverter
{
    internal sealed record EventDto
    {
        public string Location { get; set; }
        public string Name { get; set; }

        public EventNodeDto BeforeCombatNode { get; set; }
        public EventNodeDto AfterCombatNode { get; set; }
    }
}
