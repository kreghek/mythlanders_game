using Microsoft.Xna.Framework.Audio;

namespace GameClient.Engine.Animations;

/// <summary>
/// Sound effect bounded with animation frame.
/// </summary>
/// <param name="PlayFrameIndex"> Animation frame index to play the sound effect. </param>
/// <param name="SoundEffect"> Sound effect to play. </param>
public sealed record AnimationSoundEffect(int PlayFrameIndex, SoundEffect SoundEffect);