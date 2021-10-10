namespace PlotConverter
{
    internal sealed record ExcelTextFragmentRow
    {
        public int CombatPosition { get; init; }
        public string EventSid { get; init; }
        public int Index { get; init; }
        public string Speaker { get; init; }
        public string Text { get; init; }
    }
}