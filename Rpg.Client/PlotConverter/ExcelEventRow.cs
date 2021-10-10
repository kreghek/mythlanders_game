namespace PlotConverter
{
    internal sealed record ExcelEventRow
    {
        public string Sid { get; init; }
        public string Name { get; init; }
        public string Location { get; init; }
        public string Aftermath { get; init; }
    }
}
