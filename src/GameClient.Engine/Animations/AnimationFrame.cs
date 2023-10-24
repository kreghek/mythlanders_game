namespace GameClient.Engine.Animations;

/// <summary>
/// Sound effect bounded with animation frame.
/// </summary>
/// <param name="FrameInfo"> Animation frame to use payload. Usually contains frame info and animation id. </param>
/// <param name="Payload"> Payload to use in key frame. </param>
/// <typeparam name="T"> Type of frame payload. </typeparam>
public sealed record AnimationFrame<T>(IAnimationFrameInfo FrameInfo, T Payload);