using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CorpseGameObject : EwarRenderableBase
{
    private readonly UnitGraphics _graphics;
    private double _counter;

    private bool _startToDeath;
    private bool _startToWound;

    public CorpseGameObject(UnitGraphics graphics)
    {
        _graphics = graphics;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

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

    protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
    {
        base.DoDraw(spriteBatch, zindex);

        _graphics.Draw(spriteBatch);
    }

    internal float GetZIndex()
    {
        return _graphics.Root.RootNode.Position.Y;
    }

    public bool IsComplete => _counter > 2;
}