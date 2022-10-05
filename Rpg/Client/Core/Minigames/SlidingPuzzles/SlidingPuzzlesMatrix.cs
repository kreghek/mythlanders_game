namespace Client.Core.Minigames.BarleyBreak
{
    internal sealed class SlidingPuzzlesMatrix
    {
        private readonly int[,] _values;

        public SlidingPuzzlesMatrix(int[,] values)
        {
            _values = values;
            Width = values.GetLength(0);
            Height = values.GetLength(1);
        }

        public int Width { get; }
        public int Height { get; }

        public int this[int x, int y] { get => _values[x, y]; set { _values[x, y] = value; } }
    }
}
