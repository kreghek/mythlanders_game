using System.Collections.Generic;

using Client.Assets.InteractionDeliveryObjects;
using Client.Core;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.States.HeroSpecific.Primitives;

internal sealed class SvarogSymbolAppearingState : IActorVisualizationState
{
    private readonly UnitGraphics _graphics;
    private readonly IList<IInteractionDelivery> _interactionDeliveryList;
    private readonly SvarogSymbolObject _svarogSymbol;
    private readonly SoundEffectInstance? _symbolAppearingSoundEffect;
    private bool _started;

    public SvarogSymbolAppearingState(UnitGraphics graphics,
        SvarogSymbolObject svarogSymbol,
        IList<IInteractionDelivery> interactionDeliveryList,
        SoundEffectInstance symbolAppearingSoundEffect)
    {
        _graphics = graphics;
        _svarogSymbol = svarogSymbol;
        _interactionDeliveryList = interactionDeliveryList;
        _symbolAppearingSoundEffect = symbolAppearingSoundEffect;

        svarogSymbol.AppearingCompleted += (sender, args) =>
        {
            // 1 stage is rising power/burning
            svarogSymbol.SwitchStage(1);
            IsComplete = true;
        };
    }

    public bool CanBeReplaced => true;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        // Nothing to cancel
    }

    public void Update(GameTime gameTime)
    {
        if (!_started)
        {
            _started = true;
            _graphics.PlayAnimation(PredefinedAnimationSid.Skill3);
            _symbolAppearingSoundEffect?.Play();
            _interactionDeliveryList.Add(_svarogSymbol);
        }
    }
}