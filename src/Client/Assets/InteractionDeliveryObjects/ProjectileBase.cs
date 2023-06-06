using System;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.InteractionDeliveryObjects;

internal abstract class ProjectileBase : IInteractionDelivery
{
    private readonly IAnimationFrameSet _frameSet;
    private readonly ProjectileFunctions _functions;
    private readonly Sprite _graphics;
    private readonly double _lifetimeDuration;

    private double _lifetimeCounter;

    protected ProjectileBase(
        ProjectileFunctions functions,
        Texture2D texture,
        IAnimationFrameSet frameSet,
        double lifetimeDuration)
    {
        _graphics = new Sprite(texture)
        {
            Position = functions.MoveFunction.CalcPosition(0),
            SourceRectangle = new Rectangle(0, 0, 1, 1),
            Rotation = functions.RotationFunction.CalculateRadianAngle(0)
        };

        _functions = functions;
        _lifetimeDuration = lifetimeDuration;

        _frameSet = frameSet;
    }

    protected Vector2 CurrentPosition
    {
        get
        {
            var t = _lifetimeCounter / _lifetimeDuration;
            return _functions.MoveFunction.CalcPosition(t);
        }
    }

    protected virtual void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch) { }

    protected virtual void UpdateAdditionalEffects(GameTime gameTime) { }

    public event EventHandler? InteractionPerformed;

    /// <inheritdoc />
    public bool IsDestroyed { get; private set; }

    /// <inheritdoc />
    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsDestroyed)
        {
            return;
        }

        _graphics.Draw(spriteBatch);

        DrawForegroundAdditionalEffects(spriteBatch);
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        _frameSet.Update(gameTime);
        _graphics.SourceRectangle = _frameSet.GetFrameRect();

        if (_lifetimeCounter < _lifetimeDuration)
        {
            _lifetimeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            _graphics.Position = CurrentPosition;
            var t = _lifetimeCounter / _lifetimeDuration;
            _graphics.Rotation = _functions.RotationFunction.CalculateRadianAngle((float)t);
        }
        else
        {
            if (!IsDestroyed)
            {
                IsDestroyed = true;
                InteractionPerformed?.Invoke(this, EventArgs.Empty);
            }
        }

        UpdateAdditionalEffects(gameTime);
    }
}