namespace PlotConverter
{
    internal sealed record EventDto
    {
        public EventNodeDto AfterCombatNode { get; set; }
        public string BeforeCombatAftermath { get; set; }

        public string AfterCombatAftermath { get; set; }

        public EventNodeDto BeforeCombatNode { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Sid { get; set; }

        public string GoalDescription { get; set; }
        public string[] ParentSids { get; set; }
    }
}