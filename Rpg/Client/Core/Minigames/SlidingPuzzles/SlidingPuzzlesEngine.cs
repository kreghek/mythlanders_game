namespace Client.Core.Minigames.BarleyBreak
{
    internal sealed class SlidingPuzzlesEngine
    {
        private readonly SlidingPuzzlesMatrix _matrix;

        public int TurnCounter { get; private set; }

        public SlidingPuzzlesEngine(SlidingPuzzlesMatrix startMatrix)
        {
            _matrix = startMatrix;

            for (int x = 0; x < _matrix.Width; x++)
            {
                for (int y = 0; y < _matrix.Height; y++)
                {
                    if (_matrix[x, y] == 0)
                    {
                        _currentPosition = (x, y);
                    }
                }
            }
        }

        public bool TryMove(int x, int y)
        {
            var emptyPositionOffsets = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

            for (int i = 0; i < emptyPositionOffsets.Length; i++)
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
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public bool IsCompleted
        {
            get
            {
                for (int x = 0; x < _matrix.Width; x++)
                {
                    for (int y = 0; y < _matrix.Height; y++)
                    {
                        if (_matrix[x, y] == (x + y * _matrix.Width + 1))
                        { 
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
