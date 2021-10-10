namespace PlotConverter
{
    internal sealed record ExcelTextFragmentRow
    {
        public string EventSid { get; init; }
        public string Speaker { get; init; }
        public string Text { get; init; }
        public int Index { get; init; }
        public int CombatPosition { get; init; }
    }

    internal sealed record ExcelEventRow
    {
        public string Sid { get; init; }
        public string Name { get; init; }
        public string Location { get; init; }
        public string Aftermaths { get; init; }
    }
}
