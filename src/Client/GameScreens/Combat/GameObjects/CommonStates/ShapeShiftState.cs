using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

internal class ShapeShiftState : IActorVisualizationState
{
    private readonly UnitGraphics _graphics;
    private readonly AnimationBlocker _shapeShiftBlocker;
    private readonly SoundEffectInstance _soundEffectInstance;
    private bool _isStarted;

    public ShapeShiftState(UnitGraphics graphics,
        SoundEffectInstance soundEffectInstance, AnimationBlocker shapeShiftBlocker)
    {
        _graphics = graphics;
        _soundEffectInstance = soundEffectInstance;
        _shapeShiftBlocker = shapeShiftBlocker;
        //var shapeShiftAnimation = graphics.GetAnimationInfo(PredefinedAnimationSid.ShapeShift);

        //shapeShiftAnimation.End += (_, _) =>
        //{
        //    _graphics.IsDamaged = false;
        //    IsComplete = true;
        //    _shapeShiftBlocker.Release();
        //};
    }

    /// <inheritdoc />
    /// <remarks> The state engine has no blockers. So we can't remove it with no aftermaths. </remarks>
    public bool CanBeReplaced => false;

    /// <summary>
    /// This engine is infinite.
    /// </summary>
    public bool IsComplete { get; }

    public void Cancel()
    {
        _shapeShiftBlocker.Release();
    }

    public void Update(GameTime gameTime)
    {
        if (_isStarted)
        {
            // Just wait until animation will end.
            return;
        }

        _isStarted = true;
        //_graphics.PlayAnimation(PredefinedAnimationSid.ShapeShift);
        _soundEffectInstance.Play();
    }
}