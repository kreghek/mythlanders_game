using System;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States;

namespace Client.Assets.States;

internal class CommonSelfSkillUsageState : IActorVisualizationState
{
    private readonly AnimationBlocker _mainAnimationBlocker;
    private readonly IActorVisualizationState[] _subStates;

    private int _subStateIndex;

    public CommonSelfSkillUsageState(UnitGraphics graphics, AnimationBlocker mainAnimationBlocker,
        Action interaction,
        SoundEffectInstance hitSound, PredefinedAnimationSid animationSid)
    {
        _mainAnimationBlocker = mainAnimationBlocker;

        _subStates = new IActorVisualizationState[]
        {
            new HealState(graphics, interaction, hitSound, animationSid)
        };
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        if (IsComplete)
        {
            return;
        }

        _mainAnimationBlocker.Release();
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (_subStateIndex < _subStates.Length)
        {
            var currentSubState = _subStates[_subStateIndex];
            if (currentSubState.IsComplete)
            {
                _subStateIndex++;
            }
            else
            {
                currentSubState.Update(gameTime);
            }
        }
        else
        {
            IsComplete = true;
            _mainAnimationBlocker.Release();
        }
    }
}