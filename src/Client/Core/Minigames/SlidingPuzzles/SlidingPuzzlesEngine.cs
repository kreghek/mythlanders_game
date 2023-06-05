namespace Client.Core.Minigames.BarleyBreak
{
    internal sealed class SlidingPuzzlesEngine
    {
        private readonly SlidingPuzzlesMatrix _matrix;

        public SlidingPuzzlesEngine(SlidingPuzzlesMatrix startMatrix)
        {
            _matrix = startMatrix;
        }

        public bool IsCompleted
        {
            get
            {
                for (var x = 0; x < _matrix.Width; x++)
                {
                    for (var y = 0; y < _matrix.Height; y++)
                    {
                        if (x == _matrix.Width - 1 && y == _matrix.Height - 1)
                        {
                            if (_matrix[x, y] != 0)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (_matrix[x, y] != (x + y * _matrix.Width + 1))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
        }

        public int TurnCounter { get; private set; }

        public bool TryMove(int x, int y)
        {
            var emptyPositionOffsets = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

            for (var i = 0; i < emptyPositionOffsets.Length; i++)
            {
                var testedX = emptyPositionOffsets[i].Item1 + x;
                var testedY = emptyPositionOffsets[i].Item2 + y;

                if (testedX >= 0 && testedX <= _matrix.Width - 1
                                 && testedY >= 0 && testedY <= _matrix.Height - 1)
                {
                    if (_matrix[testedX, testedY] == 0)
                    {
                        var nValue = _matrix[x, y];
                        _matrix[testedX, testedY] = nValue;
                        _matrix[x, y] = 0;

                        TurnCounter++;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}