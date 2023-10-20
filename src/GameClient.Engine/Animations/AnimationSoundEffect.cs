using Microsoft.Xna.Framework.Audio;

namespace GameClient.Engine.Animations;

/// <summary>
/// Sound effect bounded with animation frame.
/// </summary>
/// <param name="FrameInfo"> Animation frame to play the sound effect. Usually contains frame info and animation id. </param>
/// <param name="SoundEffect"> Sound effect to play. </param>
public sealed record AnimationSoundEffect(IAnimationFrameInfo FrameInfo, IAnimationSoundEffect SoundEffect);