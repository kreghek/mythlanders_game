using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

internal sealed record SoundedAnimation(IAnimationFrameSet Animation, SoundEffectInstance? Sound);