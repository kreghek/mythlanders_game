using Client.Assets.InteractionDeliveryObjects;
using Client.Engine.PostProcessing;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;

using GameClient.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.ActorVisualizationStates.HeroSpecific.Primitives;

internal sealed class SvarogSymbolBurningState : IActorVisualizationState
{
    private const double STATE_DURATION_SECONDS = 3f;
    private const double SHAKEING_DURATION_SECONDS = STATE_DURATION_SECONDS;
    private readonly UnitGraphics _graphics;
    private readonly PostEffectManager _postEffectManager;

    private readonly SoundEffectInstance _risingPowerSoundEffect;
    private bool _isStarted;

    public SvarogSymbolBurningState(UnitGraphics graphics, SvarogSymbolObject svarogSymbol,
        PostEffectManager postEffectManager, SoundEffectInstance risingPowerSoundEffect)
    {
        _graphics = graphics;
        _postEffectManager = postEffectManager;
        _risingPowerSoundEffect = risingPowerSoundEffect;

        svarogSymbol.RisingPowerCompleted += (_, _) =>
        {
            IsComplete = true;
            // 2 stage is exposion!
            svarogSymbol.SwitchStage(2);
        };
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        // Nothing to cancel
    }

    public void Update(GameTime gameTime)
    {
        if (!_isStarted)
        {
            //_graphics.PlayAnimation(PredefinedAnimationSid.Ult);
            _isStarted = true;
            var effect = new TimeLimitedShakePostEffect(new Duration(SHAKEING_DURATION_SECONDS), new FadeOutShakeFunction(ShakePowers.Normal));
            _postEffectManager.AddEffect(effect);
            _risingPowerSoundEffect.Play();
        }
    }
}