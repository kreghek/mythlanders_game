using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameComponents;

internal class SoundtrackManagerComponent : GameComponent
{
    private SoundtrackManager? _soundtrackManager;

    public SoundtrackManagerComponent(Game game) : base(game)
    {
    }

    public void Initialize(SoundtrackManager soundtrackManager)
    {
        _soundtrackManager = soundtrackManager;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_soundtrackManager is not null)
        {
            _soundtrackManager.Update();
        }
    }
}