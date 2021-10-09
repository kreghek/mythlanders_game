namespace PlotConverter
{
    internal sealed record EventDto
    {
        public string Name { get; set; }

        public EventNodeDto BeforeCombatNode { get; set; }
        public EventNodeDto AfterCombatNode { get; set; }
    }

    internal sealed record EventNodeDto
    { 
        public string Speaker { get; set; }
        public string Text { get; set; }
        public EventNodeDto NextNode { get; set; }
    }
}
