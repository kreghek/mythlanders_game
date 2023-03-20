using System.Collections.Generic;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

namespace Client.GameScreens.Combat.Ui;
internal sealed class TargetMarkers
{
    private IReadOnlyCollection<CombatMoveTargetEstimate>? _targets;
    private ITargetMarkerContext? _targetMarkerContext;

    public void SetTargets(Combatant actor, IReadOnlyCollection<IEffectInstance> effectInstances, ITargetMarkerContext targetMarkerContext)
    {
        var allTargets = new List<CombatMoveTargetEstimate>();

        foreach (var effect in effectInstances)
        {
            var targets = effect.Selector.GetEstimate(actor, targetMarkerContext.TargetSelectorContext);

            allTargets.AddRange(targets);
        }

        _targets = allTargets;
        _targetMarkerContext = targetMarkerContext;
    }

    public void EriseTargets()
    {
        _targets = null;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_targets is null || _targetMarkerContext is null)
        {
            return;
        }

        var actorGameObject = _targetMarkerContext.GetCombatantGameObject(_targetMarkerContext.CurrentCombatant);

        foreach (var targetEstimate in _targets)
        {
            var gameObject = _targetMarkerContext.GetCombatantGameObject(targetEstimate.Target);
            DrawTargetMarker(spriteBatch,
                actorGameObject.Animator.GraphicRoot.Position,
                gameObject.Animator.GraphicRoot.Position,
                !targetEstimate.Target.IsPlayerControlled);
        }
    }

    private void DrawTargetMarker(SpriteBatch spriteBatch, Vector2 actorPosition, Vector2 targetPosition, bool isAgression)
    {
        const int MARKER_RADIUS = 32;
        const int MARKER_AGGRESSION_RADIUS = 10;
        var neutralColor = Color.Cyan;
        var aggressionColor = Color.Red;

        spriteBatch.DrawCircle(targetPosition, MARKER_RADIUS, 8, isAgression ? aggressionColor : neutralColor, 2);

        if (isAgression)
        {
            spriteBatch.DrawLine(
                targetPosition.X - MARKER_RADIUS - MARKER_AGGRESSION_RADIUS,
                targetPosition.Y,
                targetPosition.X - MARKER_AGGRESSION_RADIUS,
                targetPosition.Y, aggressionColor, 2);

            spriteBatch.DrawLine(
                targetPosition.X + MARKER_RADIUS + MARKER_AGGRESSION_RADIUS,
                targetPosition.Y,
                targetPosition.X + MARKER_AGGRESSION_RADIUS,
                targetPosition.Y, aggressionColor, 2);

            spriteBatch.DrawLine(
                targetPosition.X,
                targetPosition.Y - MARKER_RADIUS - MARKER_AGGRESSION_RADIUS,
                targetPosition.X,
                targetPosition.Y - MARKER_AGGRESSION_RADIUS, aggressionColor, 2);

            spriteBatch.DrawLine(
                targetPosition.X,
                targetPosition.Y + MARKER_RADIUS + MARKER_AGGRESSION_RADIUS,
                targetPosition.X,
                targetPosition.Y + MARKER_AGGRESSION_RADIUS, aggressionColor, 2);
        }

        if (actorPosition != targetPosition)
        {
            spriteBatch.DrawLine(actorPosition, targetPosition, aggressionColor, 2);
        }
    }
}
