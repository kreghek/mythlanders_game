namespace PlotConverter
{
    internal sealed record ExcelRow
    {
        public string Event { get; init; }
        public string Speaker { get; init; }
        public string Text { get; init; }
        public int Index { get; init; }
        public int CombatPosition { get; init; }
    }
}
