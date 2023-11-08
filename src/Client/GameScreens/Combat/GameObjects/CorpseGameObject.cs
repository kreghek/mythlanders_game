using Client.Core;
using Client.Engine;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CorpseGameObject
{
    private readonly UnitGraphics _graphics;
    private readonly IAnimationFrameSet _deathAnimation;
    private double _counter;

    private bool _startToDeath;
    private bool _startToWound;

    public CorpseGameObject(UnitGraphics graphics, IAnimationFrameSet deathAnimation)
    {
        _graphics = graphics;
        _deathAnimation = deathAnimation;
    }

    public bool IsComplete => _counter > 2;

    public void Update(GameTime gameTime)
    {
        _graphics.Update(gameTime);

        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_counter > 0.05)
        {
            _graphics.IsDamaged = false;
        }

        if (_counter > 1 && !_startToDeath)
        {
            _startToDeath = true;
            _graphics.IsDamaged = false;
            _graphics.PlayAnimation(PredefinedAnimationSid.Death);
        }

        if (!_startToWound)
        {
            _graphics.PlayAnimation(PredefinedAnimationSid.Wound);
            _startToWound = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphics.Draw(spriteBatch);
    }

    internal float GetZIndex()
    {
        return _graphics.Root.RootNode.Position.Y;
    }
}