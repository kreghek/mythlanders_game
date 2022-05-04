using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific.Primitives
{
    internal sealed class SvarogSymbolAppearingState : IUnitStateEngine
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
                _graphics.PlayAnimation(AnimationSid.Skill3);
                _symbolAppearingSoundEffect?.Play();
                _interactionDeliveryList.Add(_svarogSymbol);
            }
        }
    }
}