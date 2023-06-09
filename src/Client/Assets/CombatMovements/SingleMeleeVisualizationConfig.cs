﻿using Client.Core;

namespace Client.Assets.CombatMovements;

internal sealed record SingleMeleeVisualizationConfig(
    IAnimationFrameSet PrepareMovementAnimation,
    IAnimationFrameSet CombatMovementAnimation,
    IAnimationFrameSet HitAnimation,
    IAnimationFrameSet HitCompleteAnimation,
    IAnimationFrameSet BackAnimation);