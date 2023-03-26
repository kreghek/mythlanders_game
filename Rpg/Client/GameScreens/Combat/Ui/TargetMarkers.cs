﻿using System;
using System.Collections.Generic;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui;

internal sealed class TargetMarkers
{
    private double _counter;
    private ITargetMarkerContext? _targetMarkerContext;
    private IReadOnlyCollection<CombatMoveTargetEstimate>? _targets;

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
                actorGameObject.InteractionPoint,
                gameObject.InteractionPoint,
                !targetEstimate.Target.IsPlayerControlled);
        }
    }

    public void EriseTargets()
    {
        if (_targets is not null && _targetMarkerContext is not null)
        {
            foreach (var target in _targets)
            {
                var gameObject = _targetMarkerContext.GetCombatantGameObject(target.Target);

                gameObject.Graphics.OutlineMode = OutlineMode.None;
            }
        }

        _targets = null;
    }

    public void SetTargets(Combatant actor, IReadOnlyCollection<IEffectInstance> effectInstances,
        ITargetMarkerContext targetMarkerContext)
    {
        var allTargets = new List<CombatMoveTargetEstimate>();

        foreach (var effect in effectInstances)
        {
            var targets = effect.Selector.GetEstimate(actor, targetMarkerContext.TargetSelectorContext);

            allTargets.AddRange(targets);
        }

        _targets = allTargets;
        _targetMarkerContext = targetMarkerContext;

        foreach (var target in _targets)
        {
            var gameObject = targetMarkerContext.GetCombatantGameObject(target.Target);

            gameObject.Graphics.OutlineMode = OutlineMode.AvailableEnemyTarget;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (_targets is not null)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            _counter = 0;
        }
    }

    private void DrawTargetMarker(SpriteBatch spriteBatch, Vector2 actorPosition, Vector2 targetPosition,
        bool isAgression)
    {
        const int MARKER_RADIUS = 16;
        const int MARKER_RADIUR_DIFF = MARKER_RADIUS / 5;
        const int MARKER_AGGRESSION_RADIUS = 10;
        var neutralColor = Color.Cyan;
        var aggressionColor = Color.Red;

        spriteBatch.DrawCircle(targetPosition,
            (int)(MARKER_RADIUS - MARKER_RADIUR_DIFF * Math.Clamp(Math.Sin(_counter * 15), 0, 1)),
            16,
            isAgression ? aggressionColor : neutralColor,
            2);

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