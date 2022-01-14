namespace PlotConverter
{
    internal sealed record ExcelEventRow
    {
        public string AfterCombatAftermath { get; init; }
        public string BeforeCombatAftermath { get; init; }
        public string GoalDescription { get; init; }
        public string Location { get; init; }
        public string Name { get; init; }
        public string ParentSids { get; init; }
        public string Sid { get; init; }
    }
}