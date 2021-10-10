namespace PlotConverter
{
    internal sealed record EventNodeDto
    { 
        public string Speaker { get; set; }
        public string Text { get; set; }
        public EventNodeDto NextNode { get; set; }
    }
}
