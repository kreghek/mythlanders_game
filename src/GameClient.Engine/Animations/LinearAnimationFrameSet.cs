using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

public sealed class LinearAnimationFrameSet : IAnimationFrameSet
{
    private readonly float _fps;
    private readonly int _frameHeight;

    private readonly IReadOnlyList<int> _frames;

    private readonly int _frameWidth;
    private readonly int _textureColumns;

    private double _frameCounter;
    private int _frameListIndex;
    private bool _isEnded;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="frames">Numbers of frames in sprite sheet.</param>
    /// <param name="fps"></param>
    /// <param name="frameWidth"></param>
    /// <param name="frameHeight"></param>
    /// <param name="textureColumns"></param>
    /// <exception cref="ArgumentException"></exception>
    public LinearAnimationFrameSet(IReadOnlyList<int> frames, float fps, int frameWidth,
        int frameHeight, int textureColumns)
    {
        _frames = frames;

        if (!_frames.Any())
        {
            throw new ArgumentException("Frames must be not empty", nameof(frames));
        }

        _fps = fps;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _textureColumns = textureColumns;
    }

    /// <summary>
    /// Make animation to play in infinite cycle.
    /// </summary>
    public bool IsLooping { get; init; }

    private static Rectangle CalcRect(int frameIndex, int cols, int frameWidth, int frameHeight)
    {
        var col = frameIndex % cols;
        var row = frameIndex / cols;
        return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
    }

    /// <inheritdoc />
    public bool IsIdle { get; init; }

    /// <inheritdoc />
    public Rectangle GetFrameRect()
    {
        return CalcRect(_frames[_frameListIndex], _textureColumns, _frameWidth, _frameHeight);
    }

    /// <inheritdoc />
    public void Reset()
    {
        _frameCounter = 0;
        _frameListIndex = 0;
        _isEnded = false;
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (_isEnded)
        {
            return;
        }

        _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
        if (_frameCounter > 1 / _fps)
        {
            _frameCounter = 0;
            _frameListIndex++;

            if (_frameListIndex > _frames.Count - 1)
            {
                if (IsLooping)
                {
                    _frameListIndex = 0;
                    KeyFrame?.Invoke(this, new AnimationFrameEventArgs(0));
                }
                else
                {
                    _frameListIndex = _frames.Count - 1;
                    _isEnded = true;
                    End?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                KeyFrame?.Invoke(this, new AnimationFrameEventArgs(_frameListIndex));
            }
        }
    }

    /// <inheritdoc />
    public event EventHandler? End;
    
    /// <inheritdoc />
    public event EventHandler<AnimationFrameEventArgs>? KeyFrame;
}